<script lang="ts">
    import { Map } from "$lib/index";

    let { data } = $props();
    const { reporte } = data;

    function formatDate(dateString: string) {
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
    <div class="back-nav">
        <a href="/reportes/mis-reportes" class="btn-back">← Volver a mis reportes</a>
    </div>

    <div class="report-header">
        <div class="title-section">
            <h1>Reporte #{reporte.id}</h1>
            <span class="category">{reporte.nombreCategoria}</span>
        </div>
        <div class="status-badge {getStatusClass(reporte.nombreEstado)}">
            {reporte.nombreEstado}
        </div>
    </div>

    <div class="grid">
        <div class="main-content">
            <section class="card info-card">
                <h2>Descripción</h2>
                <p>{reporte.descripcion}</p>
                
                <div class="meta-info">
                    <div class="meta-item">
                        <strong>Fecha de creación:</strong>
                        <span>{formatDate(reporte.fechaCreacion)}</span>
                    </div>
                    <div class="meta-item">
                        <strong>Dirección:</strong>
                        <span>{reporte.direccionTexto}</span>
                    </div>
                </div>
            </section>

            {#if reporte.fotos && reporte.fotos.length > 0}
                <section class="card photos-card">
                    <h2>Evidencia Fotográfica</h2>
                    <div class="photo-grid">
                        {#each reporte.fotos as foto}
                            <img src={foto.url} alt="Evidencia del reporte" />
                        {/each}
                    </div>
                </section>
            {/if}

            <section class="card history-card">
                <h2>Historial de Cambios</h2>
                {#if reporte.historial && reporte.historial.length > 0}
                    <div class="timeline">
                        {#each reporte.historial as h}
                            <div class="timeline-item">
                                <div class="timeline-marker"></div>
                                <div class="timeline-content">
                                    <div class="timeline-header">
                                        <span class="new-status {getStatusClass(h.nombreEstadoNuevo)}">
                                            {h.nombreEstadoNuevo}
                                        </span>
                                        <span class="time">{formatDate(h.fechaCambio)}</span>
                                    </div>
                                    {#if h.comentario}
                                        <p class="comment">{h.comentario}</p>
                                    {/if}
                                </div>
                            </div>
                        {/each}
                    </div>
                {:else}
                    <p class="empty-msg">No hay cambios registrados todavía.</p>
                {/if}
            </section>
        </div>

        <aside class="sidebar">
            <div class="card map-card">
                <h2>Ubicación Exacta</h2>
                <div class="mini-map">
                    <Map latitud={reporte.latitud} longitud={reporte.longitud} />
                </div>
                <div class="coords">
                    <span>Lat: {reporte.latitud.toFixed(6)}</span>
                    <span>Lng: {reporte.longitud.toFixed(6)}</span>
                </div>
            </div>
        </aside>
    </div>
</div>

<style>
    .container {
        max-width: 1100px;
        margin: 0 auto;
        padding-bottom: 4rem;
    }

    .back-nav {
        margin-bottom: 1.5rem;
    }

    .btn-back {
        color: var(--secondary-color);
        text-decoration: none;
        font-weight: 500;
        font-size: 0.9rem;
    }

    .btn-back:hover {
        color: var(--primary-color);
    }

    .report-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 2rem;
    }

    .title-section h1 {
        margin: 0 0 0.25rem 0;
        font-size: 2rem;
    }

    .category {
        color: var(--primary-color);
        font-weight: 600;
        text-transform: uppercase;
        letter-spacing: 0.05em;
        font-size: 0.85rem;
    }

    .status-badge {
        padding: 0.5rem 1.25rem;
        border-radius: 999px;
        font-weight: 800;
        font-size: 0.9rem;
        text-transform: uppercase;
    }

    .status-pending { background: #fef3c7; color: #92400e; }
    .status-review { background: #e0e7ff; color: #3730a3; }
    .status-resolved { background: #d1fae5; color: #065f46; }
    .status-default { background: #f3f4f6; color: #374151; }

    .grid {
        display: grid;
        grid-template-columns: 1fr;
        gap: 2rem;
    }

    @media (min-width: 900px) {
        .grid {
            grid-template-columns: 1fr 350px;
        }
    }

    .card {
        background: white;
        padding: 1.5rem;
        border-radius: 12px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        border: 1px solid #eee;
        margin-bottom: 1.5rem;
    }

    .card h2 {
        font-size: 1.1rem;
        margin: 0 0 1.25rem 0;
        color: #111827;
        border-bottom: 1px solid #f0f0f0;
        padding-bottom: 0.75rem;
    }

    .info-card p {
        font-size: 1.05rem;
        line-height: 1.6;
        color: #374151;
        margin-bottom: 1.5rem;
    }

    .meta-info {
        display: flex;
        flex-direction: column;
        gap: 0.75rem;
        padding: 1rem;
        background: #f9fafb;
        border-radius: 8px;
    }

    .meta-item {
        display: flex;
        gap: 0.5rem;
        font-size: 0.9rem;
    }

    .meta-item strong {
        color: #6b7280;
    }

    .photo-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
        gap: 1rem;
    }

    .photo-grid img {
        width: 100%;
        height: 150px;
        object-fit: cover;
        border-radius: 8px;
    }

    .timeline {
        position: relative;
        padding-left: 2rem;
    }

    .timeline-item {
        position: relative;
        padding-bottom: 1.5rem;
    }

    .timeline-marker {
        position: absolute;
        left: -2rem;
        width: 12px;
        height: 12px;
        border-radius: 50%;
        background: var(--primary-color);
        border: 3px solid white;
        box-shadow: 0 0 0 2px var(--primary-color);
        z-index: 2;
    }

    .timeline-item:not(:last-child)::before {
        content: '';
        position: absolute;
        left: -1.65rem;
        top: 12px;
        width: 2px;
        height: 100%;
        background: #e5e7eb;
        z-index: 1;
    }

    .timeline-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 0.5rem;
    }

    .new-status {
        font-size: 0.75rem;
        font-weight: bold;
        padding: 0.2rem 0.5rem;
        border-radius: 4px;
    }

    .time {
        font-size: 0.75rem;
        color: #6b7280;
    }

    .comment {
        font-size: 0.9rem;
        color: #4b5563;
        margin: 0;
        background: #f3f4f6;
        padding: 0.5rem;
        border-radius: 6px;
    }

    .mini-map {
        height: 300px;
        margin-bottom: 1rem;
    }

    .coords {
        display: flex;
        justify-content: space-between;
        font-family: monospace;
        font-size: 0.8rem;
        color: #6b7280;
    }

    .empty-msg {
        color: #9ca3af;
        font-style: italic;
    }
</style>
