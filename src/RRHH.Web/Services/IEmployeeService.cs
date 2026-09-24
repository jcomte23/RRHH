using Microsoft.AspNetCore.Mvc.Rendering;
using RRHH.Web.ViewModels.Employees;

namespace RRHH.Web.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeListItemViewModel>> GetAllAsync(bool soloActivos = false, Guid? departmentId = null);

    Task<EmployeeDetailsViewModel?> GetDetailsAsync(Guid id);

    Task<EmployeeFormViewModel?> GetForEditAsync(Guid id);

    Task<EmployeeDeactivateViewModel?> GetForDeactivateAsync(Guid id);

    Task CreateAsync(EmployeeFormViewModel modelo);

    Task<bool> UpdateAsync(EmployeeFormViewModel modelo);

    /// <summary>Borrado logico: registra la fecha de retiro y desactiva al empleado.</summary>
    Task<bool> DeactivateAsync(Guid id, DateOnly terminationDate);

    /// <summary>Reintegro: vuelve a activarlo y limpia la fecha de retiro.</summary>
    Task<bool> ActivateAsync(Guid id);

    Task<bool> DocumentNumberExistsAsync(string documentNumber, Guid? excluirId = null);

    Task<bool> EmailExistsAsync(string email, Guid? excluirId = null);

    /// <summary>
    /// Un empleado solo se asigna a departamentos activos, salvo que ya
    /// pertenezca a uno que se desactivo despues.
    /// </summary>
    Task<bool> CanAssignDepartmentAsync(Guid departmentId, Guid? employeeId = null);

    /// <summary>Opciones del formulario: departamentos activos mas el actual del empleado.</summary>
    Task<IEnumerable<SelectListItem>> GetDepartmentOptionsAsync(Guid? employeeId = null);

    /// <summary>Opciones del filtro del listado: todos los departamentos.</summary>
    Task<IEnumerable<SelectListItem>> GetDepartmentFilterOptionsAsync();
}
