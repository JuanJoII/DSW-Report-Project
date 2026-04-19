# 🚀 Entorno de Desarrollo - Fullstack

Este proyecto utiliza Docker Compose con el motor de sincronización en tiempo real para facilitar el desarrollo sin instalar dependencias locales.

## 🛠️ Requisitos
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) o Docker Engine con **Compose V2** (comando sin guion: `docker compose`).

## 🏁 Inicio Rápido
1. Crea tu archivo de configuración local:
```bash
   cp .env.example .env
````

2.  Levanta el entorno con sincronización activa:
```bash
    docker compose up --watch
```

## 🌐 Puertos y Acceso

  - **Frontend (SvelteKit):** [http://localhost:5173](https://www.google.com/search?q=http://localhost:5173)
  - **Backend (.NET 9):** [http://localhost:5000/swagger](https://www.google.com/search?q=http://localhost:5000/swagger)
  - **Base de Datos (SQL Server):** `localhost,1433` (User: `sa`)

## 💻 Cómo trabajar

  - **Sincronización:** Al guardar cualquier archivo en tu editor, los cambios se reflejan automáticamente en los contenedores.
  - **Persistencia:** Los datos de la base de datos se mantienen en el volumen `sqlserver_data` aunque detengas los contenedores.
  - **Detener:** `Ctrl + C` en la terminal o `docker compose down`.
  - **Limpiar Todo:** Para resetear la DB por completo: `docker compose down -v`.
---

