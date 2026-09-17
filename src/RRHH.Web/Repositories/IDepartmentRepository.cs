using RRHH.Web.Models;

namespace RRHH.Web.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync(bool soloActivos = false);

    Task<Department?> GetByIdAsync(Guid id);

    Task AddAsync(Department department);

    Task UpdateAsync(Department department);

    Task<bool> CodeExistsAsync(string code, Guid? excluirId = null);

    Task<bool> NameExistsAsync(string name, Guid? excluirId = null);
}
