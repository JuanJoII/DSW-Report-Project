<script>
  let { data, children } = $props();
  let user = $derived(data.user);
  let sidebarOpen = $state(true);
</script>

<div class="flex min-h-screen bg-slate-50 dark:bg-slate-950 font-onest transition-colors duration-300">
  <!-- Sidebar -->
  <aside 
    class="fixed inset-y-0 left-0 z-50 bg-slate-900 dark:bg-slate-950 text-slate-400 transition-all duration-300 flex flex-col shadow-2xl border-r border-slate-800"
    class:w-64={sidebarOpen}
    class:w-20={!sidebarOpen}
  >
    <!-- Header -->
    <div class="h-20 flex items-center px-6 border-b border-slate-800/50 mb-6">
      <div class="w-8 h-8 bg-sabana-violet rounded-lg flex-shrink-0 flex items-center justify-center text-white shadow-lg shadow-sabana-violet/20">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><path d="M15 22v-4a4.8 4.8 0 0 0-1-3.5L20 9l-4-4-10 10-3 9h9Z"/><path d="m14 18-4-4"/><path d="m11.5 7.5 3 3"/></svg>
      </div>
      {#if sidebarOpen}
        <span class="ml-3 font-jost font-bold text-white tracking-tight text-lg overflow-hidden whitespace-nowrap">Admin <span class="text-sabana-violet">Sabana</span></span>
      {/if}
    </div>

    <!-- Navigation -->
    <nav class="flex-1 px-3 space-y-1">
      <a href="/" class="flex items-center px-3 py-3 rounded-xl hover:bg-slate-800 hover:text-white transition-all group">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 flex-shrink-0" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="m3 9 9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/></svg>
        {#if sidebarOpen}
          <span class="ml-3 font-medium overflow-hidden whitespace-nowrap">Vista Pública</span>
        {/if}
      </a>
      
      <div class="pt-4 pb-2">
        {#if sidebarOpen}
          <span class="px-4 text-[10px] font-bold uppercase tracking-[0.2em] text-slate-500">Gestión</span>
        {:else}
          <div class="h-[1px] bg-slate-800 mx-2"></div>
        {/if}
      </div>

      <a href="/admin" class="flex items-center px-3 py-3 rounded-xl bg-sabana-violet/10 text-sabana-violet border border-sabana-violet/20 font-bold group">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 flex-shrink-0" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><rect width="7" height="9" x="3" y="3" rx="1"/><rect width="7" height="5" x="14" y="3" rx="1"/><rect width="7" height="9" x="14" y="12" rx="1"/><rect width="7" height="5" x="3" y="16" rx="1"/></svg>
        {#if sidebarOpen}
          <span class="ml-3 overflow-hidden whitespace-nowrap">Reportes</span>
        {/if}
      </a>
    </nav>

    <!-- Footer -->
    <div class="p-4 border-t border-slate-800/50 bg-slate-900/50">
      <div class="flex items-center gap-3 mb-4">
        <div class="w-10 h-10 rounded-full bg-gradient-to-br from-slate-700 to-slate-800 border border-slate-700 flex items-center justify-center text-white font-bold flex-shrink-0">
          {user?.email?.[0].toUpperCase()}
        </div>
        {#if sidebarOpen}
          <div class="overflow-hidden">
            <p class="text-sm font-bold text-white truncate">{user?.email.split('@')[0]}</p>
            <p class="text-[10px] text-slate-500 uppercase tracking-widest">Administrador</p>
          </div>
        {/if}
      </div>
      
      <button 
        onclick={() => (sidebarOpen = !sidebarOpen)}
        class="w-full flex items-center justify-center p-2 rounded-lg hover:bg-slate-800 transition-colors text-slate-500 hover:text-white"
        title={sidebarOpen ? "Colapsar" : "Expandir"}
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 transition-transform duration-300" class:rotate-180={!sidebarOpen} viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="m15 18-6-6 6-6"/></svg>
      </button>

      <a href="/logout" data-sveltekit-reload class="mt-4 flex items-center justify-center gap-2 w-full py-2.5 rounded-xl bg-red-500/10 text-red-500 hover:bg-red-500 hover:text-white transition-all font-bold text-sm">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><line x1="21" x2="9" y1="12" y2="12"/></svg>
        {#if sidebarOpen}<span>Salir</span>{/if}
      </a>
    </div>
  </aside>

  <!-- Main Content -->
  <main 
    class="flex-1 flex flex-col min-w-0 transition-all duration-300"
    style:margin-left={sidebarOpen ? '16rem' : '5rem'}
  >
    <!-- Top Header -->
    <header class="h-20 bg-white/80 dark:bg-slate-900/80 border-b border-slate-100 dark:border-slate-800 flex items-center justify-between px-8 sticky top-0 z-40 backdrop-blur-md">
      <div class="flex items-center gap-4">
        <h1 class="text-2xl font-bold text-slate-900 dark:text-white font-jost">Dashboard de Gestión</h1>
        <div class="h-6 w-[1px] bg-slate-200 dark:bg-slate-700"></div>
        <div class="flex items-center gap-2 text-slate-400 dark:text-slate-500 text-sm font-medium">
          <span>Administración</span>
          <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="m9 18 6-6-6-6"/></svg>
          <span class="text-slate-900 dark:text-slate-300">Reportes</span>
        </div>
      </div>

      <div class="flex items-center gap-4">
        <button class="p-2 text-slate-400 dark:text-slate-500 hover:text-slate-600 dark:hover:text-slate-300 rounded-lg hover:bg-slate-50 dark:hover:bg-slate-800 transition-colors relative">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M6 8a6 6 0 0 1 12 0c0 7 3 9 3 9H3s3-2 3-9"/><path d="M10.3 21a1.94 1.94 0 0 0 3.4 0"/></svg>
          <span class="absolute top-2 right-2 w-2 h-2 bg-red-500 rounded-full border-2 border-white dark:border-slate-900"></span>
        </button>
      </div>
    </header>

    <div class="p-8">
      {@render children()}
    </div>
  </main>
</div>

<style>
  :global(body) {
    background-color: var(--bg-color);
  }
</style>
