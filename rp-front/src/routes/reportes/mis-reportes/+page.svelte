<script lang="ts">
    let { data } = $props();
    let reportes = $derived(data.reportes || []);

    function formatDate(dateString: string) {
        if (!dateString) return "Sin fecha";
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

<div class="max-w-6xl mx-auto space-y-12 pb-20 animate-in fade-in duration-500">
    <div class="flex flex-col md:flex-row justify-between items-start md:items-center gap-6">
        <div class="space-y-2">
            <h1 class="text-4xl md:text-5xl font-bold text-slate-900 dark:text-white font-jost leading-tight">Mis Reportes</h1>
            <p class="text-slate-500 dark:text-slate-400 font-onest text-lg">Sigue el progreso de tus contribuciones a la ciudad.</p>
        </div>
        <a href="/reportes/nuevo" class="group bg-sabana-violet text-white hover:bg-sabana-purple px-8 py-4 rounded-full text-lg font-bold transition-all shadow-xl shadow-violet-200 dark:shadow-none hover:scale-105 font-onest flex items-center gap-3">
            Nuevo Reporte
            <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 group-hover:translate-x-1 transition-transform" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><line x1="12" x2="12" y1="5" y2="19"/><line x1="5" x2="19" y1="12" y2="12"/></svg>
        </a>
    </div>

    {#if !reportes || reportes.length === 0}
        <div class="text-center py-24 bg-white dark:bg-slate-800 rounded-[2.5rem] border border-slate-100 dark:border-slate-700 shadow-sm">
            <div class="w-24 h-24 bg-slate-50 dark:bg-slate-700 rounded-full flex items-center justify-center mx-auto mb-8 text-slate-300 dark:text-slate-500">
                <svg xmlns="http://www.w3.org/2000/svg" class="w-12 h-12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect width="18" height="18" x="3" y="3" rx="2"/><path d="M3 9h18"/><path d="M9 21V9"/></svg>
            </div>
            <h2 class="text-2xl font-bold text-slate-900 dark:text-white font-jost mb-4">Aún no tienes reportes</h2>
            <p class="text-slate-500 dark:text-slate-400 font-onest max-w-md mx-auto mb-10 leading-relaxed">
                Cuando crees un reporte de algún daño en la vía pública, aparecerá aquí para que puedas seguir su estado de gestión.
            </p>
            <a href="/reportes/nuevo" class="inline-flex items-center gap-3 bg-violet-50 dark:bg-violet-900/30 text-sabana-violet dark:text-sabana-lila hover:bg-violet-100 dark:hover:bg-violet-900/50 px-10 py-5 rounded-full text-lg font-bold transition-all font-onest">
                Crear mi primer reporte
                <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><line x1="5" x2="19" y1="12" y2="12"/><polyline points="12 5 19 12 12 19"/></svg>
            </a>
        </div>
    {:else}
        <div class="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
            {#each reportes as reporte}
                {@const status = getStatusConfig(reporte.nombreEstado)}
                <div class="bg-white dark:bg-slate-800 p-8 rounded-[2rem] border border-slate-100 dark:border-slate-700 shadow-sm hover:shadow-xl hover:shadow-violet-500/5 hover:-translate-y-1 transition-all group flex flex-col h-full">
                    <div class="flex items-center justify-between mb-6">
                        <span class="text-xs font-bold text-sabana-violet dark:text-sabana-lila uppercase tracking-widest font-onest bg-violet-50 dark:bg-violet-900/30 px-3 py-1 rounded-lg">{reporte.nombreCategoria || 'Sin categoría'}</span>
                        <span class="text-xs text-slate-400 dark:text-slate-500 font-medium font-onest">{formatDate(reporte.fechaCreacion)}</span>
                    </div>

                    <h3 class="text-xl font-bold text-slate-900 dark:text-white mb-6 font-jost group-hover:text-sabana-violet dark:group-hover:text-sabana-lila transition-colors flex-grow">
                        {reporte.descripcion?.length > 80 ? reporte.descripcion.substring(0, 80) + '...' : (reporte.descripcion || '')}
                    </h3>

                    <div class="flex items-center gap-3 text-slate-500 dark:text-slate-400 mb-8 p-4 bg-slate-50 dark:bg-slate-900/50 rounded-2xl border border-slate-100/50 dark:border-slate-700/50">
                        <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4 text-slate-400 dark:text-slate-500 shrink-0" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z"/><circle cx="12" cy="10" r="3"/></svg>
                        <span class="text-sm font-onest truncate" title={reporte.direccionTexto}>{reporte.direccionTexto}</span>
                    </div>

                    <div class="flex items-center justify-between mt-auto pt-6 border-t border-slate-50 dark:border-slate-700">
                        <span class="inline-flex items-center gap-2 px-4 py-1.5 rounded-full text-[10px] font-bold uppercase tracking-widest font-onest border {status.class} dark:bg-opacity-10">
                            <span class="w-1.5 h-1.5 rounded-full {status.dot}"></span>
                            {reporte.nombreEstado || 'Pendiente'}
                        </span>
                        
                        <a href="/reportes/{reporte.id}" class="w-10 h-10 rounded-full bg-slate-50 dark:bg-slate-700 flex items-center justify-center text-slate-400 dark:text-slate-500 hover:bg-sabana-violet dark:hover:bg-sabana-purple hover:text-white transition-all shadow-sm">
                            <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><path d="m9 18 6-6-6-6"/></svg>
                        </a>
                    </div>
                </div>
            {/each}
        </div>
    {/if}
</div>

<style>
    @reference "../../../app.css";
</style>
