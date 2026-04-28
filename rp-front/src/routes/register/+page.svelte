<script>
  let { form } = $props();
</script>

<div class="auth-page">
  <div class="auth-card">
    <h2>Crear una cuenta</h2>
    <form method="POST" class="auth-form">
      <div class="form-group">
        <label for="nombreCompleto">Nombre Completo</label>
        <input type="text" name="nombreCompleto" id="nombreCompleto" placeholder="Juan Pérez" required>
      </div>

      <div class="form-group">
        <label for="cedula">Cédula</label>
        <input type="text" name="cedula" id="cedula" placeholder="12345678" required>
      </div>

      <div class="form-group">
        <label for="email">Tu email</label>
        <input type="email" name="email" id="email" placeholder="name@company.com" required>
      </div>
      
      <div class="form-group">
        <label for="password">Contraseña</label>
        <input type="password" name="password" id="password" placeholder="••••••••" required>
      </div>

      <div class="form-group">
        <label for="telefono">Teléfono (Opcional)</label>
        <input type="text" name="telefono" id="telefono" placeholder="3001234567">
      </div>

      <div class="form-group">
        <label for="direccionResidencia">Dirección (Opcional)</label>
        <input type="text" name="direccionResidencia" id="direccionResidencia" placeholder="Calle 123 #45-67">
      </div>

      {#if form?.missing}
        <p class="error-msg">Por favor completa los campos obligatorios.</p>
      {/if}

      {#if form?.message}
        <p class="error-msg">{form.message}</p>
      {/if}

      <button type="submit" class="btn-submit">Registrarse</button>
      
      <p class="auth-footer">
        ¿Ya tienes cuenta? <a href="/login">Inicia sesión aquí</a>
      </p>
    </form>
  </div>
</div>

<style>
  .auth-page {
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 2rem 0;
  }
  .auth-card {
    background: white;
    padding: 2.5rem;
    border-radius: 12px;
    box-shadow: 0 4px 6px -1px rgba(0,0,0,0.1);
    width: 100%;
    max-width: 450px;
    border: 1px solid var(--border-color);
  }
  h2 {
    margin-top: 0;
    margin-bottom: 1.5rem;
    font-size: 1.5rem;
    color: #111;
  }
  .auth-form {
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }
  .form-group {
    display: flex;
    flex-direction: column;
    gap: 0.4rem;
  }
  label {
    font-size: 0.875rem;
    font-weight: 500;
  }
  input {
    padding: 0.625rem;
    border-radius: 8px;
    border: 1px solid var(--border-color);
    font-size: 0.9rem;
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
    margin-top: 1rem;
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
