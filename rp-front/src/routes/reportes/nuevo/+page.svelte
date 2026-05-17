<script lang="ts">
    import { Map, AddressSearch, CategorySearch } from "$lib/index";
    import { enhance } from "$app/forms";
    import { goto } from "$app/navigation";

    let { data, form } = $props();

    let lat = $state(4.5709);
    let lng = $state(-74.2973);
    let address = $state("");
    let catId = $state(null);
    let catValue = $state("");
    let descripcion = $state("");
    let isSubmitting = $state(false);

    // Reverse Geocoding para obtener dirección desde coordenadas
    async function reverseGeocode(lt: number, lg: number) {
        try {
            const res = await fetch(`https://nominatim.openstreetmap.org/reverse?format=json&lat=${lt}&lon=${lg}`);
            if (res.ok) {
                const data = await res.json();
                address = data.display_name;
            }
        } catch (e) {
            console.error("Error en reverse geocoding:", e);
        }
    }

    // Efecto para actualizar dirección cuando cambia la ubicación (por GPS o manual)
    $effect(() => {
        if (lat && lng) {
            reverseGeocode(lat, lng);
        }
    });

    function handleAddressSelect(newLat: number, newLng: number, displayName: string) {
        lat = newLat;
        lng = newLng;
        address = displayName;
    }

    const handleSubmit = () => {
        isSubmitting = true;
        return async ({ result, update }) => {
            isSubmitting = false;
            if (result.type === 'redirect') {
                goto(result.location);
            } else {
                await update();
            }
        };
    };
</script>

