namespace RRHH.Web.Models;

/// <summary>
/// Entidad de persistencia. El mapeo esta en
/// Data/Configurations/DepartmentConfiguration y las reglas de validacion
/// en los ViewModels.
/// </summary>
public class Department
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public decimal? Budget { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}
