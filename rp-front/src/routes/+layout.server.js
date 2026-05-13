/** @type {import('./$types').LayoutServerLoad} */
export async function load({ locals, url }) {
  return {
    user: locals.user,
    isAdminRoute: url.pathname.startsWith('/admin')
  };
}
