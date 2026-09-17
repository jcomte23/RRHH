using System.ComponentModel.DataAnnotations;

namespace RRHH.Web.ViewModels.Departments;

/// <summary>Vista de solo lectura del departamento completo.</summary>
public class DepartmentDetailsViewModel
{
    public Guid Id { get; init; }

    [Display(Name = "Código")]
    public string Code { get; init; } = string.Empty;

    [Display(Name = "Nombre")]
    public string Name { get; init; } = string.Empty;

    [Display(Name = "Descripción")]
    public string? Description { get; init; }

    [Display(Name = "Ubicación")]
    public string? Location { get; init; }

    [Display(Name = "Presupuesto")]
    public decimal? Budget { get; init; }

    [Display(Name = "Teléfono")]
    public string? Phone { get; init; }

    [Display(Name = "Correo electrónico")]
    public string? Email { get; init; }

    [Display(Name = "Activo")]
    public bool IsActive { get; init; }

    [Display(Name = "Creado")]
    public DateTimeOffset CreatedAt { get; init; }

    [Display(Name = "Última edición")]
    public DateTimeOffset? UpdatedAt { get; init; }

    public string BudgetTexto => Budget.HasValue ? Budget.Value.ToString("C2") : "—";

    public string CreatedAtTexto => CreatedAt.ToLocalTime().ToString("g");

    public string UpdatedAtTexto => UpdatedAt.HasValue
        ? UpdatedAt.Value.ToLocalTime().ToString("g")
        : "Sin ediciones";
}
