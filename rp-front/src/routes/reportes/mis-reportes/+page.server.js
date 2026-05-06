import { redirect } from '@sveltejs/kit';

/** @type {import('./$types').PageServerLoad} */
export async function load({ locals, fetch, cookies }) {
    if (!locals.user) {
        throw redirect(303, '/login');
    }

    const accessToken = cookies.get('accessToken');
    const userId = locals.user.id;

    // Usamos el endpoint general con filtro para evitar conflictos de rutas en el backend
    const url = `http://backend:8080/api/Reportes?usuarioId=${userId}`;

    try {
        const response = await fetch(url, {
            headers: { 
                'Authorization': `Bearer ${accessToken}`,
                'Accept': 'application/json'
            }
        });

        if (response.ok) {
            const result = await response.json();
            // Normalizamos la respuesta por si viene envuelta en un objeto
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
