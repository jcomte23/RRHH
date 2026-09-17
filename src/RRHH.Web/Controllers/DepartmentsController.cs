using Microsoft.AspNetCore.Mvc;
using RRHH.Web.Services;

namespace RRHH.Web.Controllers;

public class DepartmentsController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index()
    {
        var datos = await _departmentService.GetAllAsync();
        return View(datos);
    }
}
