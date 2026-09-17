using RRHH.Web.Models;
using RRHH.Web.Repositories;

namespace RRHH.Web.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Department>> GetAllAsync(bool soloActivos = false)
    {
        return _repository.GetAllAsync(soloActivos);
    }

    public Task<Department?> GetByIdAsync(Guid id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task CreateAsync(Department department)
    {
        Normalizar(department);

        // El id y el created_at los pone la base (gen_random_uuid() / now()).
        department.Id = Guid.Empty;
        department.UpdatedAt = null;

        return _repository.AddAsync(department);
    }

    public Task UpdateAsync(Department department)
    {
        Normalizar(department);
        department.UpdatedAt = DateTimeOffset.UtcNow;

        return _repository.UpdateAsync(department);
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

    private static void Normalizar(Department department)
    {
        department.Code = NormalizarCodigo(department.Code);
        department.Name = department.Name.Trim();
        department.Description = LimpiarOpcional(department.Description);
        department.Location = LimpiarOpcional(department.Location);
        department.Phone = LimpiarOpcional(department.Phone);
        department.Email = LimpiarOpcional(department.Email);
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
