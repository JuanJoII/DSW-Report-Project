import { env } from '$env/dynamic/private';

/** @type {import('./$types').PageServerLoad} */
export async function load({ fetch, locals, cookies }) {
  // Verificar que el usuario sea admin (redundancia de seguridad)
  if (locals.user?.role?.toLowerCase() !== 'admin') {
    return {
      reportes: [],
      error: 'No tienes permisos para acceder a esta página'
    };
  }

  const accessToken = cookies.get('accessToken');
  const backendUrl = env.BACKEND_URL;

  try {
    // Obtener TODOS los reportes del sistema
    const response = await fetch(`${backendUrl}/api/Reportes`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${accessToken}`
      }
    });

    if (!response.ok) {
      console.error(`Error en API Reportes: ${response.status}`);
      throw new Error(`Error del servidor: ${response.status}`);
    }

    const reportes = await response.json();
    const reportesArray = Array.isArray(reportes) ? reportes : (reportes.reportes || reportes.data || []);

    // Obtener todos los estados disponibles
    let estadosArray = [];
    try {
      const estadosResponse = await fetch(`${backendUrl}/api/Estados/ListarTodos`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${accessToken}`
        }
      });

      if (estadosResponse.ok) {
        const estados = await estadosResponse.json();
        estadosArray = Array.isArray(estados) ? estados : (estados.estados || estados.data || []);
      }
    } catch (eError) {
      console.error('Error fetching estados:', eError);
    }

    return {
      reportes: reportesArray,
      estados: estadosArray,
      error: null
    };
  } catch (error) {
    console.error('Error loading admin page data:', error);
    return {
      reportes: [],
      estados: [],
      error: 'Error al cargar los datos: ' + error.message
    };
  }
}

/** @type {import('./$types').Actions} */
export const actions = {
  cambiarEstado: async ({ request, cookies, fetch }) => {
    const data = await request.formData();
    const reporteId = data.get('reporteId');
    const estadoId = data.get('estadoId');
    const accessToken = cookies.get('accessToken');
    const backendUrl = env.BACKEND_URL;

    console.log('Action cambiarEstado - reporteId:', reporteId, 'estadoId:', estadoId);
    console.log('Token disponible:', !!accessToken);

    if (!reporteId || !estadoId) {
      return { success: false, error: 'Datos inválidos' };
    }

    if (!accessToken) {
      return { success: false, error: 'No autenticado' };
    }

    try {
      const url = `${backendUrl}/api/Reportes/${reporteId}/estado/${estadoId}`;
      console.log('Llamando a:', url);

      const response = await fetch(url, {
        method: 'PATCH',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${accessToken}`
        }
      });

      console.log('Response status:', response.status);

      if (!response.ok) {
        const errorText = await response.text();
        console.error('Error response:', errorText);
        throw new Error(`Error ${response.status}: ${errorText}`);
      }

      const text = await response.text();
      console.log('Response text:', text);

      let result;
      try {
        result = JSON.parse(text);
      } catch (parseError) {
        console.error('Error al parsear JSON:', parseError, 'Texto:', text);
        return { success: false, error: 'Respuesta inválida del servidor: ' + text };
      }

      console.log('Cambio exitoso:', result);
      return { success: true, reporte: result };
    } catch (error) {
      console.error('Error al cambiar estado:', error);
      return { success: false, error: error.message || 'Error desconocido al cambiar el estado' };
    }
  }
};
