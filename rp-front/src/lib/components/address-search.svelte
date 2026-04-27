<script lang="ts">
  import { untrack } from "svelte";

  let { onSelect } = $props<{
    onSelect: (lat: number, lng: number, displayName: string) => void;
  }>();

  let query = $state("");
  let results = $state<any[]>([]);
  let isSearching = $state(false);
  let showResults = $state(false);
  let debounceTimer: ReturnType<typeof setTimeout>;

  async function searchAddresses(searchQuery: string) {
    if (searchQuery.length < 3) {
      results = [];
      return;
    }

    isSearching = true;
    try {
      const response = await fetch(
        `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(searchQuery)}&limit=5`
      );
      if (response.ok) {
        results = await response.json();
      }
    } catch (error) {
      console.error("Error searching addresses:", error);
    } finally {
      isSearching = false;
    }
  }

  function handleInput(e: Event) {
    const value = (e.target as HTMLInputElement).value;
    query = value;
    
    clearTimeout(debounceTimer);
    debounceTimer = setTimeout(() => {
      searchAddresses(query);
    }, 500);
  }

  function selectAddress(result: any) {
    const lat = parseFloat(result.lat);
    const lon = parseFloat(result.lon);
    onSelect(lat, lon, result.display_name);
    query = result.display_name;
    showResults = false;
  }
</script>

<div class="search-container">
  <div class="input-wrapper">
    <input
      type="text"
      placeholder="Buscar dirección..."
      value={query}
      oninput={handleInput}
      onfocus={() => showResults = true}
      class="search-input"
    />
    {#if isSearching}
      <div class="loader"></div>
    {/if}
  </div>

  {#if showResults && results.length > 0}
    <ul class="results-list">
      {#each results as result}
        <li>
          <button type="button" onclick={() => selectAddress(result)}>
            {result.display_name}
          </button>
        </li>
      {/each}
    </ul>
  {/if}
</div>

<style>
  .search-container {
    position: relative;
    width: 100%;
    max-width: 800px;
    margin: 0 auto 1rem auto;
  }

  .input-wrapper {
    position: relative;
    display: flex;
    align-items: center;
  }

  .search-input {
    width: 100%;
    padding: 12px 16px;
    border-radius: 8px;
    border: 1px solid #ccc;
    font-size: 1rem;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
    transition: border-color 0.2s;
  }

  .search-input:focus {
    outline: none;
    border-color: #4338ca;
    box-shadow: 0 0 0 3px rgba(67, 56, 202, 0.1);
  }

  .loader {
    position: absolute;
    right: 12px;
    width: 20px;
    height: 20px;
    border: 2px solid #f3f3f3;
    border-top: 2px solid #4338ca;
    border-radius: 50%;
    animation: spin 1s linear infinite;
  }

  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }

  .results-list {
    position: absolute;
    top: 100%;
    left: 0;
    right: 0;
    background: white;
    border: 1px solid #eee;
    border-radius: 8px;
    margin-top: 4px;
    padding: 0;
    list-style: none;
    box-shadow: 0 4px 6px -1px rgb(0 0 0 / 0.1);
    z-index: 2000;
    max-height: 300px;
    overflow-y: auto;
  }

  .results-list li {
    border-bottom: 1px solid #f5f5f5;
  }

  .results-list li:last-child {
    border-bottom: none;
  }

  .results-list button {
    width: 100%;
    text-align: left;
    padding: 10px 16px;
    background: none;
    border: none;
    font-size: 0.9rem;
    cursor: pointer;
    transition: background 0.2s;
  }

  .results-list button:hover {
    background: #f3f4f6;
    color: #4338ca;
  }
</style>
