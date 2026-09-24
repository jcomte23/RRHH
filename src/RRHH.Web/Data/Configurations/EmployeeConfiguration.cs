using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH.Web.Models;

namespace RRHH.Web.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");

        builder.HasKey(e => e.Id);

        // La base genera el id y el created_at; EF los omite en el INSERT
        // y los lee de vuelta.
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        // Cedula o documento de identidad: texto y no numero, porque puede
        // traer ceros a la izquierda.
        builder.Property(e => e.DocumentNumber)
            .HasColumnName("document_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(e => e.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasColumnName("email")
            .HasMaxLength(120);

        builder.Property(e => e.Phone)
            .HasColumnName("phone")
            .HasMaxLength(20);

        builder.Property(e => e.Address)
            .HasColumnName("address")
            .HasMaxLength(150);

        builder.Property(e => e.Position)
            .HasColumnName("position")
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(e => e.HireDate)
            .HasColumnName("hire_date")
            .IsRequired();

        // Queda null mientras el empleado siga vinculado.
        builder.Property(e => e.TerminationDate)
            .HasColumnName("termination_date");

        // numeric(14,2): con plata nunca punto flotante.
        builder.Property(e => e.Salary)
            .HasColumnName("salary")
            .HasPrecision(14, 2)
            .IsRequired();

        // Borrado logico: los retirados se desactivan, no se eliminan.
        builder.Property(e => e.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.DepartmentId)
            .HasColumnName("department_id")
            .IsRequired();

        // Un departamento con empleados no se puede borrar: primero se
        // reubican o se desactiva el departamento.
        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.DocumentNumber).IsUnique();
        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.DepartmentId);
    }
}
