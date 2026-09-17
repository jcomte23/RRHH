using System.ComponentModel.DataAnnotations;

namespace RRHH.Web.ViewModels;

/// <summary>
/// Campos editables de un departamento. No expone id generado, fechas ni
/// is_active: eso lo manejan la base y las acciones Activate/Deactivate.
/// </summary>
public class DepartmentFormViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Código")]
    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(10, ErrorMessage = "El código no puede superar los {1} caracteres.")]
    public string Code { get; set; } = string.Empty;

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, ErrorMessage = "El nombre no puede superar los {1} caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    public string? Description { get; set; }

    [Display(Name = "Ubicación")]
    [StringLength(80, ErrorMessage = "La ubicación no puede superar los {1} caracteres.")]
    public string? Location { get; set; }

    [Display(Name = "Presupuesto")]
    [Range(0, 999999999999.99, ErrorMessage = "El presupuesto no puede ser negativo.")]
    public decimal? Budget { get; set; }

    [Display(Name = "Teléfono")]
    [StringLength(20, ErrorMessage = "El teléfono no puede superar los {1} caracteres.")]
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    public string? Phone { get; set; }

    [Display(Name = "Correo electrónico")]
    [StringLength(120, ErrorMessage = "El correo no puede superar los {1} caracteres.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string? Email { get; set; }
}
