# RRHH — Proyecto de práctica con ASP.NET Core MVC

Aplicación web para gestionar recursos humanos, construida como **proyecto de estudio** para
practicar ASP.NET Core MVC, Entity Framework Core y una arquitectura por capas sencilla.
No está pensada para producción; el objetivo es aprender haciendo.

## Stack

| Tecnología | Versión | Uso |
|---|---|---|
| .NET / ASP.NET Core MVC | 10.0 | Framework web (controladores + vistas Razor) |
| Entity Framework Core | 10.0 | ORM para el acceso a datos |
| Npgsql | 10.0 | Proveedor de EF Core para PostgreSQL |
| PostgreSQL | — | Base de datos (esquema ya existente, enfoque *database-first*) |
| Bootstrap | 5 | Estilos del layout por defecto |

## Qué se practica aquí

- **Arquitectura por capas**: Controlador → Servicio → Repositorio → `DbContext`.
- **Inyección de dependencias** con interfaces (`IDepartmentService`, `IDepartmentRepository`).
- **ViewModels** separados de las entidades, para que las vistas no dependan del modelo de datos.
- **Mapeo explícito de EF Core** con `IEntityTypeConfiguration<T>` (nombres de tablas y columnas
  en `snake_case`, precisión de decimales, índices únicos, valores por defecto de la base).
- **Borrado lógico** (`is_active`) en lugar de eliminar registros.
- **Validaciones** con Data Annotations en los ViewModels y validaciones de negocio
  (código y nombre únicos) en el servicio.
- Uso de `IEnumerable<T>` en los contratos y `IActionResult` en las acciones del controlador.

## Estructura del proyecto

```
src/RRHH.Web/
├── Controllers/        Acciones MVC (reciben la petición, devuelven vistas o redirecciones)
├── Services/           Lógica de negocio; trabaja con ViewModels
├── Repositories/       Acceso a datos; trabaja con entidades y el DbContext
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Configurations/ Mapeo de cada entidad (Fluent API)
├── Models/             Entidades de persistencia (Department, Employee)
├── ViewModels/
│   └── Departments/    ViewModels por módulo (listado, detalle, formulario)
├── Views/              Vistas Razor
└── wwwroot/            Archivos estáticos
```

### Flujo de una petición

```
Navegador → DepartmentsController → IDepartmentService → IDepartmentRepository → ApplicationDbContext → PostgreSQL
                                    (ViewModels)           (Entidades)
```

## Módulos

- **Departamentos** (completo): listado con filtro de activos, detalle, crear, editar,
  desactivar y reactivar.
- **Empleados**: pendiente.

## Requisitos

- [SDK de .NET 10](https://dotnet.microsoft.com/download)
- Una instancia de PostgreSQL con la tabla `departments` creada (ver abajo)

## Puesta en marcha

1. Clonar el repositorio.

2. Configurar la cadena de conexión en `src/RRHH.Web/appsettings.Development.json`
   (es un entorno local de práctica, por eso va directamente en el archivo):

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=rrhh;Username=postgres;Password=tu_password"
     }
   }
   ```

3. Crear la tabla en PostgreSQL (el proyecto no usa migraciones; el esquema ya existe):

   ```sql
   CREATE TABLE departments (
       id          uuid PRIMARY KEY DEFAULT gen_random_uuid(),
       code        varchar(10)  NOT NULL UNIQUE,
       name        varchar(60)  NOT NULL UNIQUE,
       description text,
       location    varchar(80),
       budget      numeric(14,2),
       phone       varchar(20),
       email       varchar(120),
       is_active   boolean      NOT NULL DEFAULT true,
       created_at  timestamptz  NOT NULL DEFAULT now(),
       updated_at  timestamptz
   );
   ```

4. Ejecutar:

   ```bash
   dotnet run --project src/RRHH.Web
   ```

   Abrir la URL que muestra la consola y navegar a `/Departments`.

## Convenciones

- Commits en español con prefijo tipo *conventional commits* (`feat:`, `refactor:`, `fix:`).
- Código en inglés (clases, propiedades), textos de la interfaz y comentarios en español.
- Nombres de tablas y columnas en `snake_case`; se mapean explícitamente en `Data/Configurations`.

## Autor

**Javier Cómbita Téllez**
