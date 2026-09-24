using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RRHH.Web.ViewModels.Employees;

/// <summary>
/// Campos editables de un empleado. No expone id generado, fechas de
/// auditoria, is_active ni la fecha de retiro: eso lo manejan la base y las
/// acciones Activate/Deactivate.
/// </summary>
public class EmployeeFormViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Documento")]
    [Required(ErrorMessage = "El documento es obligatorio.")]
    [StringLength(20, ErrorMessage = "El documento no puede superar los {1} caracteres.")]
    public string DocumentNumber { get; set; } = string.Empty;

    [Display(Name = "Nombres")]
    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(60, ErrorMessage = "Los nombres no pueden superar los {1} caracteres.")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Apellidos")]
    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(60, ErrorMessage = "Los apellidos no pueden superar los {1} caracteres.")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Correo electrónico")]
    [StringLength(120, ErrorMessage = "El correo no puede superar los {1} caracteres.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string? Email { get; set; }

    [Display(Name = "Teléfono")]
    [StringLength(20, ErrorMessage = "El teléfono no puede superar los {1} caracteres.")]
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    public string? Phone { get; set; }

    [Display(Name = "Dirección")]
    [StringLength(150, ErrorMessage = "La dirección no puede superar los {1} caracteres.")]
    public string? Address { get; set; }

    [Display(Name = "Cargo")]
    [Required(ErrorMessage = "El cargo es obligatorio.")]
    [StringLength(80, ErrorMessage = "El cargo no puede superar los {1} caracteres.")]
    public string Position { get; set; } = string.Empty;

    // Nullable para que el campo vacio falle en [Required] en lugar de
    // llegar como 01/01/0001.
    [Display(Name = "Fecha de ingreso")]
    [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
    [DataType(DataType.Date)]
    public DateOnly? HireDate { get; set; }

    [Display(Name = "Salario")]
    [Required(ErrorMessage = "El salario es obligatorio.")]
    [Range(0.01, 999999999999.99, ErrorMessage = "El salario debe ser mayor que cero y no superar {2}.")]
    public decimal? Salary { get; set; }

    [Display(Name = "Departamento")]
    [Required(ErrorMessage = "Selecciona un departamento.")]
    public Guid? DepartmentId { get; set; }

    /// <summary>Opciones del select; las carga el controlador, no llegan en el POST.</summary>
    [ValidateNever]
    public IEnumerable<SelectListItem> Departments { get; set; } = [];
}
