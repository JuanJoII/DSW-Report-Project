import { redirect } from '@sveltejs/kit';

/** @type {import('./$types').PageServerLoad} */
export async function load({ cookies }) {
  // Eliminar las cookies de sesión
  cookies.delete('accessToken', { path: '/' });
  cookies.delete('refreshToken', { path: '/' });
  cookies.delete('userId', { path: '/' });

  throw redirect(303, '/login');
}
