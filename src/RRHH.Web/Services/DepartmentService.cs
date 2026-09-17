using RRHH.Web.Models;
using RRHH.Web.Repositories;
using RRHH.Web.ViewModels;

namespace RRHH.Web.Services;

/// <summary>
/// Unico punto donde la entidad Department se convierte en ViewModel y al
/// reves: fuera de Repositories/Services la entidad no circula.
/// </summary>
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DepartmentListItemViewModel>> GetAllAsync(bool soloActivos = false)
    {
        var departments = await _repository.GetAllAsync(soloActivos);
        return departments.Select(ADetalleDeLista).ToList();
    }

    public async Task<DepartmentDetailsViewModel?> GetDetailsAsync(Guid id)
    {
        var department = await _repository.GetByIdAsync(id);
        return department is null ? null : ADetalle(department);
    }

    public async Task<DepartmentFormViewModel?> GetForEditAsync(Guid id)
    {
        var department = await _repository.GetByIdAsync(id);
        return department is null ? null : AFormulario(department);
    }

    public Task CreateAsync(DepartmentFormViewModel modelo)
    {
        var department = new Department
        {
            // El id y el created_at los pone la base
            // (gen_random_uuid() / now()); un departamento nuevo nace activo.
            IsActive = true,
            UpdatedAt = null
        };

        AplicarCambios(modelo, department);
        return _repository.AddAsync(department);
    }

    public async Task<bool> UpdateAsync(DepartmentFormViewModel modelo)
    {
        var department = await _repository.GetByIdAsync(modelo.Id);
        if (department is null)
        {
            return false;
        }

        AplicarCambios(modelo, department);
        department.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(department);
        return true;
    }

    public Task<bool> DeactivateAsync(Guid id)
    {
        return CambiarEstadoAsync(id, activo: false);
    }

    public Task<bool> ActivateAsync(Guid id)
    {
        return CambiarEstadoAsync(id, activo: true);
    }

    public Task<bool> CodeExistsAsync(string code, Guid? excluirId = null)
    {
        return _repository.CodeExistsAsync(NormalizarCodigo(code), excluirId);
    }

    public Task<bool> NameExistsAsync(string name, Guid? excluirId = null)
    {
        return _repository.NameExistsAsync(name.Trim(), excluirId);
    }

    private async Task<bool> CambiarEstadoAsync(Guid id, bool activo)
    {
        var department = await _repository.GetByIdAsync(id);
        if (department is null)
        {
            return false;
        }

        department.IsActive = activo;
        department.UpdatedAt = DateTimeOffset.UtcNow;
        await _repository.UpdateAsync(department);
        return true;
    }

    /// <summary>Vuelca el formulario sobre la entidad, ya normalizado.</summary>
    private static void AplicarCambios(DepartmentFormViewModel modelo, Department department)
    {
        department.Code = NormalizarCodigo(modelo.Code);
        department.Name = modelo.Name.Trim();
        department.Description = LimpiarOpcional(modelo.Description);
        department.Location = LimpiarOpcional(modelo.Location);
        department.Budget = modelo.Budget;
        department.Phone = LimpiarOpcional(modelo.Phone);
        department.Email = LimpiarOpcional(modelo.Email);
    }

    private static DepartmentListItemViewModel ADetalleDeLista(Department department)
    {
        return new DepartmentListItemViewModel
        {
            Id = department.Id,
            Code = department.Code,
            Name = department.Name,
            Description = department.Description,
            Location = department.Location,
            Budget = department.Budget,
            IsActive = department.IsActive
        };
    }

    private static DepartmentDetailsViewModel ADetalle(Department department)
    {
        return new DepartmentDetailsViewModel
        {
            Id = department.Id,
            Code = department.Code,
            Name = department.Name,
            Description = department.Description,
            Location = department.Location,
            Budget = department.Budget,
            Phone = department.Phone,
            Email = department.Email,
            IsActive = department.IsActive,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt
        };
    }

    private static DepartmentFormViewModel AFormulario(Department department)
    {
        return new DepartmentFormViewModel
        {
            Id = department.Id,
            Code = department.Code,
            Name = department.Name,
            Description = department.Description,
            Location = department.Location,
            Budget = department.Budget,
            Phone = department.Phone,
            Email = department.Email
        };
    }

    private static string NormalizarCodigo(string code)
    {
        return code.Trim().ToUpperInvariant();
    }

    private static string? LimpiarOpcional(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
