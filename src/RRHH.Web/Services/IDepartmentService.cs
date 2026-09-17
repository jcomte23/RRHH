using RRHH.Web.Models;

namespace RRHH.Web.Services;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetAllAsync();

    Task<Department?> GetByIdAsync(Guid id);

    Task CreateAsync(Department department);

    Task UpdateAsync(Department department);

    Task<bool> DeleteAsync(Guid id);
}
