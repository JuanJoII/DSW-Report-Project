# Endpoints de autenticación para **Reporta Sabana API**.

---

## 🔐 Guía de Integración: Autenticación
**Base URL:** `http://localhost:5000`

### 1. Registro de Usuario
**Endpoint:** `POST /api/Auth/Register`

Se utiliza para dar de alta nuevos ciudadanos o administradores en el sistema.

* **Cuerpo de la petición (JSON):**
    * `email` (string, requerido): Máximo 100 caracteres.
    * `password` (string, requerido): Contraseña del usuario.
    * `cedula` (string, requerido): Máximo 20 caracteres.
    * `nombreCompleto` (string, requerido): Máximo 100 caracteres.
    * `telefono` (string, opcional): Máximo 15 caracteres.
    * `direccionResidencia` (string, opcional): Máximo 255 caracteres.

---

### 2. Inicio de Sesión (Login)
**Endpoint:** `POST /api/Auth/Login`

Al autenticarse correctamente, el servidor ahora retorna el identificador único del usuario junto con los tokens de acceso.

* **Cuerpo de la petición:**
    * `email` (string, requerido).
    * `password` (string, requerido).
* **Respuesta Exitosa (200 OK):**
    ```json
    {
      "id": "uuid-del-usuario",
      "accessToken": "string",
      "refreshToken": "string"
    }
    ```
    > **Nota para Front:** Es fundamental persistir el `id` (formato UUID) ya que será necesario para el proceso de refresco de tokens.

---

### 3. Renovación de Tokens
**Endpoint:** `POST /api/Auth/RefreshTokens`

Debe llamarse cuando el `accessToken` expire para mantener la sesión activa sin solicitar credenciales de nuevo.

* **Cuerpo de la petición:**
    * `userId` (string, formato UUID): El ID obtenido en el Login.
    * `refreshToken` (string, requerido): El token de refresco actual.
* **Respuesta Exitosa (200 OK):**
    ```json
    {
      "accessToken": "nuevo_string",
      "refreshToken": "nuevo_string"
    }
    ```

---

### 4. Endpoints de Verificación (Middleware Test)
Utiliza estos GETs para probar que los interceptores de Axios/Fetch y los encabezados `Authorization: Bearer <token>` funcionan según el rol:

* **Cualquier usuario autenticado:** `GET /api/Test/Protected`.
* **Solo administradores:** `GET /api/Test/ProtectedAdmin`.

---

## ⚡ Tips para SvelteKit 5
* **Gestión de Tokens:** Dado que estás usando Svelte 5, puedes manejar el estado del token con un **rune** `$state` en un archivo `auth.svelte.js` para que sea reactivo en toda la app.
* **Hooks de Servidor:** Considera validar la sesión en `hooks.server.js` para proteger las rutas del lado del servidor y evitar parpadeos de contenido no autorizado.