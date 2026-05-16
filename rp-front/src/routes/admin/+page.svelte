<script>
  import { fade } from 'svelte/transition';
  let { data } = $props();
  let reportes = $state(data.reportes || []);
  let error = $state(data.error || null);
  let estadosDeLaDB = $state(data.estados || []);

  // Estados para filtros
  let filtroEstado = $state('todos');
  let filtroCategoria = $state('todos');
  let busqueda = $state('');
  let sortBy = $state('fecha');
  let sortOrder = $state('desc');

  // Modal para cambiar estado
  let mostrarModal = $state(false);
  let reporteSeleccionado = $state(null);
  let nuevoEstado = $state(null);
  let estadosDisponibles = $state([]);

  // Derivadas para datos filtrados
  let reportesFiltrados = $derived.by(() => {
    let resultado = [...reportes];

    if (filtroEstado !== 'todos') {
      resultado = resultado.filter(r => r.estadoId === parseInt(filtroEstado));
    }

    if (filtroCategoria !== 'todos') {
      resultado = resultado.filter(r => r.categoriaId === parseInt(filtroCategoria));
    }

    if (busqueda.trim()) {
      const termino = busqueda.toLowerCase();
      resultado = resultado.filter(
        r =>
          r.descripcion.toLowerCase().includes(termino) ||
          r.direccionTexto.toLowerCase().includes(termino)
      );
    }

    resultado.sort((a, b) => {
      let aVal, bVal;

      if (sortBy === 'fecha') {
        aVal = new Date(a.fechaCreacion);
        bVal = new Date(b.fechaCreacion);
      } else if (sortBy === 'estado') {
        aVal = a.nombreEstado;
        bVal = b.nombreEstado;
      } else if (sortBy === 'categoria') {
        aVal = a.nombreCategoria;
        bVal = b.nombreCategoria;
      }

      if (sortOrder === 'asc') {
        return aVal < bVal ? -1 : aVal > bVal ? 1 : 0;
      } else {
        return aVal > bVal ? -1 : aVal < bVal ? 1 : 0;
      }
    });

    return resultado;
  });

  // Usar estados de la base de datos
  let estados = $derived.by(() => {
    if (estadosDeLaDB && estadosDeLaDB.length > 0) {
      return estadosDeLaDB.map(e => ({
        id: e.id,
        nombre: e.nombre
      })).sort((a, b) => a.nombre.localeCompare(b.nombre));
    }
    const unique = [...new Set(reportes.map(r => r.estadoId))];
    return unique
      .map(id => ({
        id,
        nombre: reportes.find(r => r.estadoId === id)?.nombreEstado || 'Desconocido'
      }))
      .sort((a, b) => a.nombre.localeCompare(b.nombre));
  });

  let categorias = $derived.by(() => {
    const unique = [...new Set(reportes.map(r => r.categoriaId))];
    return unique
      .map(id => ({
        id,
        nombre: reportes.find(r => r.categoriaId === id)?.nombreCategoria || 'Desconocido'
      }))
      .sort((a, b) => a.nombre.localeCompare(b.nombre));
  });

  function formatFecha(fecha) {
    return new Date(fecha).toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  let stats = $derived.by(() => {
    return {
      total: reportes.length,
      filtrados: reportesFiltrados.length,
      porEstado: estados.map(e => ({
        id: e.id,
        nombre: e.nombre,
        cantidad: reportes.filter(r => r.estadoId === e.id).length
      }))
    };
  });

  function toggleSort(columna) {
    if (sortBy === columna) {
      sortOrder = sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      sortBy = columna;
      sortOrder = 'asc';
    }
  }

  function abrirModalCambiarEstado(reporte) {
    reporteSeleccionado = reporte;
    nuevoEstado = reporte.estadoId;
    estadosDisponibles = estadosDeLaDB && estadosDeLaDB.length > 0
      ? estadosDeLaDB.map(e => ({ id: e.id, nombre: e.nombre }))
      : estados;
    mostrarModal = true;
  }

  function cerrarModal() {
    mostrarModal = false;
    reporteSeleccionado = null;
    nuevoEstado = null;
  }

  async function guardarCambioEstado() {
    if (!reporteSeleccionado || !nuevoEstado || nuevoEstado === reporteSeleccionado.estadoId) {
      cerrarModal();
      return;
    }

    try {
      const formData = new FormData();
      formData.append('reporteId', reporteSeleccionado.id);
      formData.append('estadoId', nuevoEstado);

      const response = await fetch('?/cambiarEstado', {
        method: 'POST',
        body: formData
      });

      const result = await response.json();

      if (result.success) {
        const reporteIndex = reportes.findIndex(r => r.id === reporteSeleccionado.id);
        if (reporteIndex !== -1) {
          reportes[reporteIndex].estadoId = parseInt(nuevoEstado);
          reportes[reporteIndex].nombreEstado = estados.find(e => e.id === parseInt(nuevoEstado))?.nombre || 'Desconocido';
          reportes = [...reportes];
        }
        cerrarModal();
      } else {
        alert('Error al actualizar el estado: ' + (result.error || 'Error desconocido'));
      }
    } catch (err) {
      console.error('Error:', err);
      alert('Error de conexión al actualizar el estado');
    }
  }

  function getStatusStyle(status) {
    const s = status?.toLowerCase() || '';
    if (s.includes('pendiente')) return 'bg-cyan-50 text-cyan-600 border-cyan-100 dark:bg-cyan-900/20 dark:text-cyan-400 dark:border-cyan-800';
    if (s.includes('revisión') || s.includes('proceso')) return 'bg-amber-50 text-amber-600 border-amber-100 dark:bg-amber-900/20 dark:text-amber-400 dark:border-amber-800';
    if (s.includes('resuelto')) return 'bg-emerald-50 text-emerald-600 border-emerald-100 dark:bg-emerald-900/20 dark:text-emerald-400 dark:border-emerald-800';
    if (s.includes('rechazado') || s.includes('error')) return 'bg-red-50 text-red-600 border-red-100 dark:bg-red-900/20 dark:text-red-400 dark:border-red-800';
    return 'bg-slate-50 text-slate-600 border-slate-100 dark:bg-slate-900/20 dark:text-slate-400 dark:border-slate-800';
  }
</script>

<div class="space-y-8 animate-in fade-in duration-500">
  <!-- Estadísticas -->
  <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
    <div class="bg-white dark:bg-slate-800 p-6 rounded-3xl shadow-sm border border-slate-100 dark:border-slate-700 flex items-center gap-5">
      <div class="w-14 h-14 bg-slate-100 dark:bg-slate-700 rounded-2xl flex items-center justify-center text-slate-500 dark:text-slate-400">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-7 h-7" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14.5 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V7.5L14.5 2z"/><polyline points="14 2 14 8 20 8"/></svg>
      </div>
      <div>
        <p class="text-xs font-bold text-slate-400 dark:text-slate-500 uppercase tracking-widest mb-1">Total Reportes</p>
        <p class="text-3xl font-bold text-slate-900 dark:text-white font-jost">{stats.total}</p>
      </div>
    </div>
    
    {#each stats.porEstado as stat}
      <div class="bg-white dark:bg-slate-800 p-6 rounded-3xl shadow-sm border border-slate-100 dark:border-slate-700 flex items-center gap-5">
        <div class="w-14 h-14 rounded-2xl flex items-center justify-center {getStatusStyle(stat.nombre)} dark:bg-opacity-10">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-7 h-7" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"/><polyline points="12 6 12 12 16 14"/></svg>
        </div>
        <div>
          <p class="text-xs font-bold text-slate-400 dark:text-slate-500 uppercase tracking-widest mb-1">{stat.nombre}</p>
          <p class="text-3xl font-bold text-slate-900 dark:text-white font-jost">{stat.cantidad}</p>
        </div>
      </div>
    {/each}
  </div>

  <!-- Controles de filtro -->
  <div class="bg-white dark:bg-slate-800 p-8 rounded-3xl shadow-sm border border-slate-100 dark:border-slate-700">
    <div class="flex flex-wrap items-end gap-6">
      <div class="flex-1 min-w-[300px] space-y-2">
        <label class="text-xs font-bold text-slate-500 dark:text-slate-400 uppercase tracking-widest ml-1" for="search">Búsqueda rápida</label>
        <div class="relative group">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 dark:text-slate-500 group-focus-within:text-sabana-violet dark:group-focus-within:text-sabana-lila transition-colors" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="11" cy="11" r="8"/><path d="m21 21-4.3-4.3"/></svg>
          <input
            id="search"
            type="text"
            placeholder="Buscar por descripción o dirección..."
            class="w-full pl-12 pr-4 py-3.5 bg-slate-50 dark:bg-slate-900/50 border-transparent focus:bg-white dark:focus:bg-slate-900 focus:border-sabana-violet/30 dark:focus:border-sabana-violet/50 rounded-2xl text-slate-900 dark:text-white placeholder:text-slate-400 dark:placeholder:text-slate-600 focus:ring-4 focus:ring-sabana-violet/5 transition-all outline-none border"
            bind:value={busqueda}
          />
        </div>
      </div>

      <div class="w-48 space-y-2">
        <label class="text-xs font-bold text-slate-500 dark:text-slate-400 uppercase tracking-widest ml-1" for="estado">Estado</label>
        <select 
          id="estado" 
          bind:value={filtroEstado}
          class="w-full px-4 py-3.5 bg-slate-50 dark:bg-slate-900/50 border-transparent focus:bg-white dark:focus:bg-slate-900 focus:border-sabana-violet/30 dark:focus:border-sabana-violet/50 rounded-2xl text-slate-900 dark:text-white focus:ring-4 focus:ring-sabana-violet/5 transition-all outline-none border cursor-pointer"
        >
          <option value="todos">Todos los estados</option>
          {#each estados as estado}
            <option value={estado.id}>{estado.nombre}</option>
          {/each}
        </select>
      </div>

      <div class="w-48 space-y-2">
        <label class="text-xs font-bold text-slate-500 dark:text-slate-400 uppercase tracking-widest ml-1" for="categoria">Categoría</label>
        <select 
          id="categoria" 
          bind:value={filtroCategoria}
          class="w-full px-4 py-3.5 bg-slate-50 dark:bg-slate-900/50 border-transparent focus:bg-white dark:focus:bg-slate-900 focus:border-sabana-violet/30 dark:focus:border-sabana-violet/50 rounded-2xl text-slate-900 dark:text-white focus:ring-4 focus:ring-sabana-violet/5 transition-all outline-none border cursor-pointer"
        >
          <option value="todos">Todas las categorías</option>
          {#each categorias as categoria}
            <option value={categoria.id}>{categoria.nombre}</option>
          {/each}
        </select>
      </div>
    </div>
  </div>

  <!-- Tabla de reportes -->
  <div class="bg-white dark:bg-slate-800 rounded-[2.5rem] shadow-sm border border-slate-100 dark:border-slate-700 overflow-hidden">
    <div class="px-8 py-6 border-b border-slate-50 dark:border-slate-700 flex justify-between items-center bg-slate-50/50 dark:bg-slate-900/30">
      <p class="text-sm font-medium text-slate-500 dark:text-slate-400 font-onest">
        Mostrando <span class="text-slate-900 dark:text-white font-bold">{reportesFiltrados.length}</span> reportes
      </p>
    </div>

    {#if error}
      <div class="p-12 text-center">
        <div class="w-16 h-16 bg-red-50 dark:bg-red-900/20 text-red-500 rounded-full flex items-center justify-center mx-auto mb-4">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-8 h-8" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"/><line x1="12" x2="12" y1="8" y2="12"/><line x1="12" x2="12.01" y1="16" y2="16"/></svg>
        </div>
        <p class="text-red-600 dark:text-red-400 font-bold">{error}</p>
      </div>
    {:else if reportes.length === 0}
      <div class="p-20 text-center">
        <div class="w-20 h-20 bg-slate-50 dark:bg-slate-900/50 text-slate-300 dark:text-slate-700 rounded-full flex items-center justify-center mx-auto mb-6">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-10 h-10" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M14.5 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V7.5L14.5 2z"/><polyline points="14 2 14 8 20 8"/></svg>
        </div>
        <p class="text-slate-400 dark:text-slate-500 text-lg">No hay reportes en el sistema aún.</p>
      </div>
    {:else}
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="text-[10px] font-bold uppercase tracking-[0.2em] text-slate-400 dark:text-slate-500 bg-slate-50/30 dark:bg-slate-900/50">
              <th class="px-8 py-5 cursor-pointer hover:text-sabana-violet dark:hover:text-sabana-lila transition-colors" onclick={() => toggleSort('fecha')}>
                Fecha {sortBy === 'fecha' ? (sortOrder === 'asc' ? '↑' : '↓') : ''}
              </th>
              <th class="px-8 py-5">Descripción / Dirección</th>
              <th class="px-8 py-5 cursor-pointer hover:text-sabana-violet dark:hover:text-sabana-lila transition-colors" onclick={() => toggleSort('categoria')}>
                Categoría {sortBy === 'categoria' ? (sortOrder === 'asc' ? '↑' : '↓') : ''}
              </th>
              <th class="px-8 py-5 cursor-pointer hover:text-sabana-violet dark:hover:text-sabana-lila transition-colors" onclick={() => toggleSort('estado')}>
                Estado {sortBy === 'estado' ? (sortOrder === 'asc' ? '↑' : '↓') : ''}
              </th>
              <th class="px-8 py-5 text-center">Fotos</th>
              <th class="px-8 py-5 text-right">Acciones</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-50 dark:divide-slate-700">
            {#each reportesFiltrados as reporte}
              <tr 
                class="hover:bg-slate-50/80 dark:hover:bg-slate-700/50 transition-all cursor-pointer group" 
                onclick={() => window.location.href = `/reportes/${reporte.id}?from=admin`}
              >
                <td class="px-8 py-6">
                  <p class="text-sm font-bold text-slate-900 dark:text-white">{formatFecha(reporte.fechaCreacion)}</p>
                  <p class="text-[10px] text-slate-400 dark:text-slate-500 uppercase tracking-widest mt-0.5">ID: {reporte.id}</p>
                </td>
                <td class="px-8 py-6 max-w-md">
                  <p class="text-sm font-bold text-slate-900 dark:text-white truncate group-hover:text-sabana-violet dark:group-hover:text-sabana-lila transition-colors">{reporte.descripcion}</p>
                  <div class="flex items-center gap-1.5 mt-1 text-slate-400 dark:text-slate-500">
                    <svg xmlns="http://www.w3.org/2000/svg" class="w-3.5 h-3.5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z"/><circle cx="12" cy="10" r="3"/></svg>
                    <span class="text-xs truncate">{reporte.direccionTexto}</span>
                  </div>
                </td>
                <td class="px-8 py-6">
                  <span class="text-[10px] font-bold uppercase tracking-widest px-3 py-1 bg-violet-50 dark:bg-violet-900/30 text-sabana-violet dark:text-sabana-lila rounded-lg border border-violet-100 dark:border-violet-800">
                    {reporte.nombreCategoria}
                  </span>
                </td>
                <td class="px-8 py-6">
                  <span class="inline-flex items-center gap-1.5 px-4 py-1.5 rounded-full text-[10px] font-bold uppercase tracking-widest border {getStatusStyle(reporte.nombreEstado)} dark:bg-opacity-10">
                    <span class="w-1.5 h-1.5 rounded-full bg-current"></span>
                    {reporte.nombreEstado}
                  </span>
                </td>
                <td class="px-8 py-6 text-center">
                  <div class="inline-flex items-center justify-center w-8 h-8 rounded-full bg-slate-100 dark:bg-slate-700 text-slate-600 dark:text-slate-300 text-xs font-bold">
                    {reporte.totalFotos}
                  </div>
                </td>
                <td class="px-8 py-6 text-right">
                  <button 
                    class="p-2 text-slate-400 dark:text-slate-500 hover:text-sabana-violet dark:hover:text-sabana-lila hover:bg-violet-50 dark:hover:bg-violet-900/30 rounded-xl transition-all"
                    onclick={(e) => { e.stopPropagation(); abrirModalCambiarEstado(reporte); }}
                    title="Cambiar estado"
                  >
                    <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 22h6-12C4.4 22 3 20.6 3 19V7c0-1.6 1.4-3 3-3h5l2 3h7c1.6 0 3 1.4 3 3v4"/><path d="m17 19 2 2 4-4"/></svg>
                  </button>
                </td>
              </tr>
            {/each}
          </tbody>
        </table>
      </div>
    {/if}
  </div>
</div>

<!-- Modal para cambiar estado -->
{#if mostrarModal && reporteSeleccionado}
  <div class="fixed inset-0 z-[100] flex items-center justify-center px-4" transition:fade={{ duration: 200 }}>
    <div class="absolute inset-0 bg-slate-900/40 dark:bg-slate-950/60 backdrop-blur-sm" onclick={cerrarModal}></div>
    
    <div class="relative bg-white dark:bg-slate-800 w-full max-w-md rounded-[2.5rem] shadow-2xl p-10 overflow-hidden animate-in zoom-in-95 duration-200">
      <!-- Header -->
      <div class="flex items-center gap-4 mb-8">
        <div class="w-12 h-12 bg-violet-50 dark:bg-violet-900/30 text-sabana-violet dark:text-sabana-lila rounded-2xl flex items-center justify-center">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><path d="M12 22h6-12C4.4 22 3 20.6 3 19V7c0-1.6 1.4-3 3-3h5l2 3h7c1.6 0 3 1.4 3 3v4"/><path d="m17 19 2 2 4-4"/></svg>
        </div>
        <div>
          <h2 class="text-2xl font-bold text-slate-900 dark:text-white font-jost">Actualizar Estado</h2>
          <p class="text-sm text-slate-500 dark:text-slate-400 font-onest">Reporte #{reporteSeleccionado.id}</p>
        </div>
      </div>

      <p class="text-slate-600 dark:text-slate-300 mb-8 font-onest text-sm line-clamp-2 leading-relaxed italic">
        "{reporteSeleccionado.descripcion}"
      </p>

      <div class="space-y-6">
        <div class="space-y-2">
          <label class="text-xs font-bold text-slate-400 dark:text-slate-500 uppercase tracking-widest ml-1" for="nuevo-estado">Selecciona la nueva fase</label>
          <div class="grid grid-cols-1 gap-3">
            {#each estadosDisponibles as estado}
              <button 
                class="flex items-center justify-between px-6 py-4 rounded-2xl border-2 transition-all font-bold text-sm {nuevoEstado === estado.id ? 'border-sabana-violet bg-violet-50 text-sabana-violet dark:bg-violet-900/30 dark:text-sabana-lila' : 'border-slate-50 dark:border-slate-700 hover:border-slate-200 dark:hover:border-slate-600'}"
                onclick={() => nuevoEstado = estado.id}
              >
                {estado.nombre}
                {#if nuevoEstado === estado.id}
                  <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><polyline points="20 6 9 17 4 12"/></svg>
                {/if}
              </button>
            {/each}
          </div>
        </div>

        <div class="flex gap-4 pt-4">
          <button 
            class="flex-1 px-6 py-4 rounded-2xl bg-slate-50 dark:bg-slate-700 text-slate-600 dark:text-slate-300 font-bold hover:bg-slate-100 dark:hover:bg-slate-600 transition-colors" 
            onclick={cerrarModal}
          >
            Cancelar
          </button>
          <button 
            class="flex-1 px-6 py-4 rounded-2xl bg-sabana-violet text-white font-bold shadow-lg shadow-violet-200 dark:shadow-none hover:bg-sabana-purple transition-all"
            onclick={guardarCambioEstado}
          >
            Guardar Cambios
          </button>
        </div>
      </div>
    </div>
  </div>
{/if}

<style>
  @reference "../../app.css";
  
  :global(.animate-in) {
    animation-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
  }
</style>
