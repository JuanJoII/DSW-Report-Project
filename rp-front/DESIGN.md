## Visión General: Reporta Sabana

Reporta Sabana es una plataforma de participación ciudadana diseñada para cerrar la brecha entre los habitantes de la Sabana y la administración municipal. La interfaz busca equilibrar la **calidez cívica** con la **precisión técnica**, tomando prestada la estructura limpia, simétrica y densa en información característica de plataformas de documentación modernas (estética Mintlify), pero adaptada a un contexto gubernamental.

### El Concepto "Atardecer en la Sabana"
El diseño se inspira en los colores del cielo sobre los cerros orientales al atardecer, evocando un sentido de cuidado comunitario y balance natural. Las transiciones suaves de color (gradientes atmosféricos de violeta a púrpura cálido) transmiten calma en la vista ciudadana, mientras que el panel de control gubernamental prioriza la alta densidad de datos y la organización simétrica.

---

## Identidad Visual y Colores

La geometría del sitio favorece fuertemente la circularidad y la simetría, utilizando formas orgánicas para suavizar la interacción.

### Paleta Primaria: "Marca Sabana"
- **Violeta Institucional** (`#5B21B6` | `bg-violet-700`): Color de mando. Usado en botones de acción principal (CTA), marcadores del mapa y estados activos.
- **Púrpura Atardecer** (`#4C1D95` | `bg-violet-900`): Profundidad y contraste. Usado en cabeceras densas y fondos principales del panel gubernamental (modo oscuro).
- **Lila Brumoso** (`#EDE9FE` | `bg-violet-100`): Color de descanso. Usado en fondos de tarjetas, zonas de carga de fotos y resaltados sutiles en la interfaz.
- **Naranja Cívico** (`#EA580C` | `bg-orange-600`): Urgencia. Para avisos de la alcaldía, notificaciones del sistema o reportes de orden público marcados como críticos.

### Semántica de Gestión (Estado de Reportes)
Cada fase del reporte tiene una identidad cromática estricta para una lectura rápida en el dashboard:
- **Pendiente:** `#06B6D4` (Cian) - "Recibido y a la espera".
- **En Revisión:** `#F59E0B` (Ámbar) - "Estamos trabajando en ello".
- **Resuelto:** `#10B981` (Esmeralda) - "Misión cumplida" (vía o parque reparado).
- **Rechazado/Error:** `#EF4444` (Rojo) - "No procede / Duplicado" o campos obligatorios faltantes.

---

## Tipografía

El sistema utiliza una dualidad estratégica para separar la prosa cívica de los datos técnicos.

### Jost (Títulos y Voz Institucional)
Una fuente sin serifa geométrica y moderna. Se utiliza para:
- Títulos de páginas (Hero) y mensajes ciudadanos ("Tu voz mejora la ciudad").
- Nombres de categorías en las tarjetas de reportes.
- Cabeceras de sección.
*Aporta una voz de autoridad cívica, modernidad y claridad.*

### Onest (Interfaz, Datos y Cuerpos)
Una Sans-Serif moderna, muy limpia y optimizada para pantallas. Se utiliza para:
- Cuerpo descriptivo de los reportes (con un `line-height` amplio para máxima legibilidad).
- Coordenadas GPS, marcas de tiempo y metadatos técnicos.
- Botones, etiquetas de estado (badges) y menús de navegación.

---

## Layout y Estructura (Grid System)

### 1. Vista Ciudadana (Móvil Primero)
- **Contenedor Principal:** Max-width adaptativo centrado (ej. `max-w-3xl`). Optimizado para uso en la calle, con una sola mano.
- **Flujo Lineal y Simétrico:** Mapa (arriba) → Formulario de Descripción → Área de Fotos (abajo).
- **Interacciones:** Botones primarios con `rounded-full` (píldora) para evitar fatiga visual y mantener el balance natural.

### 2. Vista Gubernamental (3-Column Dashboard)
Para pantallas grandes (> 1280px), la interfaz adopta una densidad estilo "Mintlify", dividida en:
- **Columna 1 (Izquierda):** Navegación lateral compacta, avatares circulares e indicadores de estado rápidos.
- **Columna 2 (Centro):** Feed de reportes (Lista vertical infinita). Tarjetas simétricas con filtros en la parte superior.
- **Columna 3 (Derecha):** Detalle del reporte expandido. Incluye el mapa de ubicación exacta, galería de evidencia fotográfica (cuadrícula perfecta) y línea de tiempo de la gestión.

---

## Componentes Específicos

### `card-reporte-feed`
- **Simetría:** Foto miniatura 1:1 a la izquierda con bordes suaves (`rounded-xl`).
- **Contenido:** Título del daño en Alegreya, descripción y ubicación en Onest con un icono pequeño de pin.
- **Estado:** Badge circular (`rounded-full`) en la esquina superior derecha indicando la semántica de gestión.

### `dropzone-foto` (Captura de Evidencia)
- Área de carga interactiva con borde punteado en *Lila Brumoso*.
- Icono de cámara central rodeado por un círculo perfecto para mantener el tema orgánico.
- Al cargar la evidencia, muestra las miniaturas en una cuadrícula simétrica (`aspect-square`), optimizadas para la posterior subida a los buckets de almacenamiento.

---

## Estrategia Técnica: SvelteKit, Tailwind CSS y Mapas

Para garantizar que la estética estructurada no interfiera con las librerías técnicas subyacentes, se aplican las siguientes reglas:

### 1. Integración de Leaflet
Debemos proteger la integridad del componente de Mapa para que Tailwind no rompa sus estilos base:
- **Protección de Tiles:** Tailwind añade `img { max-width: 100% }` globalmente. Se debe incluir un reset en el archivo CSS global:
  ```css
  .leaflet-container img {
    max-width: none !important;
    display: inline !important;
  }