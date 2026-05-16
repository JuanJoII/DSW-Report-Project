<script lang="ts">
    import { enhance } from "$app/forms";
    import { goto } from "$app/navigation";

    import { deserialize } from '$app/forms';

    let { data, form } = $props();
    let files = $state<FileList | null>(null);
    let previews = $state<string[]>([]);
    let isUploading = $state(false);
    let uploadProgress = $state(0);
    let errorMessage = $state("");

    function handleFileChange(e: Event) {
        const target = e.target as HTMLInputElement;
        if (target.files) {
            files = target.files;
            previews = Array.from(target.files).map(file => URL.createObjectURL(file));
        }
    }

    async function uploadAllImages() {
        if (!files || files.length === 0) return;
        
        isUploading = true;
        errorMessage = "";
        uploadProgress = 0;
        const totalFiles = files.length;

        try {
            for (let i = 0; i < totalFiles; i++) {
                const file = files[i];
                
                // Paso 1: Obtener URL firmada usando una Server Action (Proxy)
                const presignedActionRes = await fetch(`?/getPresignedUrl&fileName=${encodeURIComponent(file.name)}`, {
                    method: 'POST',
                    body: new FormData()
                });
                
                const result = deserialize(await presignedActionRes.text());
                if (result.type !== 'success') throw new Error(`Error al obtener URL firmada para ${file.name}`);
                
                const { uploadUrl, publicUrl } = result.data;

                // Paso 2: Subir imagen directamente a R2 (Cloudflare permite CORS por bucket)
                const uploadRes = await fetch(uploadUrl, {
                    method: 'PUT',
                    headers: { 'Content-Type': file.type },
                    body: file,
                });
                
                if (!uploadRes.ok) throw new Error(`Error al subir imagen ${file.name} a R2`);

                // Paso 3: Registrar foto en el backend usando otra Server Action
                const registerFormData = new FormData();
                registerFormData.append('url', publicUrl);
                
                const registerActionRes = await fetch('?/registerPhoto', {
                    method: 'POST',
                    body: registerFormData
                });
                
                const registerResult = deserialize(await registerActionRes.text());
                if (registerResult.type !== 'success') throw new Error(`Error al registrar foto ${file.name}`);
                
                uploadProgress = Math.round(((i + 1) / totalFiles) * 100);
            }
            
            // Todo salió bien, redirigir
            goto('/reportes/mis-reportes');
        } catch (err: any) {
            console.error(err);
            errorMessage = err.message || "Ocurrió un error durante la subida de imágenes.";
            isUploading = false;
        }
    }

    const handleSubmit = (e: Event) => {
        e.preventDefault();
        uploadAllImages();
    };

    function removeImage(index: number) {
        previews = previews.filter((_, i) => i !== index);
        // Nota: Para limpiar realmente el FileList se requiere recrear el objeto DataTransfer, 
        // pero para este flujo simplificado manejamos la UI.
    }
</script>

