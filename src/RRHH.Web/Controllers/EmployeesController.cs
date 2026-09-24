using Microsoft.AspNetCore.Mvc;
using RRHH.Web.Services;
using RRHH.Web.ViewModels.Employees;

namespace RRHH.Web.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<IActionResult> Index(bool soloActivos = false, Guid? departmentId = null)
    {
        ViewData["SoloActivos"] = soloActivos;
        ViewData["DepartmentId"] = departmentId;
        ViewData["Departamentos"] = await _employeeService.GetDepartmentFilterOptionsAsync();

        var datos = await _employeeService.GetAllAsync(soloActivos, departmentId);
        return View(datos);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var empleado = await _employeeService.GetDetailsAsync(id);
        if (empleado is null)
        {
            return NotFound();
        }

        return View(empleado);
    }

    /// <summary>departmentId permite abrir el formulario desde el detalle de un departamento.</summary>
    public async Task<IActionResult> Create(Guid? departmentId = null)
    {
        var modelo = new EmployeeFormViewModel
        {
            DepartmentId = departmentId,
            HireDate = DateOnly.FromDateTime(DateTime.Today)
        };

        await CargarDepartamentosAsync(modelo);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeFormViewModel modelo)
    {
        await ValidarAsync(modelo);

        if (!ModelState.IsValid)
        {
            await CargarDepartamentosAsync(modelo);
            return View(modelo);
        }

        await _employeeService.CreateAsync(modelo);
        TempData["Mensaje"] = $"El empleado {modelo.FirstName} {modelo.LastName} se creó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var modelo = await _employeeService.GetForEditAsync(id);
        if (modelo is null)
        {
            return NotFound();
        }

        await CargarDepartamentosAsync(modelo);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EmployeeFormViewModel modelo)
    {
        if (id != modelo.Id)
        {
            return BadRequest();
        }

        await ValidarAsync(modelo);

        if (!ModelState.IsValid)
        {
            await CargarDepartamentosAsync(modelo);
            return View(modelo);
        }

        if (!await _employeeService.UpdateAsync(modelo))
        {
            return NotFound();
        }

        TempData["Mensaje"] = $"El empleado {modelo.FirstName} {modelo.LastName} se actualizó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>Pantalla de confirmacion del retiro (borrado logico).</summary>
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var modelo = await _employeeService.GetForDeactivateAsync(id);
        if (modelo is null)
        {
            return NotFound();
        }

        return View(modelo);
    }

    [HttpPost]
    [ActionName(nameof(Deactivate))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(Guid id, DateOnly? terminationDate)
    {
        var modelo = await _employeeService.GetForDeactivateAsync(id);
        if (modelo is null)
        {
            return NotFound();
        }

        modelo.TerminationDate = terminationDate;

        if (terminationDate is null)
        {
            ModelState.AddModelError(nameof(modelo.TerminationDate), "La fecha de retiro es obligatoria.");
        }
        else if (terminationDate < modelo.HireDate)
        {
            ModelState.AddModelError(nameof(modelo.TerminationDate), "La fecha de retiro no puede ser anterior a la fecha de ingreso.");
        }

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        await _employeeService.DeactivateAsync(id, terminationDate!.Value);
        TempData["Mensaje"] = $"El empleado {modelo.FullName} se desactivó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id)
    {
        if (!await _employeeService.ActivateAsync(id))
        {
            return NotFound();
        }

        TempData["Mensaje"] = "El empleado se reactivó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// document_number y email son unique en la base: se valida antes para
    /// mostrar un mensaje en el formulario en lugar de dejar estallar la
    /// excepcion. Tambien que el departamento exista y se pueda asignar.
    /// </summary>
    private async Task ValidarAsync(EmployeeFormViewModel modelo)
    {
        var excluirId = modelo.Id == Guid.Empty ? (Guid?)null : modelo.Id;

        if (!string.IsNullOrWhiteSpace(modelo.DocumentNumber)
            && await _employeeService.DocumentNumberExistsAsync(modelo.DocumentNumber, excluirId))
        {
            ModelState.AddModelError(nameof(modelo.DocumentNumber), "Ya existe un empleado con ese documento.");
        }

        if (!string.IsNullOrWhiteSpace(modelo.Email)
            && await _employeeService.EmailExistsAsync(modelo.Email, excluirId))
        {
            ModelState.AddModelError(nameof(modelo.Email), "Ya existe un empleado con ese correo.");
        }

        if (modelo.DepartmentId.HasValue
            && !await _employeeService.CanAssignDepartmentAsync(modelo.DepartmentId.Value, excluirId))
        {
            ModelState.AddModelError(nameof(modelo.DepartmentId), "El departamento no existe o está inactivo.");
        }
    }

    private async Task CargarDepartamentosAsync(EmployeeFormViewModel modelo)
    {
        var empleadoId = modelo.Id == Guid.Empty ? (Guid?)null : modelo.Id;
        modelo.Departments = await _employeeService.GetDepartmentOptionsAsync(empleadoId);
    }
}
