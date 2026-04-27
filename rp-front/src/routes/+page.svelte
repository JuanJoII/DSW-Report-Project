<script lang="ts">
  import { Map, AddressSearch } from "$lib/index";

  let lat = $state(4.5709);
  let lng = $state(-74.2973);

  function handleAddressSelect(newLat: number, newLng: number) {
    lat = newLat;
    lng = newLng;
  }
</script>

<main>
  <h1>Geolocalización Svelte 5</h1>
  <p>
    Busca una dirección o utiliza tu ubicación actual para centrar el mapa.
  </p>

  <AddressSearch onSelect={handleAddressSelect} />

  <Map bind:latitud={lat} bind:longitud={lng} />

  <section class="instructions">
    <h2>Instrucciones</h2>
    <ul>
      <li>
        El marcador es <strong>arrastrable</strong> para ajustar la ubicación exacta.
      </li>
      <li>
        Las coordenadas se actualizan automáticamente al mover el marcador.
      </li>
      <li>Se incluye un panel con la precisión del GPS.</li>
    </ul>
  </section>
</main>

<style>
  :global(body) {
    font-family:
      system-ui,
      -apple-system,
      sans-serif;
    background-color: #f4f4f9;
    color: #333;
    line-height: 1.5;
    padding: 2rem;
  }
  main {
    max-width: 800px;
    margin: 0 auto;
  }
  h1 {
    color: #1a1a1a;
    margin-bottom: 0.5rem;
  }
  .instructions {
    margin-top: 2rem;
    padding: 1.5rem;
    background: #fff;
    border-radius: 8px;
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  }
</style>
