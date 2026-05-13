<script>
  let { data } = $props();
  let reportes = $state(data.reportes || []);
  let error = $state(data.error || null);
  let backendUrl = $state(data.backendUrl || 'http://backend:8080');
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
    // Si tenemos estados de la BD, úsalos
    if (estadosDeLaDB && estadosDeLaDB.length > 0) {
      return estadosDeLaDB.map(e => ({
        id: e.id,
        nombre: e.nombre
      })).sort((a, b) => a.nombre.localeCompare(b.nombre));
    }
    // Fallback: extraer de reportes si no hay BD
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

  // Formatear fecha
  function formatFecha(fecha) {
    return new Date(fecha).toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  // Estadísticas
  let stats = $derived.by(() => {
    return {
      total: reportes.length,
      filtrados: reportesFiltrados.length,
      porEstado: estados.map(e => ({
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
    // Usar estados de la BD si existen, sino usar los derivados
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
        // Actualizar el reporte localmente
        const reporteIndex = reportes.findIndex(r => r.id === reporteSeleccionado.id);
        if (reporteIndex !== -1) {
          reportes[reporteIndex].estadoId = parseInt(nuevoEstado);
          reportes[reporteIndex].nombreEstado = estados.find(e => e.id === parseInt(nuevoEstado))?.nombre || 'Desconocido';
          reportes = [...reportes]; // Trigger reactivity
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
</script>

<div class="dashboard">
  <!-- Estadísticas -->
  <div class="stats-grid">
    <div class="stat-card">
      <div class="stat-number">{stats.total}</div>
      <div class="stat-label">Reportes Totales</div>
    </div>
    {#each stats.porEstado as stat (stat.nombre)}
      <div class="stat-card stat-secondary">
        <div class="stat-number">{stat.cantidad}</div>
        <div class="stat-label">{stat.nombre}</div>
      </div>
    {/each}
  </div>

  <!-- Controles de filtro -->
  <div class="filter-section">
    <div class="filter-group">
      <label for="filtro-estado">Estado:</label>
      <select id="filtro-estado" bind:value={filtroEstado}>
        <option value="todos">Todos</option>
        {#each estados as estado (estado.id)}
          <option value={estado.id}>{estado.nombre}</option>
        {/each}
      </select>
    </div>

    <div class="filter-group">
      <label for="filtro-categoria">Categoría:</label>
      <select id="filtro-categoria" bind:value={filtroCategoria}>
        <option value="todos">Todas</option>
        {#each categorias as categoria (categoria.id)}
          <option value={categoria.id}>{categoria.nombre}</option>
        {/each}
      </select>
    </div>

    <div class="filter-group search">
      <input
        type="text"
        placeholder="Buscar en descripción o dirección..."
        bind:value={busqueda}
      />
    </div>

    <div class="filter-result">
      Mostrando <strong>{reportesFiltrados.length}</strong> de <strong>{reportes.length}</strong>
    </div>
  </div>

  <!-- Tabla de reportes -->
  {#if error}
    <div class="error-box">
      <strong>Error:</strong> {error}
    </div>
  {:else if reportes.length === 0}
    <div class="empty-state">
      <p>No hay reportes en el sistema aún.</p>
    </div>
  {:else}
    <div class="table-container">
      <table class="reports-table">
        <thead>
          <tr>
            <th onclick={() => toggleSort('fecha')} class:active={sortBy === 'fecha'}>
              Fecha {sortBy === 'fecha' ? (sortOrder === 'asc' ? '↑' : '↓') : ''}
            </th>
            <th>Descripción</th>
            <th>Dirección</th>
            <th onclick={() => toggleSort('categoria')} class:active={sortBy === 'categoria'}>
              Categoría {sortBy === 'categoria' ? (sortOrder === 'asc' ? '↑' : '↓') : ''}
            </th>
            <th onclick={() => toggleSort('estado')} class:active={sortBy === 'estado'}>
              Estado {sortBy === 'estado' ? (sortOrder === 'asc' ? '↑' : '↓') : ''}
            </th>
            <th>Fotos</th>
            <th>Usuario</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {#each reportesFiltrados as reporte (reporte.id)}
            <tr class="clickable" onclick={() => window.location.href = `/reportes/${reporte.id}?from=admin`}>
              <td class="fecha">{formatFecha(reporte.fechaCreacion)}</td>
              <td class="descripcion" title={reporte.descripcion}>
                {reporte.descripcion.substring(0, 50)}...
              </td>
              <td class="direccion" title={reporte.direccionTexto}>
                {reporte.direccionTexto.substring(0, 35)}...
              </td>
              <td><span class="badge badge-categoria">{reporte.nombreCategoria}</span></td>
              <td>
                <span class="badge" class:badge-pendiente={reporte.nombreEstado === 'Pendiente'} class:badge-proceso={reporte.nombreEstado === 'En Proceso'} class:badge-resuelto={reporte.nombreEstado === 'Resuelto'} class:badge-rechazado={reporte.nombreEstado === 'Rechazado'}>
                  {reporte.nombreEstado}
                </span>
              </td>
              <td class="center">{reporte.totalFotos}</td>
              <td class="usuario" title={reporte.usuarioId}>
                {reporte.usuarioId.substring(0, 8)}...
              </td>
              <td class="acciones">
                <button class="btn-cambiar-estado" onclick={(e) => { e.stopPropagation(); abrirModalCambiarEstado(reporte); }}>
                  Cambiar
                </button>
              </td>
            </tr>
          {/each}
        </tbody>
      </table>
    </div>
  {/if}
</div>

<!-- Modal para cambiar estado -->
{#if mostrarModal && reporteSeleccionado}
  <div class="modal-overlay" onclick={cerrarModal}>
    <div class="modal-content" onclick={(e) => e.stopPropagation()}>
      <h2>Cambiar Estado del Reporte</h2>
      <p class="modal-desc">{reporteSeleccionado.descripcion}</p>

      <div class="form-group">
        <label for="estado-select">Nuevo Estado:</label>
        <select id="estado-select" bind:value={nuevoEstado}>
          {#each estadosDisponibles as estado (estado.id)}
            <option value={estado.id}>{estado.nombre}</option>
          {/each}
        </select>
      </div>

      <div class="modal-actions">
        <button class="btn-cancelar" onclick={cerrarModal}>Cancelar</button>
        <button class="btn-guardar" onclick={guardarCambioEstado}>Guardar</button>
      </div>
    </div>
  </div>
{/if}

<style>
  .dashboard {
    display: flex;
    flex-direction: column;
    gap: 2rem;
  }

  /* Estadísticas */
  .stats-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
    gap: 1rem;
  }

  .stat-card {
    background: #fff;
    border-left: 4px solid var(--primary-color);
    padding: 1.5rem;
    border-radius: 6px;
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  }

  .stat-card.stat-secondary {
    border-left-color: var(--secondary-color);
  }

  .stat-number {
    font-size: 2rem;
    font-weight: bold;
    color: var(--primary-color);
    margin-bottom: 0.5rem;
  }

  .stat-card.stat-secondary .stat-number {
    color: var(--secondary-color);
  }

  .stat-label {
    font-size: 0.85rem;
    color: var(--secondary-color);
    text-transform: uppercase;
    letter-spacing: 0.05em;
  }

  /* Filtros */
  .filter-section {
    background: #fff;
    padding: 1.5rem;
    border-radius: 6px;
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
    display: flex;
    gap: 1rem;
    flex-wrap: wrap;
    align-items: center;
  }

  .filter-group {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
  }

  .filter-group.search {
    flex: 1;
    min-width: 250px;
  }

  .filter-group label {
    font-size: 0.85rem;
    font-weight: 600;
    color: var(--text-color);
    text-transform: uppercase;
    letter-spacing: 0.05em;
  }

  .filter-group select,
  .filter-group input {
    padding: 0.6rem 0.75rem;
    border: 1px solid var(--border-color);
    border-radius: 4px;
    font-size: 0.9rem;
    font-family: inherit;
  }

  .filter-group select:focus,
  .filter-group input:focus {
    outline: none;
    border-color: var(--primary-color);
    box-shadow: 0 0 0 3px rgba(67, 56, 202, 0.1);
  }

  .filter-result {
    align-self: flex-end;
    font-size: 0.85rem;
    color: var(--secondary-color);
  }

  /* Tabla */
  .table-container {
    background: #fff;
    border-radius: 6px;
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
    overflow-x: auto;
  }

  .reports-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 0.9rem;
  }

  .reports-table thead {
    background: #f9fafb;
    border-bottom: 2px solid var(--border-color);
  }

  .reports-table th {
    padding: 1rem;
    text-align: left;
    font-weight: 600;
    color: var(--text-color);
    text-transform: uppercase;
    letter-spacing: 0.05em;
    font-size: 0.75rem;
    cursor: pointer;
    user-select: none;
    transition: background 0.2s;
  }

  .reports-table th:hover {
    background: #f0f0f5;
  }

  .reports-table th.active {
    color: var(--primary-color);
  }

  .reports-table tbody tr {
    border-bottom: 1px solid var(--border-color);
    transition: background 0.2s;
  }

  .reports-table tbody tr.clickable {
    cursor: pointer;
  }

  .reports-table tbody tr:hover {
    background: #fafbfc;
  }

  .reports-table tbody tr.clickable:hover {
    background: #f0f0f5;
    border-bottom-color: var(--primary-color);
  }

  .reports-table td.acciones {
    text-align: center;
  }

  .btn-cambiar-estado {
    padding: 0.4rem 0.8rem;
    background: var(--primary-color);
    color: white;
    border: none;
    border-radius: 4px;
    font-size: 0.8rem;
    font-weight: 600;
    cursor: pointer;
    transition: background 0.2s;
  }

  .btn-cambiar-estado:hover {
    background: var(--primary-hover);
  }

  /* Modal */
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }

  .modal-content {
    background: white;
    border-radius: 8px;
    padding: 2rem;
    max-width: 400px;
    box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
  }

  .modal-content h2 {
    margin: 0 0 1rem 0;
    font-size: 1.3rem;
    color: var(--text-color);
  }

  .modal-desc {
    margin: 0 0 1.5rem 0;
    color: var(--secondary-color);
    font-size: 0.9rem;
    line-height: 1.5;
  }

  .form-group {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
    margin-bottom: 1.5rem;
  }

  .form-group label {
    font-weight: 600;
    color: var(--text-color);
  }

  .form-group select {
    padding: 0.6rem;
    border: 1px solid var(--border-color);
    border-radius: 4px;
    font-size: 0.9rem;
    font-family: inherit;
  }

  .form-group select:focus {
    outline: none;
    border-color: var(--primary-color);
    box-shadow: 0 0 0 3px rgba(67, 56, 202, 0.1);
  }

  .modal-actions {
    display: flex;
    gap: 1rem;
    justify-content: flex-end;
  }

  .btn-cancelar,
  .btn-guardar {
    padding: 0.6rem 1.2rem;
    border: none;
    border-radius: 4px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s;
  }

  .btn-cancelar {
    background: #f3f4f6;
    color: var(--text-color);
  }

  .btn-cancelar:hover {
    background: #e5e7eb;
  }

  .btn-guardar {
    background: var(--primary-color);
    color: white;
  }

  .btn-guardar:hover {
    background: var(--primary-hover);
  }

  .reports-table td {
    padding: 0.875rem 1rem;
    color: var(--text-color);
  }

  .reports-table td.fecha {
    font-weight: 500;
    color: var(--secondary-color);
    font-size: 0.85rem;
  }

  .reports-table td.descripcion,
  .reports-table td.direccion {
    max-width: 200px;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
  }

  .reports-table td.center {
    text-align: center;
    font-weight: 600;
  }

  .reports-table td.usuario {
    font-size: 0.8rem;
    color: var(--secondary-color);
    font-family: 'Courier New', monospace;
  }

  /* Badges */
  .badge {
    display: inline-block;
    padding: 0.35rem 0.75rem;
    border-radius: 999px;
    font-size: 0.75rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.05em;
  }

  .badge-categoria {
    background: #dbeafe;
    color: #1e40af;
  }

  .badge-pendiente {
    background: #fef3c7;
    color: #92400e;
  }

  .badge-proceso {
    background: #bfdbfe;
    color: #1e3a8a;
  }

  .badge-resuelto {
    background: #dcfce7;
    color: #166534;
  }

  .badge-rechazado {
    background: #fee2e2;
    color: #991b1b;
  }

  /* Estados */
  .error-box {
    background: #fee2e2;
    border: 1px solid #fca5a5;
    color: #991b1b;
    padding: 1rem;
    border-radius: 6px;
    margin-top: 1rem;
  }

  .empty-state {
    background: #fff;
    padding: 3rem 1.5rem;
    border-radius: 6px;
    text-align: center;
    color: var(--secondary-color);
  }

  @media (max-width: 768px) {
    .filter-section {
      flex-direction: column;
      align-items: stretch;
    }

    .filter-group.search {
      min-width: auto;
    }

    .filter-result {
      align-self: auto;
    }

    .reports-table {
      font-size: 0.8rem;
    }

    .reports-table td {
      padding: 0.625rem 0.75rem;
    }

    .reports-table th {
      padding: 0.75rem;
    }

    .reports-table td.descripcion,
    .reports-table td.direccion {
      max-width: 120px;
    }
  }
</style>
