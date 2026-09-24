using Microsoft.EntityFrameworkCore;
using RRHH.Web.Models;

namespace RRHH.Web.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        // Toma cada IEntityTypeConfiguration de Data/Configurations, asi el
        // contexto no crece al agregar entidades nuevas.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
