/** @type {import('./$types').PageServerLoad} */
export async function load({ fetch, locals, cookies }) {
    const userRole = locals.user?.role;
    const isAdmin = userRole?.toLowerCase() === 'admin';
    let reportesRecientes = [];
    const backendUrl = process.env.BACKEND_URL || 'http://backend:8080';

    // Solo admin ve los reportes recientes en home
    if (isAdmin) {
        try {
            const accessToken = cookies.get('accessToken');
            const response = await fetch(`${backendUrl}/api/Reportes?limit=6`, {
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Accept': 'application/json'
                }
            });

            if (response.ok) {
                const result = await response.json();
                reportesRecientes = Array.isArray(result) ? result : (result.reportes || result.data || []);
            }
        } catch (err) {
            console.error("Error al cargar reportes recientes:", err);
        }
    }

    return {
        reportesRecientes
    };
}
