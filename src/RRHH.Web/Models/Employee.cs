using System.ComponentModel.DataAnnotations.Schema;

namespace RRHH.Web.Models;

public class Employee
{
    [Column("first_name")]
    public string Name { get; set; }
}