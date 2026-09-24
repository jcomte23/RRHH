using System.ComponentModel.DataAnnotations;

namespace RRHH.Web.ViewModels.Employees;

/// <summary>Vista de solo lectura del empleado completo.</summary>
public class EmployeeDetailsViewModel
{
    public Guid Id { get; init; }

    [Display(Name = "Documento")]
    public string DocumentNumber { get; init; } = string.Empty;

    [Display(Name = "Nombres")]
    public string FirstName { get; init; } = string.Empty;

    [Display(Name = "Apellidos")]
    public string LastName { get; init; } = string.Empty;

    [Display(Name = "Correo electrónico")]
    public string? Email { get; init; }

    [Display(Name = "Teléfono")]
    public string? Phone { get; init; }

    [Display(Name = "Dirección")]
    public string? Address { get; init; }

    [Display(Name = "Cargo")]
    public string Position { get; init; } = string.Empty;

    [Display(Name = "Fecha de ingreso")]
    public DateOnly HireDate { get; init; }

    [Display(Name = "Fecha de retiro")]
    public DateOnly? TerminationDate { get; init; }

    [Display(Name = "Salario")]
    public decimal Salary { get; init; }

    public Guid DepartmentId { get; init; }

    public string DepartmentCode { get; init; } = string.Empty;

    [Display(Name = "Departamento")]
    public string DepartmentName { get; init; } = string.Empty;

    [Display(Name = "Activo")]
    public bool IsActive { get; init; }

    [Display(Name = "Creado")]
    public DateTimeOffset CreatedAt { get; init; }

    [Display(Name = "Última edición")]
    public DateTimeOffset? UpdatedAt { get; init; }

    public string FullName => $"{FirstName} {LastName}";

    public string SalaryTexto => Salary.ToString("C2");

    public string HireDateTexto => HireDate.ToString("d");

    public string TerminationDateTexto => TerminationDate.HasValue
        ? TerminationDate.Value.ToString("d")
        : "—";

    public string CreatedAtTexto => CreatedAt.ToLocalTime().ToString("g");

    public string UpdatedAtTexto => UpdatedAt.HasValue
        ? UpdatedAt.Value.ToLocalTime().ToString("g")
        : "Sin ediciones";
}
