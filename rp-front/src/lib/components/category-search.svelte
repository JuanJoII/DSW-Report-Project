<script lang="ts">
  let { categorias, value = $bindable(""), selectedId = $bindable(null) } = $props<{
    categorias: Array<{id: number, nombre: string}>;
    value: string;
    selectedId: number | null;
  }>();

  let query = $state("");
  let showResults = $state(false);
  let filtered = $derived(
    categorias.filter(c => 
      c.nombre.toLowerCase().includes(query.toLowerCase())
    )
  );

  let exactMatch = $derived(
    categorias.find(c => c.nombre.toLowerCase() === query.toLowerCase())
  );

  function selectCategory(cat: {id: number, nombre: string}) {
    selectedId = cat.id;
    value = cat.nombre;
    query = cat.nombre;
    showResults = false;
  }

  function addNewCategory() {
    selectedId = -1; // Flag for new category
    value = query;
    showResults = false;
  }

  function handleInput(e: Event) {
    const target = e.target as HTMLInputElement;
    query = target.value;
    value = query;
    selectedId = null;
    showResults = true;
  }
</script>

<div class="category-search">
  <input
    type="text"
    placeholder="Escribe o busca una categoría..."
    value={query}
    oninput={handleInput}
    onfocus={() => showResults = true}
    class="search-input"
    autocomplete="off"
  />
  <input type="hidden" name="categoriaId" value={selectedId} />
  <input type="hidden" name="nuevaCategoriaNombre" value={selectedId === -1 ? query : ""} />

  {#if showResults && query.length > 0}
    <ul class="results-list">
      {#each filtered as cat}
        <li>
          <button type="button" onclick={() => selectCategory(cat)}>
            {cat.nombre}
          </button>
        </li>
      {/each}

      {#if !exactMatch}
        <li class="add-new">
          <button type="button" onclick={addNewCategory}>
            <span class="plus">+</span> Añadir "{query}"
          </button>
        </li>
      {/if}
    </ul>
  {/if}
</div>

<style>
  .category-search {
    position: relative;
    width: 100%;
  }

  .search-input {
    width: 100%;
    padding: 12px 16px;
    border-radius: 8px;
    border: 1px solid #ddd;
    font-size: 1rem;
    transition: all 0.2s;
  }

  .search-input:focus {
    outline: none;
    border-color: var(--primary-color);
    box-shadow: 0 0 0 3px rgba(67, 56, 202, 0.1);
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
    max-height: 250px;
    overflow-y: auto;
  }

  .results-list li {
    border-bottom: 1px solid #f5f5f5;
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
    color: var(--primary-color);
  }

  .add-new button {
    color: var(--primary-color);
    font-weight: 600;
  }

  .plus {
    font-size: 1.2rem;
    margin-right: 0.5rem;
  }
</style>
