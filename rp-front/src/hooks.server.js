import { jwtVerify } from 'jose';
import { JWT_KEY } from '$env/static/private';
import { redirect } from '@sveltejs/kit';

export async function handle({ event, resolve }) {
    const accessToken = event.cookies.get("accessToken");
    const refreshToken = event.cookies.get("refreshToken");
    const userId = event.cookies.get("userId");

    event.locals.user = null;

    if (accessToken) {
        try {
            // 1. Convertimos el string de la llave a bytes, que es lo que pide 'jose'
            const secret = new TextEncoder().encode(JWT_KEY);

            // 2. Verificamos la firma matemáticamente
            const { payload } = await jwtVerify(accessToken, secret, {
                algorithms: ['HS512'],
            });

            // console.log("JWT Payload:", payload);

            // 3. Extraemos claims, manejando los namespaces por defecto de .NET
            const email = payload.email || 
                          payload.sub || 
                          payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
            
            const role = payload.role || 
                         payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || 
                         "ciudadano";

            event.locals.user = {
                id: userId,
                email,
                role
            };

            // console.log("Token válido para usuario:", event.locals.user.email, event.locals.user.role);

        } catch (error) {
            // Si el token fue manipulado (firma inválida) o expiró, cae aquí.
            console.error("Error validando token:", error.code);

            // Si el error es específicamente por expiración, intentamos refrescar
            if (error.code === 'ERR_JWT_EXPIRED' && refreshToken && userId) {
                
                const response = await event.fetch("http://backend:8080/api/Auth/RefreshTokens", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ userId, refreshToken }),
                });

                if (response.ok) {
                    const tokens = await response.json();
                    
                    event.cookies.set("accessToken", tokens.accessToken, { path: "/", httpOnly: true, secure: false, sameSite: 'strict', maxAge: 60 * 15 });
                    event.cookies.set("refreshToken", tokens.refreshToken, { path: "/", httpOnly: true, secure: false, sameSite: 'strict', maxAge: 60 * 60 * 24 * 7 });

                    // Verificamos el NUEVO token recién llegado
                    const { payload: newPayload } = await jwtVerify(tokens.accessToken, secret, { algorithms: ['HS512'] });
                    
                    const newEmail = newPayload.email || 
                                     newPayload.sub || 
                                     newPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
                    
                    const newRole = newPayload.role || 
                                    newPayload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || 
                                    "ciudadano";

                    event.locals.user = {
                        id: userId,
                        email: newEmail,
                        role: newRole,
                    };
                }
            } else {
                // Si la firma es totalmente inválida (fue manipulada), borramos las cookies
                event.cookies.delete("accessToken", { path: '/' });
                event.cookies.delete("refreshToken", { path: '/' });
            }
        }
    }

    // Protección de rutas: Si la URL exige admin y el usuario no lo es, pa' fuera
    if (event.url.pathname.startsWith("/admin") && event.locals.user?.role !== "admin") {
        throw redirect(303, "/login");
    }

    return resolve(event);
}