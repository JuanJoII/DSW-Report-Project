import { fail, redirect } from '@sveltejs/kit';

/** @type {import('./$types').PageServerLoad} */
export async function load({ params, locals, fetch }) {
    if (!locals.user) {
        throw redirect(303, '/login');
    }

    const { id } = params;
    console.log("Cargando vista de fotos para reporte ID:", id);

    try {
        const response = await fetch(`http://backend:8080/api/Reportes/${id}`);
        
        if (!response.ok) {
            console.error(`No se pudo obtener el detalle del reporte ${id}. Status: ${response.status}`);
            // Fallback: devolvemos un objeto mínimo con el ID para no bloquear la subida de fotos
            return {
                reporte: { id, descripcion: "Reporte recién creado" }
            };
        }

        const reporte = await response.json();
        return {
            reporte
        };
    } catch (err) {
        console.error(`Error de red al verificar reporte ${id}:`, err);
        return {
            reporte: { id, descripcion: "Reporte recién creado" }
        };
    }
}

/** @type {import('./$types').Actions} */
export const actions = {
    getPresignedUrl: async ({ fetch, cookies, url: pageUrl }) => {
        const fileName = pageUrl.searchParams.get('fileName');
        const accessToken = cookies.get('accessToken');

        const response = await fetch(`http://backend:8080/api/FotoReporte/presignedUrl?fileName=${encodeURIComponent(fileName)}`, {
            headers: {
                'Authorization': `Bearer ${accessToken}`
            }
        });

        if (!response.ok) {
            return fail(response.status, { message: 'Error al obtener URL firmada' });
        }

        return await response.json();
    },

    registerPhoto: async ({ request, fetch, cookies, params }) => {
        const { id } = params;
        const formData = await request.formData();
        const url = formData.get('url');
        const accessToken = cookies.get('accessToken');

        const response = await fetch('http://backend:8080/api/FotoReporte', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${accessToken}`
            },
            body: JSON.stringify({
                reporteId: parseInt(id),
                url: url.toString()
            })
        });

        if (!response.ok) {
            const errorText = await response.text();
            return fail(response.status, { message: errorText });
        }

        return { success: true };
    }
};
