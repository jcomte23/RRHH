using RRHH.Web.Models;

namespace RRHH.Web.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();

    Task<Department?> GetByIdAsync(Guid id);

    Task AddAsync(Department department);

    Task UpdateAsync(Department department);

    Task DeleteAsync(Department department);
}
