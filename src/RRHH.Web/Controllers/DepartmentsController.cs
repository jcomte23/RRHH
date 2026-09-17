using Microsoft.AspNetCore.Mvc;
using RRHH.Web.Models;
using RRHH.Web.Services;

namespace RRHH.Web.Controllers;

public class DepartmentsController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(bool soloActivos = false)
    {
        ViewData["SoloActivos"] = soloActivos;
        var datos = await _departmentService.GetAllAsync(soloActivos);
        return View(datos);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department is null)
        {
            return NotFound();
        }

        return View(department);
    }

    public IActionResult Create()
    {
        return View(new Department());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Department department)
    {
        await ValidarDuplicadosAsync(department);

        if (!ModelState.IsValid)
        {
            return View(department);
        }

        await _departmentService.CreateAsync(department);
        TempData["Mensaje"] = $"El departamento {department.Name} se creó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department is null)
        {
            return NotFound();
        }

        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Department department)
    {
        if (id != department.Id)
        {
            return BadRequest();
        }

        await ValidarDuplicadosAsync(department);

        if (!ModelState.IsValid)
        {
            return View(department);
        }

        var actual = await _departmentService.GetByIdAsync(id);
        if (actual is null)
        {
            return NotFound();
        }

        // Solo se copian los campos editables: id, created_at y updated_at
        // no viajan en el formulario.
        actual.Code = department.Code;
        actual.Name = department.Name;
        actual.Description = department.Description;
        actual.Location = department.Location;
        actual.Budget = department.Budget;
        actual.Phone = department.Phone;
        actual.Email = department.Email;
        actual.IsActive = department.IsActive;

        await _departmentService.UpdateAsync(actual);
        TempData["Mensaje"] = $"El departamento {actual.Name} se actualizó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>Pantalla de confirmacion del borrado logico.</summary>
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department is null)
        {
            return NotFound();
        }

        return View(department);
    }

    [HttpPost]
    [ActionName(nameof(Deactivate))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(Guid id)
    {
        if (!await _departmentService.DeactivateAsync(id))
        {
            return NotFound();
        }

        TempData["Mensaje"] = "El departamento se desactivó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id)
    {
        if (!await _departmentService.ActivateAsync(id))
        {
            return NotFound();
        }

        TempData["Mensaje"] = "El departamento se reactivó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// code y name son unique en la base: se valida antes para mostrar un
    /// mensaje en el formulario en lugar de dejar estallar la excepcion.
    /// </summary>
    private async Task ValidarDuplicadosAsync(Department department)
    {
        var excluirId = department.Id == Guid.Empty ? (Guid?)null : department.Id;

        if (!string.IsNullOrWhiteSpace(department.Code)
            && await _departmentService.CodeExistsAsync(department.Code, excluirId))
        {
            ModelState.AddModelError(nameof(Department.Code), "Ya existe un departamento con ese código.");
        }

        if (!string.IsNullOrWhiteSpace(department.Name)
            && await _departmentService.NameExistsAsync(department.Name, excluirId))
        {
            ModelState.AddModelError(nameof(Department.Name), "Ya existe un departamento con ese nombre.");
        }
    }
}
