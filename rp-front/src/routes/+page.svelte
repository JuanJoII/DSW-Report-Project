<script lang="ts">
  let { data } = $props();
  let isAdmin = $derived(data.user?.role?.toLowerCase() === 'admin');

  function formatDate(dateString: string) {
    return new Date(dateString).toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  function getStatusClass(status: string) {
    status = status.toLowerCase();
    if (status.includes('pendiente')) return 'status-pending';
    if (status.includes('revisión')) return 'status-review';
    if (status.includes('resuelto')) return 'status-resolved';
    return 'status-default';
  }
</script>

<div class="hero">
  <div class="hero-content">
    <h1>Reporta Problemas en tu Comunidad</h1>
    <p>Ayúdanos a identificar baches, fallas en iluminación o cualquier inconveniente en la vía pública.</p>
    <div class="hero-actions">
      <a href="/reportes/nuevo" class="btn btn-primary btn-lg">Crear Reporte</a>
      <a href="#recientes" class="btn btn-secondary btn-lg">Ver Reportes</a>
    </div>
  </div>
</div>

{#if isAdmin}
  <section id="recientes" class="recent-reports">
    <div class="section-header">
      <h2>📊 Reportes Recientes del Sistema</h2>
      <p>Los últimos reportes enviados por usuarios</p>
    </div>

    {#if data.reportesRecientes.length === 0}
      <p class="empty-msg">No hay reportes recientes registrados.</p>
    {:else}
      <div class="report-list">
        {#each data.reportesRecientes as reporte}
          <div class="report-item">
            <div class="report-info">
              <div class="report-meta">
                <span class="category">{reporte.nombreCategoria}</span>
                <span class="date">{formatDate(reporte.fechaCreacion)}</span>
              </div>
              <h3>{reporte.descripcion}</h3>
              <span class="status {getStatusClass(reporte.nombreEstado)}">{reporte.nombreEstado}</span>
            </div>
            <a href="/reportes/{reporte.id}" class="view-link">Detalles →</a>
          </div>
        {/each}
      </div>
    {/if}
  </section>
{/if}

<style>
  .hero {
    background: linear-gradient(135deg, #4338ca 0%, #1e1b4b 100%);
    color: white;
    padding: 6rem 2rem;
    border-radius: 20px;
    text-align: center;
    margin-bottom: 4rem;
  }

  .hero-content {
    max-width: 800px;
    margin: 0 auto;
  }

  .hero h1 {
    font-size: 3rem;
    margin-bottom: 1.5rem;
    font-weight: 800;
  }

  .hero p {
    font-size: 1.25rem;
    margin-bottom: 2.5rem;
    opacity: 0.9;
  }

  .hero-actions {
    display: flex;
    gap: 1.5rem;
    justify-content: center;
  }

  .btn-lg {
    padding: 1rem 2.5rem;
    font-size: 1.1rem;
    border-radius: 12px;
  }

  .recent-reports {
    padding: 2rem 0;
  }

  .section-header {
    margin-bottom: 3rem;
  }

  .section-header h2 {
    font-size: 2rem;
    margin-bottom: 0.5rem;
  }

  .section-header p {
    color: #666;
  }

  .report-list {
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }

  .report-item {
    background: white;
    padding: 1.5rem;
    border-radius: 12px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
    border: 1px solid #eee;
    transition: transform 0.2s;
  }

  .report-item:hover {
    transform: translateY(-2px);
    border-color: var(--primary-color);
  }

  .report-meta {
    display: flex;
    gap: 1rem;
    margin-bottom: 0.5rem;
    font-size: 0.85rem;
  }

  .category {
    font-weight: bold;
    color: var(--primary-color);
  }

  .date {
    color: #999;
  }

  .report-info h3 {
    margin: 0 0 1rem 0;
    font-size: 1.1rem;
    color: #333;
  }

  .status {
    font-size: 0.75rem;
    font-weight: 700;
    padding: 0.25rem 0.6rem;
    border-radius: 999px;
    text-transform: uppercase;
  }

  .status-pending { background: #fef3c7; color: #92400e; }
  .status-review { background: #e0e7ff; color: #3730a3; }
  .status-resolved { background: #d1fae5; color: #065f46; }
  .status-default { background: #f3f4f6; color: #374151; }

  .view-link {
    text-decoration: none;
    color: var(--primary-color);
    font-weight: 600;
  }

  .empty-msg {
    text-align: center;
    padding: 3rem;
    background: #f9fafb;
    border-radius: 12px;
    color: #666;
  }
</style>
