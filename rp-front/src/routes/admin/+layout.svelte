<script>
  let { data, children } = $props();
  let user = $derived(data.user);
  let sidebarOpen = $state(true);
</script>

<div class="admin-container">
  <!-- Sidebar -->
  <aside class="admin-sidebar" class:collapsed={!sidebarOpen}>
    <div class="sidebar-header">
      <h2>Panel</h2>
      <button class="toggle-btn" onclick={() => (sidebarOpen = !sidebarOpen)} title="Toggle sidebar">
        ☰
      </button>
    </div>

    <nav class="sidebar-nav">
      <div class="nav-section">
        <ul>
          <li><a href="/" class="nav-link">Inicio</a></li>
          <li><a href="/admin" class="nav-link">Dashboard</a></li>
        </ul>
      </div>
    </nav>

    <div class="sidebar-footer">
      <div class="admin-info">
        <span class="admin-email">{user?.email}</span>
        <span class="admin-role">Admin</span>
      </div>
      <a href="/logout" data-sveltekit-reload class="btn-logout">Cerrar sesión</a>
    </div>
  </aside>

  <!-- Main Content -->
  <main class="admin-main">
    <div class="admin-header">
      <h1>Panel de Administración</h1>
    </div>
    <div class="admin-content">
      {@render children()}
    </div>
  </main>
</div>

<style>
  .admin-container {
    display: flex;
    min-height: 100vh;
    background-color: var(--bg-color);
  }

  .admin-sidebar {
    width: 250px;
    background: #fff;
    border-right: 1px solid var(--border-color);
    display: flex;
    flex-direction: column;
    padding: 1.5rem 1rem;
    transition: width 0.3s ease;
    position: sticky;
    top: 0;
    height: 100vh;
    overflow-y: auto;
  }

  .admin-sidebar.collapsed {
    width: 80px;
  }

  .sidebar-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 2rem;
    padding-bottom: 1rem;
    border-bottom: 2px solid var(--primary-color);
  }

  .sidebar-header h2 {
    margin: 0;
    font-size: 1.2rem;
    color: var(--primary-color);
    white-space: nowrap;
    overflow: hidden;
  }

  .admin-sidebar.collapsed .sidebar-header h2 {
    display: none;
  }

  .toggle-btn {
    background: none;
    border: none;
    font-size: 1.5rem;
    cursor: pointer;
    color: var(--text-color);
    padding: 0.25rem 0.5rem;
    transition: color 0.2s;
  }

  .toggle-btn:hover {
    color: var(--primary-color);
  }

  .sidebar-nav {
    flex: 1;
  }

  .nav-section h3 {
    margin: 1.5rem 0 0.75rem 0;
    font-size: 0.75rem;
    text-transform: uppercase;
    color: var(--secondary-color);
    letter-spacing: 0.05em;
    font-weight: 600;
  }

  .admin-sidebar.collapsed .nav-section h3 {
    display: none;
  }

  .nav-section ul {
    list-style: none;
    margin: 0;
    padding: 0;
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
  }

  .nav-link {
    display: block;
    padding: 0.75rem 1rem;
    color: var(--text-color);
    text-decoration: none;
    border-radius: 6px;
    transition: all 0.2s;
    font-weight: 500;
    font-size: 0.9rem;
  }

  .nav-link:hover {
    background: #f0f0f5;
    color: var(--primary-color);
  }

  .admin-sidebar.collapsed .nav-link {
    padding: 0.75rem 0.5rem;
    font-size: 1.2rem;
    text-align: center;
  }

  .sidebar-footer {
    padding-top: 1rem;
    border-top: 1px solid var(--border-color);
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }

  .admin-info {
    display: flex;
    flex-direction: column;
    gap: 0.25rem;
    font-size: 0.85rem;
  }

  .admin-sidebar.collapsed .admin-info {
    display: none;
  }

  .admin-email {
    color: var(--text-color);
    font-weight: 600;
    word-break: break-word;
  }

  .admin-role {
    color: var(--secondary-color);
    font-size: 0.75rem;
  }

  .btn-logout {
    padding: 0.6rem 1rem;
    background: var(--error-color);
    color: white;
    border: none;
    border-radius: 6px;
    text-decoration: none;
    font-size: 0.85rem;
    font-weight: 600;
    cursor: pointer;
    transition: background 0.2s;
    text-align: center;
  }

  .btn-logout:hover {
    background: #dc2626;
  }

  .admin-main {
    flex: 1;
    display: flex;
    flex-direction: column;
    overflow-y: auto;
  }

  .admin-header {
    background: #fff;
    border-bottom: 1px solid var(--border-color);
    padding: 1.5rem;
    sticky: top;
  }

  .admin-header h1 {
    margin: 0;
    font-size: 1.75rem;
    color: var(--text-color);
  }

  .admin-content {
    padding: 2rem 1.5rem;
    flex: 1;
  }

  @media (max-width: 768px) {
    .admin-sidebar {
      width: 70px;
      padding: 1rem 0.5rem;
    }

    .admin-sidebar.collapsed {
      width: 70px;
    }

    .sidebar-header h2,
    .nav-section h3,
    .admin-info {
      display: none;
    }

    .admin-content {
      padding: 1.5rem 1rem;
    }
  }
</style>
