<script lang="ts">
    import { MapStatic } from "$lib/index";

    let { data } = $props();
    const { reporte, fromAdmin } = data;

    // Determinar a dónde volver basado en fromAdmin
    const backLink = fromAdmin ? '/admin' : '/reportes/mis-reportes';
    const backLabel = fromAdmin ? 'Volver al panel' : 'Mis reportes';

    function formatDate(dateString: string) {
        return new Date(dateString).toLocaleDateString('es-ES', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    function getStatusConfig(status: string) {
        const s = status?.toLowerCase() || '';
        if (s.includes('pendiente')) return { class: 'bg-cyan-50 text-cyan-600 border-cyan-100 dark:bg-cyan-900/20 dark:text-cyan-400 dark:border-cyan-800', dot: 'bg-cyan-500' };
        if (s.includes('revisión') || s.includes('proceso')) return { class: 'bg-amber-50 text-amber-600 border-amber-100 dark:bg-amber-900/20 dark:text-amber-400 dark:border-amber-800', dot: 'bg-amber-500' };
        if (s.includes('resuelto')) return { class: 'bg-emerald-50 text-emerald-600 border-emerald-100 dark:bg-emerald-900/20 dark:text-emerald-400 dark:border-emerald-800', dot: 'bg-emerald-500' };
        if (s.includes('rechazado') || s.includes('error')) return { class: 'bg-red-50 text-red-600 border-red-100 dark:bg-red-900/20 dark:text-red-400 dark:border-red-800', dot: 'bg-red-500' };
        return { class: 'bg-slate-50 text-slate-600 border-slate-100 dark:bg-slate-900/20 dark:text-slate-400 dark:border-slate-800', dot: 'bg-slate-400' };
    }

    const statusConfig = $derived(getStatusConfig(reporte.nombreEstado));
</script>

<div class="max-w-6xl mx-auto space-y-8 pb-20 animate-in fade-in duration-500">
    <div class="flex items-center gap-4">
        <a href={backLink} class="group flex items-center gap-2 text-slate-400 dark:text-slate-500 hover:text-sabana-violet dark:hover:text-sabana-lila transition-colors font-onest font-bold text-sm">
            <span class="w-8 h-8 rounded-full bg-slate-100 dark:bg-slate-800 flex items-center justify-center group-hover:bg-violet-50 dark:group-hover:bg-violet-900/30 group-hover:text-sabana-violet dark:group-hover:text-sabana-lila transition-all">←</span>
            {backLabel}
        </a>
    </div>

    <header class="flex flex-col md:flex-row justify-between items-start gap-6">
        <div class="space-y-2">
            <div class="flex items-center gap-3">
                <span class="text-xs font-bold text-sabana-violet dark:text-sabana-lila uppercase tracking-[0.2em] bg-violet-50 dark:bg-violet-900/30 px-3 py-1 rounded-lg border border-violet-100 dark:border-violet-800">{reporte.nombreCategoria}</span>
                <span class="text-slate-300 dark:text-slate-700">•</span>
                <span class="text-xs text-slate-400 dark:text-slate-500 font-bold uppercase tracking-widest">ID: {reporte.id}</span>
            </div>
            <h1 class="text-4xl md:text-5xl font-bold text-slate-900 dark:text-white font-jost leading-tight">Detalles del Reporte</h1>
        </div>
        
        <div class="inline-flex items-center gap-3 px-6 py-3 rounded-full border shadow-sm {statusConfig.class} dark:bg-opacity-10">
            <span class="w-3 h-3 rounded-full {statusConfig.dot} animate-pulse"></span>
            <span class="text-sm font-bold uppercase tracking-[0.1em]">{reporte.nombreEstado}</span>
        </div>
    </header>

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div class="lg:col-span-2 space-y-8">
            <!-- Información Principal -->
            <section class="bg-white dark:bg-slate-800 p-10 rounded-[2.5rem] shadow-sm border border-slate-100 dark:border-slate-700 space-y-8">
                <div class="space-y-4">
                    <h2 class="text-2xl font-bold text-slate-900 dark:text-white font-jost flex items-center gap-3">
                        <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 text-sabana-violet dark:text-sabana-lila" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="16" x2="8" y1="13" y2="13"/><line x1="16" x2="8" y1="17" y2="17"/><polyline points="10 9 9 9 8 9"/></svg>
                        Descripción
                    </h2>
                    <p class="text-slate-600 dark:text-slate-300 font-onest text-lg leading-relaxed bg-slate-50 dark:bg-slate-900/50 p-6 rounded-2xl border border-slate-100/50 dark:border-slate-700/50 italic">
                        "{reporte.descripcion}"
                    </p>
                </div>
                
                <div class="grid grid-cols-1 sm:grid-cols-2 gap-6 pt-4">
                    <div class="p-5 rounded-2xl bg-slate-50 dark:bg-slate-900/50 border border-slate-100 dark:border-slate-700 flex items-center gap-4">
                        <div class="w-10 h-10 bg-white dark:bg-slate-800 rounded-xl flex items-center justify-center text-slate-400 dark:text-slate-500 shadow-sm">
                            <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect width="18" height="18" x="3" y="4" rx="2" ry="2"/><line x1="16" x2="16" y1="2" y2="6"/><line x1="8" x2="8" y1="2" y2="6"/><line x1="3" x2="21" y1="10" y2="10"/></svg>
                        </div>
                        <div>
                            <p class="text-[10px] font-bold text-slate-400 dark:text-slate-500 uppercase tracking-widest mb-0.5">Reportado el</p>
                            <p class="text-sm font-bold text-slate-900 dark:text-white">{formatDate(reporte.fechaCreacion)}</p>
                        </div>
                    </div>
                    
                    <div class="p-5 rounded-2xl bg-slate-50 dark:bg-slate-900/50 border border-slate-100 dark:border-slate-700 flex items-center gap-4">
                        <div class="w-10 h-10 bg-white dark:bg-slate-800 rounded-xl flex items-center justify-center text-slate-400 dark:text-slate-500 shadow-sm">
                            <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z"/><circle cx="12" cy="10" r="3"/></svg>
                        </div>
                        <div>
                            <p class="text-[10px] font-bold text-slate-400 dark:text-slate-500 uppercase tracking-widest mb-0.5">Ubicación</p>
                            <p class="text-sm font-bold text-slate-900 dark:text-white truncate max-w-[200px]" title={reporte.direccionTexto}>{reporte.direccionTexto}</p>
                        </div>
                    </div>
                </div>
            </section>

            <!-- Galería de Fotos -->
            {#if reporte.fotos && reporte.fotos.length > 0}
                <section class="space-y-6">
                    <h2 class="text-2xl font-bold text-slate-900 dark:text-white font-jost flex items-center gap-3 ml-4">
                        <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 text-sabana-violet dark:text-sabana-lila" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><rect width="18" height="18" x="3" y="3" rx="2" ry="2"/><circle cx="9" cy="9" r="2"/><path d="m21 15-3.086-3.086a2 2 0 0 0-2.828 0L6 21"/></svg>
                        Evidencia Fotográfica
                    </h2>
                    <div class="grid grid-cols-2 sm:grid-cols-3 gap-4">
                        {#each reporte.fotos as foto}
                            <div class="aspect-square rounded-3xl overflow-hidden border-4 border-white dark:border-slate-700 shadow-sm hover:shadow-xl transition-all hover:scale-[1.02] cursor-zoom-in">
                                <img src={foto.url} alt="Evidencia" class="w-full h-full object-cover" />
                            </div>
                        {/each}
                    </div>
                </section>
            {/if}

            <!-- Historial -->
            <section class="bg-white dark:bg-slate-800 p-10 rounded-[2.5rem] shadow-sm border border-slate-100 dark:border-slate-700">
                <h2 class="text-2xl font-bold text-slate-900 dark:text-white font-jost flex items-center gap-3 mb-10">
                    <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 text-sabana-violet dark:text-sabana-lila" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><path d="M12 8v4l3 3"/><circle cx="12" cy="12" r="10"/></svg>
                    Historial de Gestión
                </h2>
                
                {#if reporte.historial && reporte.historial.length > 0}
                    <div class="space-y-10 relative before:content-[''] before:absolute before:left-[11px] before:top-2 before:bottom-2 before:w-[2px] before:bg-slate-100 dark:before:bg-slate-700">
                        {#each reporte.historial as h}
                            {@const hStatus = getStatusConfig(h.nombreEstadoNuevo)}
                            <div class="relative pl-10 group">
                                <div class="absolute left-0 top-1.5 w-6 h-6 rounded-full bg-white dark:bg-slate-800 border-4 border-slate-100 dark:border-slate-700 z-10 group-hover:border-sabana-violet dark:group-hover:border-sabana-lila transition-colors"></div>
                                <div class="space-y-3 p-6 rounded-3xl bg-slate-50 dark:bg-slate-900/50 border border-slate-100/50 dark:border-slate-700/50 group-hover:bg-white dark:group-hover:bg-slate-800 group-hover:shadow-md transition-all">
                                    <div class="flex flex-wrap items-center justify-between gap-4">
                                        <span class="inline-flex items-center gap-2 px-3 py-1 rounded-full text-[10px] font-bold uppercase tracking-widest border {hStatus.class} dark:bg-opacity-10">
                                            <span class="w-1.5 h-1.5 rounded-full {hStatus.dot}"></span>
                                            {h.nombreEstadoNuevo}
                                        </span>
                                        <span class="text-[10px] font-bold text-slate-400 dark:text-slate-500 uppercase tracking-widest">{formatDate(h.fechaCambio)}</span>
                                    </div>
                                    {#if h.comentario}
                                        <p class="text-sm text-slate-600 dark:text-slate-400 font-onest leading-relaxed">
                                            {h.comentario}
                                        </p>
                                    {/if}
                                </div>
                            </div>
                        {/each}
                    </div>
                {:else}
                    <div class="text-center py-10 bg-slate-50 dark:bg-slate-900/50 rounded-2xl border border-dashed border-slate-200 dark:border-slate-700">
                        <p class="text-slate-400 dark:text-slate-500 font-onest italic">El reporte está a la espera de ser procesado.</p>
                    </div>
                {/if}
            </section>
        </div>

        <!-- Sidebar - Mapa -->
        <aside class="space-y-6">
            <div class="bg-white dark:bg-slate-800 p-2 rounded-[2.5rem] shadow-lg border border-slate-100 dark:border-slate-700 overflow-hidden">
                <div class="h-[400px] rounded-[2rem] overflow-hidden relative group">
                    <MapStatic latitud={reporte.latitud} longitud={reporte.longitud} />
                    <div class="absolute bottom-6 left-6 right-6 p-4 bg-white/80 dark:bg-slate-900/80 backdrop-blur-md rounded-2xl border border-white dark:border-slate-700 shadow-lg z-[1000]">
                        <div class="flex items-center justify-between text-[10px] font-bold font-mono text-slate-500 dark:text-slate-400 uppercase tracking-widest">
                            <span>Lat: {reporte.latitud.toFixed(6)}</span>
                            <span>Lng: {reporte.longitud.toFixed(6)}</span>
                        </div>
                    </div>
                </div>
                <div class="p-6 text-center">
                    <p class="text-sm font-bold text-slate-900 dark:text-white font-jost mb-1">Ubicación Precisa</p>
                    <p class="text-xs text-slate-500 dark:text-slate-400 font-onest">{reporte.direccionTexto}</p>
                </div>
            </div>
            
            <div class="bg-gradient-to-br from-sabana-violet to-sabana-purple p-8 rounded-[2.5rem] text-white shadow-xl shadow-violet-200 dark:shadow-none">
                <h3 class="text-xl font-bold font-jost mb-4">¿Necesitas ayuda?</h3>
                <p class="text-sm opacity-80 font-onest mb-6 leading-relaxed">
                    Si tienes dudas sobre este reporte o necesitas atención inmediata, contacta con soporte técnico.
                </p>
                <button class="w-full py-4 bg-white dark:bg-slate-800 text-sabana-violet dark:text-sabana-lila rounded-2xl font-bold text-sm hover:bg-sabana-lila dark:hover:bg-slate-700 transition-all shadow-lg">
                    Contactar Soporte
                </button>
            </div>
        </aside>
    </div>
</div>

<style>
    @reference "../../../app.css";
</style>
