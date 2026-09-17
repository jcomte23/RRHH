using Microsoft.EntityFrameworkCore;
using RRHH.Web.Data;
using RRHH.Web.Models;

namespace RRHH.Web.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync(bool soloActivos = false)
    {
        var consulta = _context.Departments.AsNoTracking().AsQueryable();

        if (soloActivos)
        {
            consulta = consulta.Where(d => d.IsActive);
        }

        return await consulta
            .OrderByDescending(d => d.IsActive)
            .ThenBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(Guid id)
    {
        return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CodeExistsAsync(string code, Guid? excluirId = null)
    {
        return await _context.Departments
            .AsNoTracking()
            .AnyAsync(d => d.Code == code && (excluirId == null || d.Id != excluirId));
    }

    public async Task<bool> NameExistsAsync(string name, Guid? excluirId = null)
    {
        return await _context.Departments
            .AsNoTracking()
            .AnyAsync(d => d.Name == name && (excluirId == null || d.Id != excluirId));
    }
}
