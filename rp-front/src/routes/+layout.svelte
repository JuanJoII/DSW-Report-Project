<script>
  import favicon from "$lib/assets/favicon.svg";
  import "../app.css";

  let { data, children } = $props();
  let user = $derived(data.user);
  let isAdmin = $derived(user?.role?.toLowerCase() === 'admin');
  let isAdminRoute = $derived(data.isAdminRoute);
</script>

<svelte:head>
  <link rel="icon" href={favicon} />
</svelte:head>

{#if isAdminRoute && isAdmin}
  <!-- Layout para Admin -->
  {@render children()}
{:else}
  <!-- Layout para Ciudadanos -->
  <nav class="navbar">
    <div class="nav-container">
      <a href="/" class="logo">Reporta Sabana</a>

      <div class="nav-links">
        <ul class="main-menu">
          <li><a href="/">Inicio</a></li>
          {#if user}
            <li><a href="/reportes/nuevo">Crear Reporte</a></li>
            <li><a href="/reportes/mis-reportes">Mis Reportes</a></li>
            {#if isAdmin}
              <li><a href="/admin" class="admin-link">📊 Panel Admin</a></li>
            {/if}
          {/if}
        </ul>

        <div class="auth-section">
          {#if user}
            <div class="user-info">
              <span class="welcome-text">Hola, <strong>{user.email}</strong></span>
              <span class="role-badge">{user.role}</span>
              <a href="/logout" data-sveltekit-reload class="btn btn-logout">Cerrar sesión</a>
            </div>
          {:else}
            <div class="auth-btns">
              <a href="/login" class="btn btn-secondary">Entrar</a>
              <a href="/register" class="btn btn-primary">Registrarse</a>
            </div>
          {/if}
        </div>
      </div>
    </div>
  </nav>

  <main class="content">
    {@render children()}
  </main>
{/if}

<style>
  .navbar {
    background: #fff;
    border-bottom: 1px solid var(--border-color);
    padding: 0.75rem 0;
    position: sticky;
    top: 0;
    z-index: 100;
  }
  .nav-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 1rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
  .logo {
    font-size: 1.5rem;
    font-weight: bold;
    color: var(--primary-color);
    text-decoration: none;
  }
  .nav-links {
    display: flex;
    align-items: center;
    gap: 2rem;
  }
  .main-menu {
    list-style: none;
    display: flex;
    margin: 0;
    padding: 0;
    gap: 1.5rem;
  }
  .main-menu a {
    text-decoration: none;
    color: var(--text-color);
    font-weight: 500;
  }
  .main-menu a:hover {
    color: var(--primary-color);
  }
  .main-menu a.admin-link {
    color: var(--primary-color);
    font-weight: 600;
  }
  .main-menu a.admin-link:hover {
    color: var(--primary-hover);
  }
  .user-info {
    display: flex;
    align-items: center;
    gap: 1rem;
  }
  .role-badge {
    background: #e0e7ff;
    color: var(--primary-color);
    padding: 0.2rem 0.6rem;
    border-radius: 999px;
    font-size: 0.75rem;
    font-weight: 600;
    text-transform: uppercase;
  }
  .btn {
    padding: 0.5rem 1rem;
    border-radius: 6px;
    text-decoration: none;
    font-size: 0.9rem;
    font-weight: 600;
    cursor: pointer;
    border: 1px solid transparent;
    transition: all 0.2s;
  }
  .btn-primary {
    background: var(--primary-color);
    color: white;
  }
  .btn-primary:hover {
    background: var(--primary-hover);
  }
  .btn-secondary {
    color: var(--text-color);
    border-color: var(--border-color);
  }
  .btn-secondary:hover {
    background: #f9fafb;
  }
  .btn-logout {
    background: var(--error-color);
    color: white;
  }
  .btn-admin {
    background: var(--primary-color);
    color: white;
  }
  .btn-admin:hover {
    background: var(--primary-hover);
  }
  .content {
    max-width: 1200px;
    margin: 2rem auto;
    padding: 0 1rem;
  }
</style>