<div class="max-w-4xl mx-auto space-y-12 pb-20 animate-in fade-in duration-500">
    <div class="text-center space-y-4">
        <h1 class="text-4xl md:text-6xl font-bold text-slate-900 dark:text-white font-jost">Crear Nuevo Reporte</h1>
        <p class="text-lg text-slate-500 dark:text-slate-400 font-onest max-w-2xl mx-auto leading-relaxed">
            Tu reporte es el primer paso para una mejor ciudad. Cuéntanos qué sucede y dónde para que podamos actuar.
        </p>
    </div>

    <form method="POST" use:enhance={handleSubmit} class="space-y-10">
        {#if form?.message}
            <div class="p-4 bg-red-50 dark:bg-red-900/20 border border-red-100 dark:border-red-800 text-red-600 dark:text-red-400 rounded-2xl font-bold text-sm flex items-center gap-3">
                <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"/><line x1="12" x2="12" y1="8" y2="12"/><line x1="12" x2="12.01" y1="16" y2="16"/></svg>
                {form.message}
            </div>
        {/if}

        <div class="bg-white dark:bg-slate-800 p-10 rounded-[2.5rem] shadow-sm border border-slate-100 dark:border-slate-700 space-y-10">
            <div class="space-y-8">
                <div class="flex items-center gap-4">
                    <div class="w-10 h-10 bg-violet-50 dark:bg-violet-900/30 text-sabana-violet dark:text-sabana-lila rounded-xl flex items-center justify-center font-bold font-jost text-lg">1</div>
                    <h2 class="text-2xl font-bold text-slate-900 dark:text-white font-jost">Información del Problema</h2>
                </div>
                
                <div class="space-y-6">
                    <div class="space-y-2">
                        <label class="text-xs font-bold text-slate-400 dark:text-slate-500 uppercase tracking-widest ml-1" for="categoria">Categoría del daño</label>
                        <CategorySearch 
                            categorias={data.categorias} 
                            bind:selectedId={catId} 
                            bind:value={catValue} 
                        />
                    </div>

                    <div class="space-y-2">
                        <label class="text-xs font-bold text-slate-400 dark:text-slate-500 uppercase tracking-widest ml-1" for="descripcion">Descripción detallada</label>
                        <textarea 
                            id="descripcion" 
                            name="descripcion" 
                            bind:value={descripcion} 
                            placeholder="Describe lo que sucede. Por ejemplo: 'Hay un bache profundo que afecta ambos carriles...'" 
                            rows="5" 
                            class="w-full px-5 py-4 bg-slate-50 dark:bg-slate-900/50 border-transparent focus:bg-white dark:focus:bg-slate-900 focus:border-sabana-violet/30 rounded-2xl text-slate-900 dark:text-white placeholder:text-slate-400 dark:placeholder:text-slate-600 focus:ring-4 focus:ring-sabana-violet/5 transition-all outline-none border resize-none font-onest"
                            required
                        ></textarea>
                    </div>
                </div>
            </div>

            <div class="h-[1px] bg-slate-50 dark:bg-slate-700"></div>

            <div class="space-y-8">
                <div class="flex items-center gap-4">
                    <div class="w-10 h-10 bg-violet-50 dark:bg-violet-900/30 text-sabana-violet dark:text-sabana-lila rounded-xl flex items-center justify-center font-bold font-jost text-lg">2</div>
                    <h2 class="text-2xl font-bold text-slate-900 dark:text-white font-jost">Ubicación Exacta</h2>
                </div>

                <div class="space-y-6">
                    <div class="space-y-4">
                        <p class="text-sm text-slate-500 dark:text-slate-400 font-onest leading-relaxed">
                            Busca la dirección o mueve el marcador azul en el mapa para indicar el punto exacto del inconveniente.
                        </p>
                        
                        <div class="space-y-4">
                            <div class="space-y-2">
                                <label class="text-xs font-bold text-slate-400 dark:text-slate-500 uppercase tracking-widest ml-1" for="direccion-search">Buscar por dirección (Opcional)</label>
                                <AddressSearch onSelect={handleAddressSelect} />
                            </div>
                            
                            <div class="space-y-2">
                                <label class="text-xs font-bold text-slate-400 dark:text-slate-500 uppercase tracking-widest ml-1" for="direccionTexto">Confirmar Dirección</label>
                                <input 
                                    type="text" 
                                    id="direccionTexto"
                                    name="direccionTexto" 
                                    bind:value={address} 
                                    placeholder="La dirección se actualizará según el mapa..." 
                                    class="w-full px-5 py-4 bg-slate-50 dark:bg-slate-900/50 border-transparent focus:bg-white dark:focus:bg-slate-900 focus:border-sabana-violet/30 rounded-2xl text-slate-900 dark:text-white placeholder:text-slate-400 dark:placeholder:text-slate-600 focus:ring-4 focus:ring-sabana-violet/5 transition-all outline-none border font-onest"
                                    required
                                />
                                <p class="text-[10px] text-slate-400 dark:text-slate-500 ml-1 italic">Puedes editar este campo si la dirección sugerida no es exacta.</p>
                            </div>

                            <div class="h-[400px] rounded-[2rem] overflow-hidden border-4 border-slate-50 dark:border-slate-900 relative group">
                                <Map bind:latitud={lat} bind:longitud={lng} />
                            </div>
                        </div>
                    </div>
                    
                    <input type="hidden" name="latitud" value={lat} />
                    <input type="hidden" name="longitud" value={lng} />
                </div>
            </div>
        </div>

        <div class="flex flex-col sm:flex-row items-center justify-between gap-6 bg-slate-900 dark:bg-slate-950 p-8 rounded-[2.5rem] text-white shadow-2xl">
            <div class="space-y-1">
                <p class="text-lg font-bold font-jost">¿Todo listo?</p>
                <p class="text-sm text-slate-400 dark:text-slate-500 font-onest">Podrás añadir fotos en el siguiente paso.</p>
            </div>
            <button 
                type="submit" 
                class="w-full sm:w-auto px-10 py-5 bg-sabana-violet text-white rounded-2xl font-bold text-lg hover:bg-sabana-purple transition-all shadow-xl shadow-sabana-violet/20 flex items-center justify-center gap-3 disabled:opacity-50 disabled:cursor-not-allowed" 
                disabled={isSubmitting}
            >
                {#if isSubmitting}
                    <svg class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                    Procesando...
                {:else}
                    Siguiente
                    <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><line x1="5" x2="19" y1="12" y2="12"/><polyline points="12 5 19 12 12 19"/></svg>
                {/if}
            </button>
        </div>
    </form>
</div>

<style>
    @reference "../../../app.css";

    :global(.animate-in) {        animation-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
    }
</style>
