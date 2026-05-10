# Fotoreportes - Integracion con Cloudflare R2

## Flujo general

El proceso para asociar fotos a un reporte se divide en 3 pasos:

1. **Solicitar URL firmada** — El cliente pide al backend una URL firmada para subir la imagen.
2. **Subir la imagen directamente a R2** — Usando la URL firmada, el cliente sube el archivo binario de la imagen.
3. **Registrar la foto** — El cliente envia al backend la URL publica y el ID del reporte para persistir el registro.

---

## 1. Obtener URL firmada para subir una imagen

Solicita una URL firmada de carga al backend. Esta URL tiene un tiempo de expiracion de **15 minutos**.

### Peticion

```
GET /api/FotoReporte/presignedUrl?fileName=<nombre_archivo>
```

**Parametros de consulta:**

| Campo    | Tipo   | Requerido | Descripcion                          |
|----------|--------|-----------|--------------------------------------|
| fileName | string | Si        | Nombre del archivo, incluyendo extension (ej: `foto-001.jpg`) |

**Ejemplo con curl:**

```bash
curl -X GET "https://tu-api.com/api/FotoReporte/presignedUrl?fileName=reporte-42-foto-1.jpg"
```

### Respuesta exitosa (200 OK)

```json
{
  "uploadUrl": "https://<account-id>.r2.cloudflarestorage.com/reporte_fotos/<guid>-reporte-42-foto-1.jpg?X-Amz-...",
  "publicUrl": "https://<public-domain>/reporte_fotos/<guid>-reporte-42-foto-1.jpg"
}
```

| Campo      | Tipo   | Descripcion                                                              |
|------------|--------|--------------------------------------------------------------------------|
| uploadUrl  | string | URL PUT firmada para subir la imagen directamente a R2. Expira en 15 min. |
| publicUrl  | string | URL publica de la imagen una vez subida correctamente.                    |

---

## 2. Subir la imagen a R2

Usando la `uploadUrl` obtenida en el paso anterior, el cliente sube el archivo binario con el metodo PUT.

### Peticion

```
PUT <uploadUrl>
Content-Type: image/<formato>
```

**Encabezados requeridos:**

| Encabezado     | Valor                         |
|----------------|-------------------------------|
| Content-Type   | `image/jpeg`, `image/png`, `image/webp`, etc. |

**Body:** el archivo binario de la imagen.

**Ejemplo con fetch (JavaScript):**

```javascript
async function uploadImage(file, uploadUrl) {
  const response = await fetch(uploadUrl, {
    method: 'PUT',
    headers: {
      'Content-Type': file.type,
    },
    body: file,
  });

  if (!response.ok) {
    throw new Error(`Error al subir imagen: ${response.status}`);
  }
}
```

> **Importante:** La URL firmada expira en 15 minutos. Si el usuario tarda mas, deberas solicitar una nueva URL firmada.

### Respuestas posibles

| Codigo | Significado                                          |
|--------|------------------------------------------------------|
| 200    | Imagen subida correctamente.                         |
| 403    | URL firmada invalida o expirada. Solicitar una nueva. |

---

## 3. Registrar la foto asociada al reporte

Una vez subida la imagen, envia la URL publica al backend para guardarla en la base de datos.

### Peticion

```
POST /api/FotoReporte
Content-Type: application/json
```

**Body (JSON):**

```json
{
  "reporteId": 42,
  "url": "https://<public-domain>/reporte_fotos/<guid>-reporte-42-foto-1.jpg"
}
```

| Campo     | Tipo   | Requerido | Descripcion                                    |
|-----------|--------|-----------|------------------------------------------------|
| reporteId | int    | Si        | ID del reporte al que pertenece la foto.       |
| url       | string | Si        | URL publica de la imagen (validada como URL).  |

**Ejemplo con curl:**

```bash
curl -X POST "https://tu-api.com/api/FotoReporte" \
  -H "Content-Type: application/json" \
  -d '{"reporteId": 42, "url": "https://cdn.example.com/reporte_fotos/abc123-reporte-42-foto-1.jpg"}'
```

### Respuestas posibles

| Codigo | Descripcion                                              |
|--------|----------------------------------------------------------|
| 201    | Foto registrada exitosamente.                             |
| 400    | Datos invalidos (url mal formateada, reporteId faltante).|
| 404    | El reporte con ese ID no existe.                         |

---

## 4. Obtener fotos de un reporte

Para recuperar las fotos asociadas a un reporte.

### Peticion

```
GET /api/FotoReporte/{reporteId}
```

### Respuesta exitosa (200 OK)

```json
[
  {
    "id": 1,
    "url": "https://cdn.example.com/reporte_fotos/abc123-reporte-42-foto-1.jpg"
  },
  {
    "id": 2,
    "url": "https://cdn.example.com/reporte_fotos/def456-reporte-42-foto-2.jpg"
  }
]
```

---

## Ejemplo completo en JavaScript

```javascript
async function crearFotoreporte(file, reporteId) {
  // Paso 1: Obtener URL firmada
  const presignedRes = await fetch(
    `/api/FotoReporte/presignedUrl?fileName=${file.name}`
  );
  if (!presignedRes.ok) throw new Error('No se pudo obtener URL firmada');
  const { uploadUrl, publicUrl } = await presignedRes.json();

  // Paso 2: Subir imagen directamente a R2
  const uploadRes = await fetch(uploadUrl, {
    method: 'PUT',
    headers: { 'Content-Type': file.type },
    body: file,
  });
  if (!uploadRes.ok) throw new Error('Error al subir imagen');

  // Paso 3: Registrar foto en el backend
  const registerRes = await fetch('/api/FotoReporte', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ reporteId, url: publicUrl }),
  });
  if (!registerRes.ok) throw new Error('Error al registrar foto');

  return publicUrl;
}
```

---

## Consideraciones

- **Extensiones permitidas:** cualquier extension valida de imagen. Se recomienda validar en el frontend antes de solicitar la URL firmada.
- **Tiempo de expiracion:** la URL firmada expira en **15 minutos**. Si el usuario necesita mas tiempo, volver a solicitar una nueva URL.
- **Formato de almacenamiento en R2:** los archivos se guardan en la carpeta `reporte_fotos/` con un prefijo de GUID para garantizar nombres unicos.
- **Validacion del reporte:** antes de registrar la foto, el backend verifica que el `reporteId` corresponda a un reporte existente en la base de datos.