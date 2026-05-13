<script lang="ts">
    let { data } = $props();
    let reportes = $derived(data.reportes || []);

    function formatDate(dateString: string) {
        if (!dateString) return "Sin fecha";
        return new Date(dateString).toLocaleDateString('es-ES', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    function getStatusClass(status: string) {
        if (!status) return 'status-default';
        status = status.toLowerCase();
        if (status.includes('pendiente')) return 'status-pending';
        if (status.includes('revisión')) return 'status-review';
        if (status.includes('resuelto')) return 'status-resolved';
        return 'status-default';
    }
</script>

<div class="container">
    <div class="header">
        <h1>Mis Reportes</h1>
        <a href="/reportes/nuevo" class="btn-new">Nuevo Reporte</a>
    </div>

    {#if !reportes || reportes.length === 0}
        <div class="empty-state">
            <div class="icon">📋</div>
            <h2>Aún no tienes reportes</h2>
            <p>Cuando crees un reporte, aparecerá aquí para que puedas seguir su estado.</p>
            <a href="/reportes/nuevo" class="btn-primary">Crear mi primer reporte</a>
        </div>
    {:else}
        <div class="report-grid">
            {#each reportes as reporte}
                <div class="report-card">
                    <div class="card-header">
                        <span class="category">{reporte.nombreCategoria || 'Sin categoría'}</span>
                        <span class="status {getStatusClass(reporte.nombreEstado)}">
                            {reporte.nombreEstado || 'Pendiente'}
                        </span>
                    </div>

                    <div class="card-body">
                        <p class="description">{reporte.descripcion}</p>
                        <div class="meta">
                            <span class="date">📅 {formatDate(reporte.fechaCreacion)}</span>
                            <span class="photos">📷 {reporte.totalFotos || 0} fotos</span>
                        </div>
                    </div>

                    <div class="card-footer">
                        <a href="/reportes/{reporte.id}" class="btn-detail">Ver detalle</a>
                    </div>
                </div>
            {/each}
        </div>
    {/if}
</div>

<style>
    /* ... (mismos estilos de antes) ... */
    .container { max-width: 1000px; margin: 0 auto; }
    .header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem; }
    .btn-new { background: var(--primary-color); color: white; padding: 0.6rem 1.2rem; border-radius: 8px; text-decoration: none; font-weight: 600; }
    .report-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1.5rem; }
    .report-card { background: white; border-radius: 12px; overflow: hidden; box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1); border: 1px solid #eee; display: flex; flex-direction: column; }
    .card-header { padding: 1rem; display: flex; justify-content: space-between; align-items: center; background: #f9fafb; border-bottom: 1px solid #f0f0f0; }
    .category { font-weight: bold; color: var(--primary-color); font-size: 0.85rem; text-transform: uppercase; }
    .status { font-size: 0.75rem; font-weight: 700; padding: 0.25rem 0.6rem; border-radius: 999px; text-transform: uppercase; }
    .status-pending { background: #fef3c7; color: #92400e; }
    .status-review { background: #e0e7ff; color: #3730a3; }
    .status-resolved { background: #d1fae5; color: #065f46; }
    .status-default { background: #f3f4f6; color: #374151; }
    .card-body { padding: 1.25rem; flex-grow: 1; }
    .description { margin: 0 0 1rem 0; font-size: 0.95rem; color: #374151; display: -webkit-box; -webkit-line-clamp: 3; -webkit-box-orient: vertical; overflow: hidden; }
    .meta { display: flex; flex-direction: column; gap: 0.4rem; font-size: 0.8rem; color: #6b7280; }
    .card-footer { padding: 1rem; border-top: 1px solid #f0f0f0; }
    .btn-detail { display: block; text-align: center; padding: 0.5rem; background: #f3f4f6; color: #374151; text-decoration: none; border-radius: 6px; font-weight: 600; font-size: 0.9rem; }
    .empty-state { text-align: center; padding: 4rem 2rem; background: white; border-radius: 12px; border: 2px dashed #ddd; }
    .empty-state .icon { font-size: 4rem; margin-bottom: 1rem; }
    .btn-primary { display: inline-block; background: var(--primary-color); color: white; padding: 0.75rem 1.5rem; border-radius: 8px; text-decoration: none; font-weight: 600; }
</style>
