<script lang="ts">
  let { data } = $props();
  let isAdmin = $derived(data.user?.role?.toLowerCase() === 'admin');

  function formatDate(dateString: string) {
    return new Date(dateString).toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  function getStatusConfig(status: string) {
    const s = status?.toLowerCase() || '';
    if (s.includes('pendiente')) return { class: 'bg-cyan-50 text-cyan-600 border-cyan-100', dot: 'bg-cyan-500' };
    if (s.includes('revisión') || s.includes('proceso')) return { class: 'bg-amber-50 text-amber-600 border-amber-100', dot: 'bg-amber-500' };
    if (s.includes('resuelto')) return { class: 'bg-emerald-50 text-emerald-600 border-emerald-100', dot: 'bg-emerald-500' };
    if (s.includes('rechazado') || s.includes('error')) return { class: 'bg-red-50 text-red-600 border-red-100', dot: 'bg-red-500' };
    return { class: 'bg-slate-50 text-slate-600 border-slate-100', dot: 'bg-slate-400' };
  }
</script>

<div class="relative overflow-hidden rounded-[2.5rem] bg-gradient-to-br from-sabana-purple via-sabana-violet to-violet-600 text-white mb-20 shadow-2xl shadow-violet-200 dark:shadow-none">
  <!-- Círculos decorativos para el concepto orgánico -->
  <div class="absolute -top-24 -right-24 w-96 h-96 bg-white/10 rounded-full blur-3xl animate-pulse"></div>
  <div class="absolute -bottom-24 -left-24 w-72 h-72 bg-sabana-lila/10 rounded-full blur-3xl"></div>
  <div class="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[800px] h-[800px] bg-white/5 rounded-full blur-[100px] pointer-events-none"></div>

  <div class="relative px-8 py-24 md:py-36 text-center max-w-4xl mx-auto z-10">
    <div class="inline-flex items-center gap-2 bg-white/10 backdrop-blur-md px-4 py-2 rounded-full mb-8 border border-white/20">
      <span class="w-2 h-2 bg-emerald-400 rounded-full animate-ping"></span>
      <span class="text-xs font-bold uppercase tracking-[0.2em] font-onest">Participación Ciudadana</span>
    </div>
    
    <h1 class="text-5xl md:text-7xl font-bold mb-8 leading-[1.1] font-jost">
      Tu voz mejora <br/> <span class="text-sabana-lila">nuestra ciudad</span>
    </h1>
    
    <p class="text-lg md:text-xl mb-12 opacity-90 font-onest max-w-2xl mx-auto leading-relaxed">
      Reporta baches, fallas en iluminación o cualquier inconveniente en la vía pública. Cerramos la brecha entre ciudadanos y administración.
    </p>
    
    <div class="flex flex-col sm:flex-row gap-5 justify-center items-center">
      <a href="/reportes/nuevo" class="group bg-white text-sabana-purple hover:bg-sabana-lila px-10 py-5 rounded-full text-lg font-bold transition-all shadow-xl hover:scale-105 font-onest flex items-center gap-3">
        Crear Reporte
        <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 group-hover:translate-x-1 transition-transform" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><line x1="12" x2="12" y1="5" y2="19"/><line x1="5" x2="19" y1="12" y2="12"/></svg>
      </a>
      <a href="#recientes" class="bg-white/10 border-2 border-white/20 hover:bg-white/20 px-10 py-5 rounded-full text-lg font-bold transition-all backdrop-blur-sm font-onest">
        Ver Reportes
      </a>
    </div>
  </div>
</div>

<section id="recientes" class="scroll-mt-32">
  <div class="flex flex-col md:flex-row md:items-end justify-between mb-12 gap-6">
    <div class="max-w-xl">
      <div class="flex items-center gap-3 mb-2">
        <h2 class="text-4xl font-bold text-slate-900 dark:text-white font-jost">Reportes Recientes</h2>
        <div class="h-[2px] flex-1 bg-gradient-to-r from-sabana-violet/20 to-transparent"></div>
      </div>
      <p class="text-slate-500 dark:text-slate-400 font-onest text-lg leading-relaxed">Observa la actividad ciudadana en tiempo real y el progreso de la gestión municipal.</p>
    </div>
    {#if !isAdmin}
      <a href="/login" class="text-sabana-violet dark:text-sabana-lila font-bold flex items-center gap-2 group font-onest">
        Inicia sesión para ver más
        <span class="w-8 h-8 rounded-full bg-violet-50 dark:bg-violet-900/30 flex items-center justify-center group-hover:bg-violet-100 dark:group-hover:bg-violet-900/50 transition-colors">→</span>
      </a>
    {/if}
  </div>

  {#if data.reportesRecientes.length === 0}
    <div class="text-center py-24 bg-slate-50 dark:bg-slate-900/50 rounded-[2rem] border-2 border-dashed border-slate-200 dark:border-slate-800">
      <div class="w-20 h-20 bg-slate-100 dark:bg-slate-800 rounded-full flex items-center justify-center mx-auto mb-6 text-slate-300 dark:text-slate-600">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-10 h-10" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect width="18" height="18" x="3" y="3" rx="2"/><path d="M3 9h18"/><path d="M9 21V9"/></svg>
      </div>
      <p class="text-slate-400 dark:text-slate-500 font-onest text-lg">No hay reportes registrados recientemente.</p>
    </div>
  {:else}
    <div class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
      {#each data.reportesRecientes as reporte}
        {@const status = getStatusConfig(reporte.nombreEstado)}
        <div class="bg-white dark:bg-slate-800 p-8 rounded-[2rem] border border-slate-100 dark:border-slate-700 shadow-sm hover:shadow-xl hover:shadow-violet-500/5 hover:-translate-y-1 transition-all group flex flex-col h-full">
          <div class="flex items-center justify-between mb-6">
            <div class="flex items-center gap-2">
              <span class="text-xs font-bold text-sabana-violet dark:text-sabana-lila uppercase tracking-widest font-onest bg-violet-50 dark:bg-violet-900/30 px-3 py-1 rounded-lg">{reporte.nombreCategoria}</span>
            </div>
            <span class="text-xs text-slate-400 dark:text-slate-500 font-medium font-onest">{formatDate(reporte.fechaCreacion)}</span>
          </div>

          <h3 class="text-xl font-bold text-slate-900 dark:text-white mb-4 font-jost group-hover:text-sabana-violet dark:group-hover:text-sabana-lila transition-colors flex-grow">
            {reporte.descripcion?.length > 80 ? reporte.descripcion.substring(0, 80) + '...' : (reporte.descripcion || '')}
          </h3>

          <div class="flex items-center gap-2 text-slate-500 dark:text-slate-400 mb-8">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4 text-slate-400 dark:text-slate-500" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z"/><circle cx="12" cy="10" r="3"/></svg>
            <span class="text-sm font-onest truncate">{reporte.direccionTexto}</span>
          </div>

          <div class="flex items-center justify-between mt-auto pt-6 border-t border-slate-50 dark:border-slate-700">
            <span class="inline-flex items-center gap-2 px-4 py-1.5 rounded-full text-xs font-bold uppercase tracking-widest font-onest border {status.class} dark:bg-opacity-10">
              <span class="w-2 h-2 rounded-full {status.dot}"></span>
              {reporte.nombreEstado}
            </span>
            
            <a href="/reportes/{reporte.id}" class="w-10 h-10 rounded-full bg-slate-50 dark:bg-slate-700 flex items-center justify-center text-slate-400 dark:text-slate-500 hover:bg-sabana-violet dark:hover:bg-sabana-purple hover:text-white transition-all">
              <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><path d="m9 18 6-6-6-6"/></svg>
            </a>
          </div>
        </div>
      {/each}
    </div>
  {/if}
</section>

<style>
  @reference "../app.css";
</style>
