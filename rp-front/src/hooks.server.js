import { jwtVerify } from 'jose';
import { JWT_KEY } from '$env/static/private';
import { env } from '$env/dynamic/private';
import { redirect } from '@sveltejs/kit';

export async function handle({ event, resolve }) {
    const accessToken = event.cookies.get("accessToken");
    const refreshToken = event.cookies.get("refreshToken");
    const userId = event.cookies.get("userId");

    event.locals.user = null;

    if (accessToken) {
        try {
            const secret = new TextEncoder().encode(JWT_KEY);
            const { payload } = await jwtVerify(accessToken, secret, {
                algorithms: ['HS512'],
            });

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

        } catch (error) {
            console.error("Error validando token:", error.code);

            if (error.code === 'ERR_JWT_EXPIRED' && refreshToken && userId) {
                const backendUrl = env.BACKEND_URL;

                try {
                    const response = await event.fetch(`${backendUrl}/api/Auth/RefreshTokens`, {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({ userId, refreshToken }),
                    });

                    if (response.ok) {
                        const tokens = await response.json();
                        
                        event.cookies.set("accessToken", tokens.accessToken, { path: "/", httpOnly: true, secure: false, sameSite: 'strict', maxAge: 60 * 15 });
                        event.cookies.set("refreshToken", tokens.refreshToken, { path: "/", httpOnly: true, secure: false, sameSite: 'strict', maxAge: 60 * 60 * 24 * 7 });

                        const secret = new TextEncoder().encode(JWT_KEY);
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
                } catch (refreshError) {
                    console.error("Error al refrescar token:", refreshError);
                }
            } else {
                event.cookies.delete("accessToken", { path: '/' });
                event.cookies.delete("refreshToken", { path: '/' });
            }
        }
    }

    if (event.url.pathname.startsWith("/admin") && event.locals.user?.role?.toLowerCase() !== "admin") {
        throw redirect(303, "/login");
    }

    return resolve(event);
}