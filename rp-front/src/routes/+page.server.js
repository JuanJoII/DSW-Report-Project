/** @type {import('./$types').PageServerLoad} */
export async function load({ fetch }) {
    const response = await fetch('http://backend:8080/api/Reportes?limit=6');
    
    let reportesRecientes = [];
    if (response.ok) {
        reportesRecientes = await response.json();
    }

    return {
        reportesRecientes
    };
}
