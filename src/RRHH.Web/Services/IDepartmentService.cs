using RRHH.Web.ViewModels;

namespace RRHH.Web.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentListItemViewModel>> GetAllAsync(bool soloActivos = false);

    Task<DepartmentDetailsViewModel?> GetDetailsAsync(Guid id);

    Task<DepartmentFormViewModel?> GetForEditAsync(Guid id);

    Task CreateAsync(DepartmentFormViewModel modelo);

    Task<bool> UpdateAsync(DepartmentFormViewModel modelo);

    /// <summary>Borrado logico: desactiva el departamento en lugar de eliminarlo.</summary>
    Task<bool> DeactivateAsync(Guid id);

    Task<bool> ActivateAsync(Guid id);

    Task<bool> CodeExistsAsync(string code, Guid? excluirId = null);

    Task<bool> NameExistsAsync(string name, Guid? excluirId = null);
}
