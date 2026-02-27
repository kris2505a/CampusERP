using Application.Dto;
using Domain.Entity;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class DepartmentServices {

    public DepartmentServices(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<List<DepartmentDataResponse>> GetDepartments() {
        return await _context.Departments
            .Select(d => new DepartmentDataResponse(d.Id, d.Name))
            .ToListAsync();
    }

    public async Task<DepartmentDataResponse?> GetDepartmentById(long id) {
        return await _context.Departments
            .Where(d => d.Id == id)
            .Select(dep => new DepartmentDataResponse(dep.Id, dep.Name))
            .FirstOrDefaultAsync();
    }

    public async Task<DepartmentDataResponse?> AddDepartment(DepartmentCreationRequest request) {
        bool found = await _context.Departments
            .AnyAsync(d => d.Name == request.name);
        
        if(found) {
            return null;
        }

        var department = new Department { Name = request.name };
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return new DepartmentDataResponse(
            department.Id,
            department.Name
        );
    }

    public async Task<DepartmentDataResponse?> EditDepartments(DepartmentEditRequest request) {
        var department = await _context.Departments.FindAsync(request.id);

        if(department is null) {
            return null;
        }

        department.Name = request.name;

        await _context.SaveChangesAsync();
        return new DepartmentDataResponse(
            department.Id,
            department.Name
        );
    }

	public async Task <bool> DeleteDepartment(long id) {
        var department = await _context.Departments.FindAsync(id);

        if(department is null) {
            return false;
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return true;
    }

    ApplicationDbContext _context;
}
