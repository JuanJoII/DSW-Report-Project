<script lang="ts">
  import { onMount, onDestroy, untrack } from "svelte";
  import { browser } from "$app/environment";
  import "leaflet/dist/leaflet.css";

  let { latitud = $bindable(4.5709), longitud = $bindable(-74.2973) } =
    $props();

  let mapElement: HTMLElement | undefined = $state();
  let map: any = $state();
  let marker: any = $state();
  let precisionGps: number | null = $state(null);
  let L: any;

  onMount(async () => {
    if (browser && mapElement) {
      L = await import("leaflet");

      // Configuración de iconos para Vite
      delete L.Icon.Default.prototype._getIconUrl;
      L.Icon.Default.mergeOptions({
        iconRetinaUrl:
          "https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon-2x.png",
        iconUrl:
          "https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon.png",
        shadowUrl:
          "https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png",
      });

      // Inicializar mapa
      map = L.map(mapElement).setView([latitud, longitud], 12);

      L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        attribution: "&copy; OpenStreetMap",
      }).addTo(map);

      // Crear el marcador inicial
      marker = L.marker([latitud, longitud], { draggable: true }).addTo(map);

      marker.on("dragend", () => {
        const position = marker.getLatLng();
        // Usamos untrack para evitar que el efecto de cambio de coordenadas mueva el mapa
        // mientras el usuario está arrastrando manualmente
        latitud = position.lat;
        longitud = position.lng;
      });

      // Intentar obtener ubicación inicial
      obtenerUbicacionReal();

      // Forzar recalculo de tamaño (importante en Svelte/Vite)
      setTimeout(() => map.invalidateSize(), 400);
    }
  });

  // Efecto para sincronizar el mapa cuando cambian las coordenadas (desde fuera o por GPS)
  $effect(() => {
    if (map && marker && L) {
      const currentPos = marker.getLatLng();
      if (currentPos.lat !== latitud || currentPos.lng !== longitud) {
        marker.setLatLng([latitud, longitud]);
        // Solo centramos el mapa automáticamente si la precisión es alta (viniendo de GPS)
        // o si es el cambio inicial
      }
    }
  });

  function obtenerUbicacionReal() {
    if (!browser || !navigator.geolocation) return;

    const opciones = {
      enableHighAccuracy: true, // ¡CLAVE! Fuerza el uso de GPS
      timeout: 10000, // Espera máximo 10 segundos
      maximumAge: 0, // No acepta ubicaciones viejas en caché
    };

    navigator.geolocation.getCurrentPosition(
      (pos) => {
        latitud = pos.coords.latitude;
        longitud = pos.coords.longitude;
        precisionGps = pos.coords.accuracy;

        if (map) {
          map.setView([latitud, longitud], 18); // Zoom alto para ver la calle
        }
      },
      (err) => {
        console.error("Error obteniendo GPS (Alta Precisión):", err);

        // Reintento de "último recurso" si falla la alta precisión
        if (err.code === err.TIMEOUT) {
          console.log("Reintentando con precisión normal...");
          navigator.geolocation.getCurrentPosition(
            (pos) => {
              latitud = pos.coords.latitude;
              longitud = pos.coords.longitude;
              precisionGps = pos.coords.accuracy;
              if (map) map.setView([latitud, longitud], 16);
            },
            (err2) => console.error("Error definitivo de GPS:", err2),
            { enableHighAccuracy: false, timeout: 5000, maximumAge: 60000 },
          );
        } else {
          alert("Error de GPS: " + err.message);
        }
      },
      opciones,
    );
  }

  onDestroy(() => {
    if (map) map.remove();
  });
</script>

<div class="map-container">
  <div bind:this={mapElement} class="canvas"></div>

  <div class="controls">
    <button onclick={obtenerUbicacionReal} class="btn-gps">
      📍 Mi ubicación actual
    </button>
  </div>

  <div class="coords-panel">
    <div class="coord-item">
      <strong>Latitud:</strong>
      <span>{latitud.toFixed(8)}</span>
    </div>
    <div class="coord-item">
      <strong>Longitud:</strong>
      <span>{longitud.toFixed(8)}</span>
    </div>
    {#if precisionGps}
      <span class="badge">Precisión: ±{precisionGps.toFixed(1)}m</span>
    {/if}
  </div>
</div>

<style>
  .map-container {
    width: 100%;
    max-width: 800px;
    margin: 0 auto;
    position: relative;
  }
  .canvas {
    height: 450px;
    width: 100%;
    border-radius: 12px;
    border: 1px solid #ccc;
    box-shadow: 0 4px 6px -1px rgb(0 0 0 / 0.1);
    z-index: 1;
  }
  .controls {
    position: absolute;
    top: 10px;
    right: 10px;
    z-index: 1000;
  }
  .btn-gps {
    background: white;
    border: 2px solid #4338ca;
    color: #4338ca;
    padding: 8px 12px;
    border-radius: 6px;
    font-weight: bold;
    cursor: pointer;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    transition: all 0.2s;
  }
  .btn-gps:hover {
    background: #4338ca;
    color: white;
  }
  .coords-panel {
    margin-top: 1rem;
    padding: 1rem;
    background: #fff;
    border: 1px solid #eee;
    border-radius: 8px;
    display: flex;
    flex-wrap: wrap;
    gap: 1.5rem;
    align-items: center;
    font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas,
      "Liberation Mono", "Courier New", monospace;
    font-size: 0.9rem;
  }
  .coord-item {
    display: flex;
    gap: 0.5rem;
  }
  .badge {
    background: #f0fdf4;
    color: #166534;
    padding: 2px 8px;
    border-radius: 9999px;
    font-size: 0.75rem;
    font-weight: 600;
    border: 1px solid #bbf7d0;
  }
</style>
