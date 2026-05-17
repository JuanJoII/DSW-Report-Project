<script lang="ts">
  import { onMount, onDestroy } from "svelte";
  import { browser } from "$app/environment";
  import "leaflet/dist/leaflet.css";

  let { latitud, longitud } = $props();

  let mapElement: HTMLElement | undefined = $state();
  let map: any = $state();
  let marker: any = $state();
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

      // Inicializar mapa sin controles de zoom (estático)
      map = L.map(mapElement, {
        zoomControl: false,
        scrollWheelZoom: false,
        doubleClickZoom: false,
        touchZoom: false,
        dragging: false
      }).setView([latitud, longitud], 16);

      L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        attribution: "&copy; OpenStreetMap",
      }).addTo(map);

      // Crear el marcador (no arrastrable)
      marker = L.marker([latitud, longitud], { draggable: false }).addTo(map);

      // Forzar recalculo de tamaño
      setTimeout(() => map.invalidateSize(), 400);
    }
  });

  onDestroy(() => {
    if (map) map.remove();
  });
</script>

<div class="map-static-container">
  <div bind:this={mapElement} class="canvas"></div>
</div>

<style>
  .map-static-container {
    width: 100%;
    height: 100%;
  }
  .canvas {
    height: 100%;
    width: 100%;
    z-index: 1;
  }
  :global(.dark) .canvas {
    filter: grayscale(0.6) invert(0.9) hue-rotate(180deg) brightness(0.9);
  }
</style>
