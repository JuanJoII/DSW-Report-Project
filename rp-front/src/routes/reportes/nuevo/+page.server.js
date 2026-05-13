import { fail, redirect } from '@sveltejs/kit';

/** @type {import('./$types').PageServerLoad} */
export async function load({ locals, fetch }) {
    if (!locals.user) {
        throw redirect(303, '/login');
    }

    const backendUrl = process.env.BACKEND_URL || 'http://backend:8080';
    const response = await fetch(`${backendUrl}/api/Categorias/ListarTodas`);
    
    let categorias = [];
    if (response.ok) {
        categorias = await response.json();
    }

    return {
        categorias
    };
}

/** @type {import('./$types').Actions} */
export const actions = {
    default: async ({ request, fetch, cookies }) => {
        const formData = await request.formData();
        let categoriaId = formData.get('categoriaId');
        const nuevaCategoriaNombre = formData.get('nuevaCategoriaNombre');
        const descripcion = formData.get('descripcion');
        const direccionTexto = formData.get('direccionTexto');
        const latitud = formData.get('latitud');
        const longitud = formData.get('longitud');

        if (!descripcion || !direccionTexto || !latitud || !longitud) {
            return fail(400, { message: 'Todos los campos son obligatorios' });
        }

        const accessToken = cookies.get('accessToken');
        const backendUrl = process.env.BACKEND_URL || 'http://backend:8080';
        const headers = {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        };

        // Si se especificó una nueva categoría
        if (categoriaId === "-1" && nuevaCategoriaNombre) {
            const catResponse = await fetch(`${backendUrl}/api/Categorias`, {
                method: 'POST',
                headers,
                body: JSON.stringify({ nombre: nuevaCategoriaNombre })
            });

            if (catResponse.ok) {
                const newCat = await catResponse.json();
                categoriaId = newCat.id;
            } else if (catResponse.status === 409) {
                // Si ya existe (conflicto), intentamos obtener todas y buscarla por nombre
                // Aunque idealmente el componente ya debería manejar esto, es un fallback
                const allCatsRes = await fetch(`${backendUrl}/api/Categorias/ListarTodas`);
                const allCats = await allCatsRes.json();
                const existing = allCats.find(c => c.nombre.toLowerCase() === nuevaCategoriaNombre.toLowerCase());
                if (existing) categoriaId = existing.id;
            } else {
                return fail(catResponse.status, { message: 'Error al crear la nueva categoría' });
            }
        }

        if (!categoriaId || categoriaId === "null" || categoriaId === "") {
            return fail(400, { message: 'Debes seleccionar o crear una categoría' });
        }

        const response = await fetch(`${backendUrl}/api/Reportes`, {
            method: 'POST',
            headers,
            body: JSON.stringify({
                categoriaId: parseInt(categoriaId),
                descripcion,
                direccionTexto,
                latitud: parseFloat(latitud),
                longitud: parseFloat(longitud)
            })
        });

        if (response.ok) {
            const text = await response.text();
            console.log("DEBUG - Respuesta completa del backend:", text);
            
            let result = {};
            try {
                if (text) result = JSON.parse(text);
                console.log("DEBUG - Objeto parseado:", result);
            } catch (e) {
                console.error("DEBUG - No se pudo parsear JSON:", e.message);
                if (text && !isNaN(Number(text.trim()))) {
                    const cleanId = text.trim();
                    console.log("DEBUG - Usando texto plano como ID:", cleanId);
                    throw redirect(303, `/reportes/nuevo/fotos/${cleanId}`);
                }
            }
            
            // Buscar el ID en el objeto o usar el resultado directamente si es un número/string
            let reportId = null;
            if (typeof result === 'object' && result !== null) {
                reportId = result.id || result.Id || result.reporteId || result.ReporteId || (result.data && (result.data.id || result.data.Id));
            } else if (typeof result === 'number' || (typeof result === 'string' && !isNaN(Number(result)))) {
                reportId = result;
            }
            
            if (reportId) {
                console.log("DEBUG - ID encontrado:", reportId, ". Redirigiendo a fotos.");
                throw redirect(303, `/reportes/nuevo/fotos/${reportId}`);
            }
            
            console.warn("DEBUG - No se encontró ID en la respuesta. Tipo de result:", typeof result);
            throw redirect(303, `/reportes/mis-reportes`);
        } else {
            const error = await response.text();
            return fail(response.status, { message: error || 'Error al crear el reporte' });
        }
    }
};
