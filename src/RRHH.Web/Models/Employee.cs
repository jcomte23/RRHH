namespace RRHH.Web.Models;

/// <summary>
/// Entidad de persistencia. El mapeo esta en
/// Data/Configurations/EmployeeConfiguration y las reglas de validacion
/// en los ViewModels.
/// </summary>
public class Employee
{
    public Guid Id { get; set; }

    public string DocumentNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string Position { get; set; } = string.Empty;

    public DateOnly HireDate { get; set; }

    public DateOnly? TerminationDate { get; set; }

    public decimal Salary { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public Guid DepartmentId { get; set; }

    public Department Department { get; set; } = null!;
}
