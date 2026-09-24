using System.ComponentModel.DataAnnotations;

namespace RRHH.Web.ViewModels.Employees;

/// <summary>
/// Pantalla de retiro. Del POST solo se usa TerminationDate; el resto se
/// vuelve a leer de la base para no confiar en lo que mande el navegador.
/// </summary>
public class EmployeeDeactivateViewModel
{
    public Guid Id { get; init; }

    [Display(Name = "Empleado")]
    public string FullName { get; init; } = string.Empty;

    [Display(Name = "Documento")]
    public string DocumentNumber { get; init; } = string.Empty;

    [Display(Name = "Cargo")]
    public string Position { get; init; } = string.Empty;

    [Display(Name = "Departamento")]
    public string DepartmentName { get; init; } = string.Empty;

    [Display(Name = "Fecha de ingreso")]
    public DateOnly HireDate { get; init; }

    [Display(Name = "Fecha de retiro")]
    [DataType(DataType.Date)]
    public DateOnly? TerminationDate { get; set; }
}
