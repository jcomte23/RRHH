using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RRHH.Web.Models;

[Table("departments")]
public class Department
{
    [Key]
    [Column("id")]
    public Guid Id  { get; set; } 
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
}