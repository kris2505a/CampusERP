
using Application.Dto;
using Domain.Entity;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;


public class StudentService {

    public StudentService(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<List<StudentDataResponse>> GetAllStudents() {

        return await _context.Set<Student>()
            .Include(s => s.Department)
            .Select(s => new StudentDataResponse(s.RegisterNumber, s.Name, s.Gpa, s.Department.Name))
            .ToListAsync();
    }

    public async Task<StudentDataResponse?> GetStudentByRegNum(long regNum) {
        return await _context.Set<Student>()
            .Where(s => s.RegisterNumber == regNum)
            .Select(
                s => new StudentDataResponse(
                    s.RegisterNumber,
                    s.Name,
                    s.Gpa,
                    s.Department.Name
                )
            ).FirstOrDefaultAsync();
    }  

    public async Task<StudentDataResponse?> AddStudent(StudentCreationRequest request) {

        var department = await _context.Departments.FindAsync(request.departmentId);

        if(department == null) {
            throw new KeyNotFoundException("Department not found");
        }

        var last = await _context.Set<Student>()
            .OrderByDescending(s => s.RegisterNumber)
            .Select(s => s.RegisterNumber)
            .FirstOrDefaultAsync();

        var student = new Student {
            RegisterNumber = last + 1,
            Name = request.name,
            DepartmentId = request.departmentId,
            Department = department
        };
        _context.Set<Student>().Add(student);

        await _context.SaveChangesAsync();

        return new StudentDataResponse (
            student.RegisterNumber,
            student.Name,
            student.Gpa,
            student.Department.Name
        );
    }

    public async Task<StudentDataResponse?> EditStudent(StudentEditRequest request) {
        var student = await _context.Set<Student>().FindAsync(request.registerNumber) ??
            throw new KeyNotFoundException("Student not found");
        
        var department = await _context.Departments.FindAsync(request.departmentId) ??
            throw new KeyNotFoundException("Department not found");

        student.Name = request.name;
        student.Gpa = request.gpa;
        student.DepartmentId = request.departmentId;
        student.Department = department;

        await _context.SaveChangesAsync();

        return new StudentDataResponse(
            student.RegisterNumber,
            student.Name,
            student.Gpa,
            department.Name
        );
    }
    
    public async Task<bool> DeleteStudent(long registerNumber) {
        var student = await _context.Set<Student>().FirstOrDefaultAsync(s => s.RegisterNumber == registerNumber);

        if (student is null)
            return false;

        _context.Set<Student>().Remove(student);
        await _context.SaveChangesAsync();
        return true; 
    }

    private readonly ApplicationDbContext _context;
}