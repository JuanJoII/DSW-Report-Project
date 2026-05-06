import { error } from '@sveltejs/kit';

/** @type {import('./$types').PageServerLoad} */
export async function load({ params, fetch, cookies }) {
    const accessToken = cookies.get('accessToken');

    const response = await fetch(`http://backend:8080/api/Reportes/${params.id}`, {
        headers: {
            'Authorization': accessToken ? `Bearer ${accessToken}` : ''
        }
    });

    if (!response.ok) {
        if (response.status === 404) {
            throw error(404, 'Reporte no encontrado');
        }
        throw error(response.status, 'Error al cargar el reporte');
    }

    const reporte = await response.json();

    return {
        reporte
    };
}