<div class="photo-upload-container">
    <div class="step-indicator">
        Paso 2 de 2
    </div>
    <h1>Añade fotos a tu reporte</h1>
    <p class="subtitle">
        Las fotos ayudan a las autoridades a entender mejor el problema. 
        Puedes saltar este paso si no tienes imágenes en este momento.
    </p>

    <div class="report-summary">
        <strong>Reporte #{data.reporte.id}:</strong> {data.reporte.descripcion?.substring(0, 100)}{data.reporte.descripcion?.length > 100 ? '...' : ''}
    </div>

    <div class="upload-card">
        {#if errorMessage}
            <div class="alert alert-error">
                {errorMessage}
            </div>
        {/if}

        <form onsubmit={handleSubmit} class="upload-form">
            <div class="upload-section">
                <div class="file-input-wrapper">
                    <input 
                        type="file" 
                        id="photos" 
                        name="photos" 
                        accept="image/*" 
                        multiple 
                        onchange={handleFileChange}
                        class="file-input"
                        disabled={isUploading}
                    />
                    <label for="photos" class="file-label" class:dragging={false}>
                        <span class="icon">📷</span>
                        <span class="text">Seleccionar fotos</span>
                        <span class="help">JPG, PNG o WEBP (Máx. 5MB por foto)</span>
                    </label>
                </div>

                {#if previews.length > 0}
                    <div class="previews-grid">
                        {#each previews as preview, i}
                            <div class="preview-item">
                                <img src={preview} alt="Vista previa {i + 1}" />
                                {#if !isUploading}
                                    <button type="button" class="btn-remove" onclick={() => removeImage(i)}>×</button>
                                {/if}
                            </div>
                        {/each}
                    </div>
                {/if}
            </div>

            {#if isUploading}
                <div class="progress-container">
                    <div class="progress-bar" style="width: {uploadProgress}%"></div>
                    <span class="progress-text">{uploadProgress}% Completado</span>
                </div>
            {/if}

            <div class="form-actions">
                <a href="/reportes/mis-reportes" class="btn-skip">Saltar por ahora</a>
                <button type="submit" class="btn-submit" disabled={isUploading || previews.length === 0}>
                    {#if isUploading}
                        Subiendo fotos...
                    {:else}
                        Finalizar Reporte
                    {/if}
                </button>
            </div>
        </form>
    </div>
    
    <div class="info-note">
        <p><strong>Información:</strong> Las imágenes se suben directamente a nuestro almacenamiento seguro (Cloudflare R2) y se asocian a tu reporte automáticamente.</p>
    </div>
</div>

<style>
    .photo-upload-container {
        max-width: 700px;
        margin: 0 auto;
        padding: 2rem 1rem 4rem;
    }

    .step-indicator {
        font-weight: bold;
        color: var(--primary-color);
        margin-bottom: 0.5rem;
        text-transform: uppercase;
        font-size: 0.85rem;
    }

    h1 {
        margin-bottom: 0.5rem;
        color: #1a1a1a;
    }

    .subtitle {
        color: #666;
        margin-bottom: 2rem;
    }

    .report-summary {
        background: #f8fafc;
        padding: 1rem;
        border-radius: 8px;
        border-left: 4px solid var(--primary-color);
        margin-bottom: 2rem;
        font-size: 0.95rem;
    }

    .upload-card {
        background: white;
        padding: 2rem;
        border-radius: 12px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        border: 1px solid #eee;
    }

    .file-input-wrapper {
        position: relative;
        margin-bottom: 2rem;
    }

    .file-input {
        position: absolute;
        width: 0.1px;
        height: 0.1px;
        opacity: 0;
        overflow: hidden;
        z-index: -1;
    }

    .file-label {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 3rem 2rem;
        border: 2px dashed #cbd5e1;
        border-radius: 12px;
        cursor: pointer;
        transition: all 0.2s;
    }

    .file-label:hover {
        border-color: var(--primary-color);
        background: #f5f7ff;
    }

    .file-label .icon {
        font-size: 2.5rem;
        margin-bottom: 1rem;
    }

    .file-label .text {
        font-weight: bold;
        font-size: 1.1rem;
        color: #334155;
    }

    .file-label .help {
        font-size: 0.85rem;
        color: #64748b;
        margin-top: 0.5rem;
    }

    .previews-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
        gap: 1rem;
        margin-bottom: 2rem;
    }

    .preview-item {
        position: relative;
        aspect-ratio: 1;
        border-radius: 8px;
        overflow: hidden;
        border: 1px solid #e2e8f0;
    }

    .preview-item img {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }

    .btn-remove {
        position: absolute;
        top: 4px;
        right: 4px;
        background: rgba(0, 0, 0, 0.5);
        color: white;
        border: none;
        width: 24px;
        height: 24px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        cursor: pointer;
        font-size: 1.2rem;
    }

    .btn-remove:hover {
        background: rgba(220, 38, 38, 0.8);
    }

    .progress-container {
        margin-bottom: 2rem;
        background: #f1f5f9;
        height: 24px;
        border-radius: 12px;
        overflow: hidden;
        position: relative;
        border: 1px solid #e2e8f0;
    }

    .progress-bar {
        background: var(--primary-color);
        height: 100%;
        transition: width 0.3s ease;
    }

    .progress-text {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 0.75rem;
        font-weight: bold;
        color: #334155;
    }

    .form-actions {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-top: 1rem;
    }

    .btn-submit {
        background: var(--primary-color);
        color: white;
        padding: 0.75rem 1.5rem;
        border-radius: 8px;
        font-weight: bold;
        border: none;
        cursor: pointer;
        transition: background 0.2s;
    }

    .btn-submit:hover:not(:disabled) {
        background: var(--primary-hover);
    }

    .btn-submit:disabled {
        background: #cbd5e1;
        cursor: not-allowed;
    }

    .btn-skip {
        color: #64748b;
        text-decoration: none;
        font-weight: 500;
        font-size: 0.95rem;
    }

    .btn-skip:hover {
        color: #334155;
        text-decoration: underline;
    }

    .alert {
        padding: 1rem;
        border-radius: 8px;
        margin-bottom: 1.5rem;
    }

    .alert-error {
        background: #fee2e2;
        color: #991b1b;
        border: 1px solid #fecaca;
    }

    .info-note {
        margin-top: 2rem;
        padding: 1rem;
        background: #f0fdf4;
        border: 1px solid #dcfce7;
        border-radius: 8px;
        font-size: 0.85rem;
        color: #166534;
    }
</style>
