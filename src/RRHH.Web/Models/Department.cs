using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RRHH.Web.Models;

[Table("departments")]
public class Department
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("code")]
    [Display(Name = "Código")]
    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(10, ErrorMessage = "El código no puede superar los {1} caracteres.")]
    public string Code { get; set; } = string.Empty;

    [Column("name")]
    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, ErrorMessage = "El nombre no puede superar los {1} caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [Display(Name = "Descripción")]
    public string? Description { get; set; }

    [Column("location")]
    [Display(Name = "Ubicación")]
    [StringLength(80, ErrorMessage = "La ubicación no puede superar los {1} caracteres.")]
    public string? Location { get; set; }

    [Column("budget")]
    [Display(Name = "Presupuesto")]
    [Range(0, 999999999999.99, ErrorMessage = "El presupuesto no puede ser negativo.")]
    public decimal? Budget { get; set; }

    [Column("phone")]
    [Display(Name = "Teléfono")]
    [StringLength(20, ErrorMessage = "El teléfono no puede superar los {1} caracteres.")]
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    public string? Phone { get; set; }

    [Column("email")]
    [Display(Name = "Correo electrónico")]
    [StringLength(120, ErrorMessage = "El correo no puede superar los {1} caracteres.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string? Email { get; set; }

    [Column("is_active")]
    [Display(Name = "Activo")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    [Display(Name = "Creado")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    [Display(Name = "Última edición")]
    public DateTimeOffset? UpdatedAt { get; set; }
}
