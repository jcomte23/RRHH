using RRHH.Web.Models;

namespace RRHH.Web.Repositories;

public interface IEmployeeRepository
{
    /// <summary>Incluye el departamento de cada empleado.</summary>
    Task<IEnumerable<Employee>> GetAllAsync(bool soloActivos = false, Guid? departmentId = null);

    /// <summary>Incluye el departamento del empleado.</summary>
    Task<Employee?> GetByIdAsync(Guid id);

    Task AddAsync(Employee employee);

    Task UpdateAsync(Employee employee);

    Task<bool> DocumentNumberExistsAsync(string documentNumber, Guid? excluirId = null);

    Task<bool> EmailExistsAsync(string email, Guid? excluirId = null);
}
