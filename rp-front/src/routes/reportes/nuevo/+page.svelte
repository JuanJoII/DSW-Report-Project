<script lang="ts">
    import { Map, AddressSearch, CategorySearch } from "$lib/index";
    import { enhance } from "$app/forms";

    let { data, form } = $props();

    let lat = $state(4.5709);
    let lng = $state(-74.2973);
    let address = $state("");
    let catId = $state(null);
    let catValue = $state("");
    let descripcion = $state("");
    let isSubmitting = $state(false);

    function handleAddressSelect(newLat: number, newLng: number, displayName: string) {
        lat = newLat;
        lng = newLng;
        address = displayName;
    }

    const handleSubmit = () => {
        isSubmitting = true;
        return async ({ update }) => {
            isSubmitting = false;
            await update();
        };
    };
</script>

<div class="report-container">
    <h1>Crear Nuevo Reporte</h1>
    <p class="subtitle">Cuéntanos qué está pasando y dónde. Tu reporte ayudará a mejorar nuestra comunidad.</p>

    <form method="POST" use:enhance={handleSubmit} class="report-form">
        {#if form?.message}
            <div class="alert alert-error">
                {form.message}
            </div>
        {/if}

        <div class="form-section">
            <h2>1. Información del Problema</h2>
            
            <div class="form-group">
                <label for="categoria">Categoría</label>
                <CategorySearch 
                    categorias={data.categorias} 
                    bind:selectedId={catId} 
                    bind:value={catValue} 
                />
            </div>

            <div class="form-group">
                <label for="descripcion">Descripción</label>
                <textarea 
                    id="descripcion" 
                    name="descripcion" 
                    bind:value={descripcion} 
                    placeholder="Describe el problema detalladamente..." 
                    rows="4" 
                    required
                ></textarea>
            </div>
        </div>

        <div class="form-section">
            <h2>2. Ubicación</h2>
            <p class="form-help">Busca la dirección o mueve el marcador en el mapa para mayor precisión.</p>
            
            <div class="location-wrapper">
                <AddressSearch onSelect={handleAddressSelect} />
                
                <input 
                    type="hidden" 
                    name="direccionTexto" 
                    bind:value={address} 
                />

                <Map bind:latitud={lat} bind:longitud={lng} />
            </div>
            
            <input type="hidden" name="latitud" value={lat} />
            <input type="hidden" name="longitud" value={lng} />
        </div>

        <div class="form-actions">
            <button type="submit" class="btn-submit" disabled={isSubmitting}>
                {#if isSubmitting}
                    Enviando reporte...
                {:else}
                    Publicar Reporte
                {/if}
            </button>
        </div>
    </form>
</div>

<style>
    .report-container {
        max-width: 900px;
        margin: 0 auto;
        padding-bottom: 4rem;
    }

    h1 {
        margin-bottom: 0.5rem;
        color: #1a1a1a;
    }

    .subtitle {
        color: #666;
        margin-bottom: 2rem;
    }

    .report-form {
        display: flex;
        flex-direction: column;
        gap: 2rem;
    }

    .form-section {
        background: #fff;
        padding: 2rem;
        border-radius: 12px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        border: 1px solid #eee;
    }

    .form-section h2 {
        font-size: 1.25rem;
        margin-bottom: 1.5rem;
        color: var(--primary-color);
        border-bottom: 2px solid #f0f0f0;
        padding-bottom: 0.5rem;
    }

    .form-group {
        margin-bottom: 1.5rem;
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
    }

    .form-group label {
        font-weight: 600;
        font-size: 0.95rem;
    }

    .form-help {
        font-size: 0.85rem;
        color: #666;
        margin-bottom: 1rem;
    }

    input[type="text"],
    select,
    textarea {
        padding: 0.75rem 1rem;
        border: 1px solid #ddd;
        border-radius: 8px;
        font-size: 1rem;
        transition: border-color 0.2s;
    }

    input[type="text"]:focus,
    select:focus,
    textarea:focus {
        outline: none;
        border-color: var(--primary-color);
        box-shadow: 0 0 0 3px rgba(67, 56, 202, 0.1);
    }

    .alert {
        padding: 1rem;
        border-radius: 8px;
        margin-bottom: 1rem;
        font-weight: 500;
    }

    .alert-error {
        background: #fee2e2;
        color: #991b1b;
        border: 1px solid #fecaca;
    }

    .form-actions {
        display: flex;
        justify-content: flex-end;
    }

    .btn-submit {
        background: var(--primary-color);
        color: white;
        padding: 1rem 2rem;
        border-radius: 8px;
        font-weight: bold;
        font-size: 1.1rem;
        border: none;
        cursor: pointer;
        transition: background 0.2s;
        width: 100%;
    }

    .btn-submit:hover {
        background: var(--primary-hover);
    }

    .btn-submit:disabled {
        background: #a5b4fc;
        cursor: not-allowed;
    }

    @media (min-width: 640px) {
        .btn-submit {
            width: auto;
        }
    }
</style>
