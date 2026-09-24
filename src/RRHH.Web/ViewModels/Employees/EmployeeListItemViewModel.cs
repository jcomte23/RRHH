namespace RRHH.Web.ViewModels.Employees;

/// <summary>Lo que necesita una fila del listado, nada mas.</summary>
public class EmployeeListItemViewModel
{
    public Guid Id { get; init; }

    public string DocumentNumber { get; init; } = string.Empty;

    public string FullName { get; init; } = string.Empty;

    public string? Email { get; init; }

    public string Position { get; init; } = string.Empty;

    public string DepartmentName { get; init; } = string.Empty;

    public DateOnly HireDate { get; init; }

    public decimal Salary { get; init; }

    public bool IsActive { get; init; }
}
