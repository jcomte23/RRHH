using Microsoft.AspNetCore.Mvc.Rendering;
using RRHH.Web.Models;
using RRHH.Web.Repositories;
using RRHH.Web.ViewModels.Employees;

namespace RRHH.Web.Services;

/// <summary>
/// Unico punto donde la entidad Employee se convierte en ViewModel y al
/// reves: fuera de Repositories/Services la entidad no circula.
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IDepartmentRepository _departmentRepository;

    public EmployeeService(IEmployeeRepository repository, IDepartmentRepository departmentRepository)
    {
        _repository = repository;
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<EmployeeListItemViewModel>> GetAllAsync(bool soloActivos = false, Guid? departmentId = null)
    {
        var employees = await _repository.GetAllAsync(soloActivos, departmentId);
        return employees.Select(ADetalleDeLista).ToList();
    }

    public async Task<EmployeeDetailsViewModel?> GetDetailsAsync(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee is null ? null : ADetalle(employee);
    }

    public async Task<EmployeeFormViewModel?> GetForEditAsync(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee is null ? null : AFormulario(employee);
    }

    public async Task<EmployeeDeactivateViewModel?> GetForDeactivateAsync(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee is null)
        {
            return null;
        }

        return new EmployeeDeactivateViewModel
        {
            Id = employee.Id,
            FullName = NombreCompleto(employee),
            DocumentNumber = employee.DocumentNumber,
            Position = employee.Position,
            DepartmentName = employee.Department.Name,
            HireDate = employee.HireDate,
            TerminationDate = DateOnly.FromDateTime(DateTime.Today)
        };
    }

    public Task CreateAsync(EmployeeFormViewModel modelo)
    {
        var employee = new Employee
        {
            // El id y el created_at los pone la base
            // (gen_random_uuid() / now()); un empleado nuevo nace activo.
            IsActive = true,
            TerminationDate = null,
            UpdatedAt = null
        };

        AplicarCambios(modelo, employee);
        return _repository.AddAsync(employee);
    }

    public async Task<bool> UpdateAsync(EmployeeFormViewModel modelo)
    {
        var employee = await _repository.GetByIdAsync(modelo.Id);
        if (employee is null)
        {
            return false;
        }

        AplicarCambios(modelo, employee);
        employee.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(employee);
        return true;
    }

    public async Task<bool> DeactivateAsync(Guid id, DateOnly terminationDate)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee is null)
        {
            return false;
        }

        employee.IsActive = false;
        employee.TerminationDate = terminationDate;
        employee.UpdatedAt = DateTimeOffset.UtcNow;
        await _repository.UpdateAsync(employee);
        return true;
    }

    public async Task<bool> ActivateAsync(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee is null)
        {
            return false;
        }

        employee.IsActive = true;
        employee.TerminationDate = null;
        employee.UpdatedAt = DateTimeOffset.UtcNow;
        await _repository.UpdateAsync(employee);
        return true;
    }

    public Task<bool> DocumentNumberExistsAsync(string documentNumber, Guid? excluirId = null)
    {
        return _repository.DocumentNumberExistsAsync(documentNumber.Trim(), excluirId);
    }

    public Task<bool> EmailExistsAsync(string email, Guid? excluirId = null)
    {
        return _repository.EmailExistsAsync(NormalizarCorreo(email), excluirId);
    }

    public async Task<bool> CanAssignDepartmentAsync(Guid departmentId, Guid? employeeId = null)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);
        if (department is null)
        {
            return false;
        }

        if (department.IsActive)
        {
            return true;
        }

        if (employeeId is null)
        {
            return false;
        }

        var employee = await _repository.GetByIdAsync(employeeId.Value);
        return employee?.DepartmentId == departmentId;
    }

    public async Task<IEnumerable<SelectListItem>> GetDepartmentOptionsAsync(Guid? employeeId = null)
    {
        Guid? actualId = null;
        if (employeeId.HasValue)
        {
            var employee = await _repository.GetByIdAsync(employeeId.Value);
            actualId = employee?.DepartmentId;
        }

        var departments = await _departmentRepository.GetAllAsync();
        return departments
            .Where(d => d.IsActive || d.Id == actualId)
            .Select(AOpcion)
            .ToList();
    }

    public async Task<IEnumerable<SelectListItem>> GetDepartmentFilterOptionsAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(AOpcion).ToList();
    }

    /// <summary>Vuelca el formulario sobre la entidad, ya normalizado.</summary>
    private static void AplicarCambios(EmployeeFormViewModel modelo, Employee employee)
    {
        // HireDate, Salary y DepartmentId son [Required]: aqui ya llegan validados.
        employee.DocumentNumber = modelo.DocumentNumber.Trim();
        employee.FirstName = modelo.FirstName.Trim();
        employee.LastName = modelo.LastName.Trim();
        employee.Email = string.IsNullOrWhiteSpace(modelo.Email) ? null : NormalizarCorreo(modelo.Email);
        employee.Phone = LimpiarOpcional(modelo.Phone);
        employee.Address = LimpiarOpcional(modelo.Address);
        employee.Position = modelo.Position.Trim();
        employee.HireDate = modelo.HireDate!.Value;
        employee.Salary = modelo.Salary!.Value;
        employee.DepartmentId = modelo.DepartmentId!.Value;
    }

    private static EmployeeListItemViewModel ADetalleDeLista(Employee employee)
    {
        return new EmployeeListItemViewModel
        {
            Id = employee.Id,
            DocumentNumber = employee.DocumentNumber,
            FullName = NombreCompleto(employee),
            Email = employee.Email,
            Position = employee.Position,
            DepartmentName = employee.Department.Name,
            HireDate = employee.HireDate,
            Salary = employee.Salary,
            IsActive = employee.IsActive
        };
    }

    private static EmployeeDetailsViewModel ADetalle(Employee employee)
    {
        return new EmployeeDetailsViewModel
        {
            Id = employee.Id,
            DocumentNumber = employee.DocumentNumber,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            Address = employee.Address,
            Position = employee.Position,
            HireDate = employee.HireDate,
            TerminationDate = employee.TerminationDate,
            Salary = employee.Salary,
            DepartmentId = employee.DepartmentId,
            DepartmentCode = employee.Department.Code,
            DepartmentName = employee.Department.Name,
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }

    private static EmployeeFormViewModel AFormulario(Employee employee)
    {
        return new EmployeeFormViewModel
        {
            Id = employee.Id,
            DocumentNumber = employee.DocumentNumber,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            Address = employee.Address,
            Position = employee.Position,
            HireDate = employee.HireDate,
            Salary = employee.Salary,
            DepartmentId = employee.DepartmentId
        };
    }

    private static SelectListItem AOpcion(Department department)
    {
        var texto = department.IsActive ? department.Name : $"{department.Name} (inactivo)";
        return new SelectListItem(texto, department.Id.ToString());
    }

    private static string NombreCompleto(Employee employee)
    {
        return $"{employee.FirstName} {employee.LastName}";
    }

    /// <summary>email es unique: se guarda en minusculas para no duplicar por mayusculas.</summary>
    private static string NormalizarCorreo(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static string? LimpiarOpcional(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
