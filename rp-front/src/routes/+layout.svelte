<script>
  import { ThemeToggle } from "$lib/index";
  import favicon from "$lib/assets/favicon.svg";
  import "@fontsource/jost/600.css";
  import "@fontsource/jost/700.css";
  import "@fontsource/onest";
  import "@fontsource/onest/500.css";
  import "@fontsource/onest/600.css";
  import "../app.css";

  let { data, children } = $props();
  let user = $derived(data.user);
  let isAdmin = $derived(user?.role?.toLowerCase() === 'admin');
  let isAdminRoute = $derived(data.isAdminRoute);
</script>

<svelte:head>
  <link rel="icon" href={favicon} />
</svelte:head>

{#if isAdminRoute && isAdmin}
  <!-- Layout para Admin -->
  <div class="min-h-screen bg-slate-50">
    {@render children()}
  </div>
{:else}
  <!-- Background Atmosférico "Atardecer en la Sabana" -->
  <div class="fixed inset-0 -z-10 h-full w-full bg-[#fdfaff] dark:bg-slate-950 bg-[radial-gradient(circle_at_20%_80%,_rgba(91,33,182,0.05)_0%,_transparent_50%),_radial-gradient(circle_at_80%_20%,_rgba(234,88,12,0.04)_0%,_transparent_50%),_radial-gradient(circle_at_50%_50%,_rgba(76,29,149,0.02)_0%,_transparent_50%)]"></div>

  <!-- Layout para Ciudadanos -->
  <header class="sticky top-0 z-[1001] bg-white/70 dark:bg-slate-900/70 backdrop-blur-xl border-b border-slate-100 dark:border-slate-800 py-4 transition-all duration-300">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 flex justify-between items-center">
      <a href="/" class="flex items-center gap-2 group">
        <div class="w-10 h-10 bg-gradient-to-br from-sabana-violet to-sabana-purple rounded-xl flex items-center justify-center shadow-lg group-hover:scale-105 transition-transform">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 text-white" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M15 22v-4a4.8 4.8 0 0 0-1-3.5L20 9l-4-4-10 10-3 9h9Z"/><path d="m14 18-4-4"/><path d="m11.5 7.5 3 3"/></svg>
        </div>
        <span class="text-xl font-bold text-slate-900 dark:text-white font-jost tracking-tight hidden sm:block">Reporta <span class="text-sabana-violet">Sabana</span></span>
      </a>

      <nav class="hidden md:flex items-center gap-10">
        <ul class="flex items-center gap-8 list-none m-0 p-0 font-onest font-medium text-slate-600 dark:text-slate-400">
          <li><a href="/" class="hover:text-sabana-violet dark:hover:text-sabana-lila transition-colors relative after:content-[''] after:absolute after:bottom-[-4px] after:left-0 after:w-0 after:h-[2px] after:bg-sabana-violet after:transition-all hover:after:w-full">Inicio</a></li>
          {#if user}
            <li><a href="/reportes/nuevo" class="hover:text-sabana-violet dark:hover:text-sabana-lila transition-colors relative after:content-[''] after:absolute after:bottom-[-4px] after:left-0 after:w-0 after:h-[2px] after:bg-sabana-violet after:transition-all hover:after:w-full">Crear Reporte</a></li>
            <li><a href="/reportes/mis-reportes" class="hover:text-sabana-violet dark:hover:text-sabana-lila transition-colors relative after:content-[''] after:absolute after:bottom-[-4px] after:left-0 after:w-0 after:h-[2px] after:bg-sabana-violet after:transition-all hover:after:w-full">Mis Reportes</a></li>
            {#if isAdmin}
              <li><a href="/admin" class="flex items-center gap-2 text-sabana-violet dark:text-sabana-lila font-semibold bg-violet-50 dark:bg-violet-900/30 px-4 py-2 rounded-full hover:bg-violet-100 transition-all border border-violet-100 dark:border-violet-800">
                <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><rect width="7" height="9" x="3" y="3" rx="1"/><rect width="7" height="5" x="14" y="3" rx="1"/><rect width="7" height="9" x="14" y="12" rx="1"/><rect width="7" height="5" x="3" y="16" rx="1"/></svg>
                Dashboard
              </a></li>
            {/if}
          {/if}
        </ul>

        <div class="h-6 w-[1px] bg-slate-200 dark:bg-slate-700"></div>

        <div class="flex items-center gap-4">
          <ThemeToggle />
          {#if user}
            <div class="flex items-center gap-4">
              <div class="flex flex-col items-end">
                <span class="text-xs text-slate-400 font-bold uppercase tracking-widest">{user.role}</span>
                <span class="text-sm text-slate-900 dark:text-white font-semibold">{user.email.split('@')[0]}</span>
              </div>
              <a href="/logout" data-sveltekit-reload class="p-2.5 text-slate-400 hover:text-red-500 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-full transition-all" title="Cerrar sesión">
                <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><line x1="21" x2="9" y1="12" y2="12"/></svg>
              </a>
            </div>
          {:else}
            <div class="flex items-center gap-2">
              <a href="/login" class="text-slate-600 dark:text-slate-400 hover:text-sabana-violet dark:hover:text-sabana-lila px-5 py-2.5 text-sm font-bold transition-colors">Entrar</a>
              <a href="/register" class="bg-sabana-violet hover:bg-sabana-purple text-white px-6 py-2.5 rounded-full text-sm font-bold transition-all shadow-md shadow-violet-200 dark:shadow-none hover:shadow-lg hover:-translate-y-0.5">Registrarse</a>
            </div>
          {/if}
        </div>
      </nav>

      <!-- Mobile Menu Button (Optional/Simplified) -->
      <button class="md:hidden p-2 text-slate-600 dark:text-slate-400">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><line x1="4" x2="20" y1="12" y2="12"/><line x1="4" x2="20" y1="6" y2="6"/><line x1="4" x2="20" y1="18" y2="18"/></svg>
      </button>
    </div>
  </header>

  <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-10 md:py-16">
    {@render children()}
  </main>

  <footer class="border-t border-slate-100 dark:border-slate-800 bg-white dark:bg-slate-900 py-12 mt-20">
    <div class="max-w-7xl mx-auto px-4 text-center">
      <div class="flex justify-center mb-6">
        <div class="w-8 h-8 bg-slate-200 dark:bg-slate-800 rounded-lg flex items-center justify-center text-slate-400 dark:text-slate-500">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M15 22v-4a4.8 4.8 0 0 0-1-3.5L20 9l-4-4-10 10-3 9h9Z"/><path d="m14 18-4-4"/><path d="m11.5 7.5 3 3"/></svg>
        </div>
      </div>
      <p class="text-slate-400 dark:text-slate-500 text-sm font-onest">© 2026 Reporta Sabana. Una plataforma de participación ciudadana.</p>
    </div>
  </footer>
{/if}

<style>
  /* Clases adicionales si son necesarias */
</style>
