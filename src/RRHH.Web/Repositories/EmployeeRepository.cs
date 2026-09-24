using Microsoft.EntityFrameworkCore;
using RRHH.Web.Data;
using RRHH.Web.Models;

namespace RRHH.Web.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(bool soloActivos = false, Guid? departmentId = null)
    {
        var consulta = _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .AsQueryable();

        if (soloActivos)
        {
            consulta = consulta.Where(e => e.IsActive);
        }

        if (departmentId.HasValue)
        {
            consulta = consulta.Where(e => e.DepartmentId == departmentId.Value);
        }

        return await consulta
            .OrderByDescending(e => e.IsActive)
            .ThenBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        // El empleado viene rastreado desde GetByIdAsync. No se llama a
        // Update(): recorreria el grafo y marcaria tambien el Department
        // incluido como modificado.
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DocumentNumberExistsAsync(string documentNumber, Guid? excluirId = null)
    {
        return await _context.Employees
            .AsNoTracking()
            .AnyAsync(e => e.DocumentNumber == documentNumber && (excluirId == null || e.Id != excluirId));
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excluirId = null)
    {
        return await _context.Employees
            .AsNoTracking()
            .AnyAsync(e => e.Email == email && (excluirId == null || e.Id != excluirId));
    }
}
