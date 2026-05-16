<script>
  let { form } = $props();
</script>

<div class="auth-page">
  <div class="auth-card">
    <h2>Iniciar sesión</h2>
    <form method="POST" class="auth-form">
      <div class="form-group">
        <label for="email">Tu email</label>
        <input type="email" name="email" id="email" placeholder="name@company.com" required>
      </div>
      
      <div class="form-group">
        <label for="password">Contraseña</label>
        <input type="password" name="password" id="password" placeholder="••••••••" required>
      </div>

      {#if form?.incorrect}
        <p class="error-msg">Credenciales incorrectas. Intenta de nuevo.</p>
      {/if}

      {#if form?.missing}
        <p class="error-msg">Por favor completa todos los campos.</p>
      {/if}

      <button type="submit" class="btn-submit">Entrar</button>
      
      <p class="auth-footer">
        ¿Aún no tienes cuenta? <a href="/register">Regístrate aquí</a>
      </p>
    </form>
  </div>
</div>

<style>
  .auth-page {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 70vh;
  }
  .auth-card {
    background: var(--color-bg-primary);
    padding: 2.5rem;
    border-radius: 12px;
    box-shadow: 0 4px 6px -1px var(--color-card-shadow);
    width: 100%;
    max-width: 400px;
    border: 1px solid var(--border-color);
  }
  h2 {
    margin-top: 0;
    margin-bottom: 1.5rem;
    font-size: 1.5rem;
    color: var(--text-color);
  }
  .auth-form {
    display: flex;
    flex-direction: column;
    gap: 1.25rem;
  }
  .form-group {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
  }
  label {
    font-size: 0.875rem;
    font-weight: 500;
    color: var(--text-color);
  }
  input {
    padding: 0.625rem;
    border-radius: 8px;
    border: 1px solid var(--border-color);
    font-size: 0.9rem;
    background: var(--color-bg-secondary);
    color: var(--text-color);
  }
  input:focus {
    outline: none;
    border-color: var(--primary-color);
    box-shadow: 0 0 0 3px rgba(67, 56, 202, 0.1);
  }
  .btn-submit {
    background: var(--primary-color);
    color: white;
    border: none;
    padding: 0.75rem;
    border-radius: 8px;
    font-weight: 600;
    cursor: pointer;
    transition: background 0.2s;
    margin-top: 0.5rem;
  }
  .btn-submit:hover {
    background: var(--primary-hover);
  }
  .error-msg {
    color: var(--error-color);
    font-size: 0.875rem;
    margin: 0;
    font-weight: 500;
  }
  .auth-footer {
    text-align: center;
    font-size: 0.875rem;
    color: var(--secondary-color);
    margin-top: 1rem;
  }
  .auth-footer a {
    color: var(--primary-color);
    text-decoration: none;
    font-weight: 600;
  }
  .auth-footer a:hover {
    text-decoration: underline;
  }
</style>
