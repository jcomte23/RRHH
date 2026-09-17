using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH.Web.Models;

namespace RRHH.Web.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(d => d.Id);

        // La base genera el id y el created_at; EF los omite en el INSERT
        // y los lee de vuelta.
        builder.Property(d => d.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(d => d.Code)
            .HasColumnName("code")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(d => d.Name)
            .HasColumnName("name")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(d => d.Description)
            .HasColumnName("description");

        builder.Property(d => d.Location)
            .HasColumnName("location")
            .HasMaxLength(80);

        // numeric(14,2): con plata nunca punto flotante.
        builder.Property(d => d.Budget)
            .HasColumnName("budget")
            .HasPrecision(14, 2);

        builder.Property(d => d.Phone)
            .HasColumnName("phone")
            .HasMaxLength(20);

        builder.Property(d => d.Email)
            .HasColumnName("email")
            .HasMaxLength(120);

        // Borrado logico: los departamentos disueltos se desactivan.
        builder.Property(d => d.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasIndex(d => d.Code).IsUnique();
        builder.HasIndex(d => d.Name).IsUnique();
    }
}
