import { redirect } from '@sveltejs/kit';
import { env } from '$env/dynamic/private';

/** @type {import('./$types').PageServerLoad} */
export async function load({ locals, fetch, cookies }) {
    if (!locals.user) {
        throw redirect(303, '/login');
    }

    const accessToken = cookies.get('accessToken');
    const backendUrl = env.BACKEND_URL;

    // SIEMPRE traer solo los reportes del usuario logeado
    // (incluso si es admin, en esta vista ve solo los suyos)
    const url = `${backendUrl}/api/Reportes/usuario/mis-reportes`;

    try {
        const response = await fetch(url, {
            headers: {
                'Authorization': `Bearer ${accessToken}`,
                'Accept': 'application/json'
            }
        });

        if (response.ok) {
            const result = await response.json();
            const reportes = Array.isArray(result) ? result : (result.reportes || result.data || []);
            return { reportes };
        } else {
            console.error(`Error al cargar reportes (${response.status})`);
            return { reportes: [] };
        }
    } catch (err) {
        console.error("Error de conexión con el backend:", err.message);
        return { reportes: [] };
    }
}
