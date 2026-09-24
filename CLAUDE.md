# CLAUDE.md

Guía para trabajar en este repositorio. Proyecto de estudio (no productivo) de ASP.NET Core MVC.

## Stack

- .NET 10, ASP.NET Core MVC (controladores + vistas Razor), `Nullable` e `ImplicitUsings` activos.
- EF Core 10 + Npgsql sobre PostgreSQL. Enfoque **code-first** con migraciones en `Data/Migrations`
  (herramienta `dotnet-ef` local en `dotnet-tools.json`).
- Bootstrap 5 y jQuery Validation servidos desde `wwwroot/lib` (versionados en el repo).
- Solución: `RRHH.slnx` con un único proyecto, `src/RRHH.Web`.

## Comandos

```bash
dotnet build RRHH.slnx
dotnet run --project src/RRHH.Web
dotnet tool restore
dotnet ef database update --project src/RRHH.Web
dotnet ef migrations add <Nombre> --project src/RRHH.Web
```

No hay proyecto de pruebas todavía.

## Arquitectura

Flujo: `Controller → I*Service → I*Repository → ApplicationDbContext → PostgreSQL`.

- **Controllers/**: reciben y devuelven ViewModels; nunca tocan entidades ni el `DbContext`.
  Validaciones que requieren la base (código/nombre duplicados) se agregan a `ModelState` antes de guardar.
- **Services/**: lógica de negocio y **único lugar** donde se convierte entidad ↔ ViewModel
  (métodos privados `ADetalle`, `AFormulario`, etc.). También normalizan datos (trim, código en mayúsculas).
- **Repositories/**: acceso a datos con entidades. Lecturas de listado con `AsNoTracking`.
- **Models/**: entidades de persistencia, sin Data Annotations.
- **Data/Configurations/**: mapeo Fluent API con `IEntityTypeConfiguration<T>`; se cargan solas con
  `ApplyConfigurationsFromAssembly`, no hace falta tocar `ApplicationDbContext` al agregar entidades.
- **ViewModels/<Módulo>/**: uno por uso (`ListItem`, `Details`, `Form`). Las validaciones
  (Data Annotations con mensajes en español) viven aquí.
- Registro en DI en `Program.cs` con `AddScoped<IInterfaz, Implementacion>()`.

## Añadir un módulo nuevo (p. ej. Employees)

1. Entidad en `Models/` + `XConfiguration` en `Data/Configurations/` + `DbSet` en `ApplicationDbContext`.
2. Generar la migración con `dotnet ef migrations add`.
3. `IXRepository`/`XRepository`, `IXService`/`XService`, ViewModels en `ViewModels/X/`.
4. Registrar ambos en `Program.cs`, crear `XController` y vistas en `Views/X/` (formulario compartido en `_Form.cshtml`).

## Convenciones

- Código (clases, propiedades, métodos públicos) en inglés; textos de UI, comentarios, variables locales
  y métodos privados de apoyo en español (`soloActivos`, `ValidarDuplicadosAsync`, `LimpiarOpcional`).
- Tablas y columnas en `snake_case`, siempre mapeadas explícitamente con `HasColumnName`.
- Valores generados por la base (`id` con `gen_random_uuid()`, `created_at` con `now()`) se configuran
  con `HasDefaultValueSql` + `ValueGeneratedOnAdd`; la app no los asigna.
- `UpdatedAt` se asigna en el servicio con `DateTimeOffset.UtcNow` (Npgsql exige offset 0 para `timestamptz`).
- Dinero: `decimal` con `HasPrecision` igual al `numeric` de la base.
- Borrado lógico con `is_active`; nunca se eliminan registros. Acciones POST `Activate`/`Deactivate`.
- Toda acción POST lleva `[ValidateAntiForgeryToken]`. Mensajes de éxito vía `TempData["Mensaje"]`.
- Commits en español con prefijo conventional commits (`feat:`, `fix:`, `refactor:`, `docs:`, `chore:`).

## Cuidado

- **No subir credenciales.** La cadena de conexión `DefaultConnection` debe ir en `dotnet user-secrets`
  o variables de entorno, no en `appsettings*.json`.
- **Cultura:** `Program.cs` fija con `UseRequestLocalization` una `es-CO` con **punto decimal** (fechas en
  español, números como los envían `<input type="number">` y jQuery Validation). No quitarla: con la coma
  de `es-CO` el model binding leía `1500.50` como `150050`.
- Cambios de esquema: configuración Fluent + nueva migración. `db/schema.sql` es el script original y ya no
  coincide con el modelo (p. ej. `employees.document` vs `document_number`).
- En `Index.cshtml` de Departamentos, el buscador, Importar/Exportar y la paginación son maqueta sin lógica.
