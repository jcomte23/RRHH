using RRHH.Web.Models;

namespace RRHH.Web.Services;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetAllAsync(bool soloActivos = false);

    Task<Department?> GetByIdAsync(Guid id);

    Task CreateAsync(Department department);

    Task UpdateAsync(Department department);

    /// <summary>Borrado logico: desactiva el departamento en lugar de eliminarlo.</summary>
    Task<bool> DeactivateAsync(Guid id);

    Task<bool> ActivateAsync(Guid id);

    Task<bool> CodeExistsAsync(string code, Guid? excluirId = null);

    Task<bool> NameExistsAsync(string name, Guid? excluirId = null);
}
