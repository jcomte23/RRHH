using Microsoft.EntityFrameworkCore;
using RRHH.Web.Models;

namespace RRHH.Web.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Department> Departments { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>(entity =>
        {
            // La base genera el id y el created_at; EF los omite en el INSERT
            // y los lee de vuelta.
            entity.Property(d => d.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .ValueGeneratedOnAdd();

            entity.Property(d => d.CreatedAt)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            entity.Property(d => d.IsActive).HasDefaultValue(true);

            entity.Property(d => d.Budget).HasPrecision(14, 2);

            entity.HasIndex(d => d.Code).IsUnique();
            entity.HasIndex(d => d.Name).IsUnique();
        });
    }
}
