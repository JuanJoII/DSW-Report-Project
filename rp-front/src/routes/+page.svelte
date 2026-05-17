<script lang="ts">
  let { data } = $props();
  let user = $derived(data.user);
  let scrollY = $state(0);
</script>

<svelte:window bind:scrollY />

<!-- Hero Section: Propósito Principal -->
<div class="relative min-h-[90vh] flex items-center justify-center overflow-hidden rounded-[3rem] bg-gradient-to-br from-sabana-purple via-sabana-violet to-violet-600 text-white mb-24 shadow-2xl shadow-violet-200 dark:shadow-none">
  <!-- Capas de Parallax Real -->
  <div 
    class="parallax-bg absolute inset-0 opacity-20 dark:opacity-10 pointer-events-none"
    style="transform: translateY({scrollY * 0.3}px)"
  ></div>
  
  <div 
    class="absolute -top-24 -right-24 w-96 h-96 bg-white/10 rounded-full blur-3xl animate-float"
    style="transform: translateY({scrollY * 0.15}px)"
  ></div>
  
  <div 
    class="absolute bottom-24 left-1/4 w-64 h-64 bg-sabana-orange/10 rounded-full blur-2xl animate-float-delayed"
    style="transform: translateY({scrollY * -0.1}px)"
  ></div>
  
  <div 
    class="absolute top-1/2 -left-24 w-72 h-72 bg-sabana-lila/10 rounded-full blur-3xl animate-float"
    style="transform: translateY({scrollY * 0.2}px)"
  ></div>

  <div class="relative px-8 py-20 text-center max-w-5xl mx-auto z-10">
    <div class="inline-flex items-center gap-2 bg-white/10 backdrop-blur-md px-4 py-2 rounded-full mb-8 border border-white/20">
      <span class="w-2 h-2 bg-sabana-orange rounded-full"></span>
      <span class="text-xs font-bold uppercase tracking-[0.2em] font-onest">Compromiso Ciudadano</span>
    </div>
    
    <h1 class="text-5xl md:text-7xl font-bold mb-8 leading-[1.1] font-jost">
      Transformando <br/> <span class="text-sabana-lila">nuestro entorno</span>
    </h1>
    
    <p class="text-lg md:text-xl mb-12 opacity-90 font-onest max-w-2xl mx-auto leading-relaxed">
      Reporta Sabana es el puente directo entre tu comunidad y la administración pública. Una herramienta diseñada para dar voz a cada ciudadano y agilizar la mejora de nuestro espacio común.
    </p>
    
    <div class="flex flex-col sm:flex-row gap-5 justify-center items-center">
      {#if user}
        <a href="/reportes/nuevo" class="group bg-white text-sabana-purple hover:bg-sabana-lila px-10 py-5 rounded-full text-lg font-bold transition-all shadow-xl hover:scale-105 font-onest flex items-center gap-3">
          Crear Reporte
          <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 group-hover:translate-x-1 transition-transform" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><line x1="12" x2="12" y1="5" y2="19"/><line x1="5" x2="19" y1="12" y2="12"/></svg>
        </a>
        <a href="/reportes/mis-reportes" class="bg-white/10 border-2 border-white/20 hover:bg-white/20 px-10 py-5 rounded-full text-lg font-bold transition-all backdrop-blur-sm font-onest">
          Ver Mis Reportes
        </a>
      {:else}
        <a href="/register" class="group bg-white text-sabana-purple hover:bg-sabana-lila px-10 py-5 rounded-full text-lg font-bold transition-all shadow-xl hover:scale-105 font-onest flex items-center gap-3">
          Empezar Ahora
          <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 group-hover:translate-x-1 transition-transform" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><line x1="12" x2="12" y1="5" y2="19"/><line x1="5" x2="19" y1="12" y2="12"/></svg>
        </a>
        <a href="/login" class="bg-white/10 border-2 border-white/20 hover:bg-white/20 px-10 py-5 rounded-full text-lg font-bold transition-all backdrop-blur-sm font-onest">
          Iniciar Sesión
        </a>
      {/if}
    </div>
  </div>
</div>

<!-- Sección de Propósito y Problemática -->
<div class="grid lg:grid-cols-2 gap-16 items-center mb-32">
  <div class="space-y-8">
    <div class="space-y-4">
      <h2 class="text-4xl font-bold text-slate-900 dark:text-white font-jost">¿Por qué es importante reportar?</h2>
      <p class="text-slate-600 dark:text-slate-400 font-onest text-lg leading-relaxed">
        Las ciudades crecen y cambian constantemente. A menudo, pequeñas fallas en la infraestructura pueden convertirse en grandes riesgos si no se atienden a tiempo.
      </p>
    </div>

    <div class="space-y-6">
      <div class="flex gap-4 p-6 bg-white dark:bg-slate-800 rounded-3xl border border-slate-100 dark:border-slate-700 shadow-sm">
        <div class="w-12 h-12 bg-amber-50 dark:bg-amber-900/20 text-amber-600 dark:text-amber-400 rounded-2xl flex items-center justify-center shrink-0">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"/><line x1="12" y1="9" x2="12" y2="13"/><line x1="12" y1="17" x2="12.01" y2="17"/></svg>
        </div>
        <div>
          <h4 class="font-bold text-slate-900 dark:text-white mb-1">Detección Temprana</h4>
          <p class="text-sm text-slate-500 dark:text-slate-400">Identificamos baches, fugas o fallas eléctricas antes de que causen accidentes o daños mayores.</p>
        </div>
      </div>

      <div class="flex gap-4 p-6 bg-white dark:bg-slate-800 rounded-3xl border border-slate-100 dark:border-slate-700 shadow-sm">
        <div class="w-12 h-12 bg-violet-50 dark:bg-violet-900/30 text-sabana-violet dark:text-sabana-lila rounded-2xl flex items-center justify-center shrink-0">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="22 12 18 12 15 21 9 3 6 12 2 12"/></svg>
        </div>
        <div>
          <h4 class="font-bold text-slate-900 dark:text-white mb-1">Eficiencia Operativa</h4>
          <p class="text-sm text-slate-500 dark:text-slate-400">Tus reportes geolocalizados permiten que las cuadrillas de mantenimiento lleguen directamente al problema.</p>
        </div>
      </div>

      <div class="flex gap-4 p-6 bg-white dark:bg-slate-800 rounded-3xl border border-slate-100 dark:border-slate-700 shadow-sm">
        <div class="w-12 h-12 bg-emerald-50 dark:bg-emerald-900/20 text-emerald-600 dark:text-emerald-400 rounded-2xl flex items-center justify-center shrink-0">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
        </div>
        <div>
          <h4 class="font-bold text-slate-900 dark:text-white mb-1">Transparencia Total</h4>
          <p class="text-sm text-slate-500 dark:text-slate-400">Sabrás exactamente en qué etapa se encuentra tu solicitud y recibirás retroalimentación directa.</p>
        </div>
      </div>
    </div>
  </div>

  <div class="relative bg-slate-100 dark:bg-slate-900 rounded-[3rem] p-12 overflow-hidden group">
    <div class="absolute inset-0 bg-gradient-to-br from-sabana-violet/5 to-transparent"></div>
    <div class="relative z-10 space-y-6">
      <div class="w-20 h-20 bg-white dark:bg-slate-800 rounded-3xl shadow-lg flex items-center justify-center mb-8">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-10 h-10 text-sabana-violet" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z"/><circle cx="12" cy="10" r="3"/></svg>
      </div>
      <h3 class="text-3xl font-bold text-slate-900 dark:text-white font-jost">Nuestra Misión</h3>
      <p class="text-slate-600 dark:text-slate-400 font-onest text-lg leading-relaxed">
        Buscamos democratizar el mantenimiento urbano. Creemos que una ciudad inteligente no es la que tiene más sensores, sino la que mejor escucha a sus habitantes.
      </p>
      <div class="pt-8 border-t border-slate-200 dark:border-slate-800">
        <p class="text-sabana-violet dark:text-sabana-lila font-bold italic">
          "Pequeñas acciones ciudadanas generan grandes cambios colectivos."
        </p>
      </div>
    </div>
  </div>
</div>

<!-- CTA Final -->
<div class="bg-slate-900 dark:bg-slate-950 p-12 md:p-20 rounded-[3rem] text-center space-y-8 shadow-2xl">
  <h2 class="text-3xl md:text-5xl font-bold text-white font-jost">¿Viste algo que necesita atención?</h2>
  <p class="text-slate-400 text-lg max-w-2xl mx-auto font-onest">
    No esperes a que alguien más lo reporte. Tu contribución es vital para mantener la seguridad y calidad de vida en nuestra Sabana.
  </p>
  <div class="flex justify-center">
    <a href="/reportes/nuevo" class="px-12 py-5 bg-sabana-violet hover:bg-sabana-purple text-white rounded-full font-bold text-xl transition-all hover:scale-105 shadow-xl shadow-sabana-violet/20">
      Hacer un Reporte
    </a>
  </div>
</div>

<style>
  @reference "../app.css";

  .parallax-bg {
    background-image: radial-gradient(circle at 2px 2px, rgba(255,255,255,0.15) 1px, transparent 0);
    background-size: 40px 40px;
  }

  @keyframes float {
    0%, 100% { transform: translateY(0) scale(1); }
    50% { transform: translateY(-20px) scale(1.05); }
  }

  @keyframes float-delayed {
    0%, 100% { transform: translateY(0) rotate(0deg); }
    50% { transform: translateY(25px) rotate(5deg); }
  }

  .animate-float {
    animation: float 8s ease-in-out infinite;
  }

  .animate-float-delayed {
    animation: float-delayed 10s ease-in-out infinite;
    animation-delay: 2s;
  }
</style>
