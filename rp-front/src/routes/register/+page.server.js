import { fail, redirect } from '@sveltejs/kit';

/** @type {import('./$types').Actions} */
export const actions = {
  default: async ({ request, fetch }) => {
    const data = await request.formData();
    const email = data.get('email');
    const password = data.get('password');
    const cedula = data.get('cedula');
    const nombreCompleto = data.get('nombreCompleto');
    const telefono = data.get('telefono');
    const direccionResidencia = data.get('direccionResidencia');

    if (!email || !password || !cedula || !nombreCompleto) {
      return fail(400, { missing: true });
    }

    try {
      const response = await fetch('http://backend:8080/api/Auth/Register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ 
          email, 
          password, 
          cedula, 
          nombreCompleto, 
          telefono, 
          direccionResidencia 
        })
      });

      if (!response.ok) {
        const error = await response.json();
        return fail(400, { message: error.message || 'Error en el registro' });
      }

    } catch (err) {
      console.error('Register error:', err);
      return fail(500, { message: 'Error de conexión con el servidor' });
    }

    throw redirect(303, '/login');
  }
};
