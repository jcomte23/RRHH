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

    public Task<IEnumerable<Department>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<Department?> GetByIdAsync(Guid id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task CreateAsync(Department department)
    {
        if (department.Id == Guid.Empty)
        {
            department.Id = Guid.NewGuid();
        }

        return _repository.AddAsync(department);
    }

    public Task UpdateAsync(Department department)
    {
        return _repository.UpdateAsync(department);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var department = await _repository.GetByIdAsync(id);
        if (department is null)
        {
            return false;
        }

        await _repository.DeleteAsync(department);
        return true;
    }
}
