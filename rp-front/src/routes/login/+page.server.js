import { fail, redirect } from '@sveltejs/kit';
import { env } from '$env/dynamic/private';

/** @type {import('./$types').Actions} */
export const actions = {
  default: async ({ request, cookies, fetch }) => {
    const data = await request.formData();
    const email = data.get('email');
    const password = data.get('password');

    if (!email || !password) {
      return fail(400, { email, missing: true });
    }

    try {
      const backendUrl = env.BACKEND_URL;
      const response = await fetch(`${backendUrl}/api/Auth/Login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
      });

      if (!response.ok) {
        return fail(401, { email, incorrect: true });
      }

      const result = await response.json();

      // Guardar en cookies seguras
      cookies.set('accessToken', result.accessToken, { 
        path: '/', 
        httpOnly: true, 
        secure: false, // Cambiar a true en producción con HTTPS
        sameSite: 'strict',
        maxAge: 60 * 15 // 15 minutos
      });

      cookies.set('refreshToken', result.refreshToken, { 
        path: '/', 
        httpOnly: true, 
        secure: false, 
        sameSite: 'strict',
        maxAge: 60 * 60 * 24 * 7 // 7 días
      });

      cookies.set('userId', result.id, { 
        path: '/', 
        httpOnly: true, 
        secure: false, 
        sameSite: 'strict',
        maxAge: 60 * 60 * 24 * 7
      });

    } catch (err) {
      console.error('Login error:', err);
      return fail(500, { message: 'Error de conexión con el servidor' });
    }

    throw redirect(303, '/');
  }
};
