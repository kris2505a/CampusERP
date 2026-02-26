
using Application.Dto;
using Domain.Entity;
using Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Application.Services;


public class StudentService {

    public StudentService(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<List<StudentDataResponse>> GetAllStudents() {

        return await _context.Students
            .Include(s => s.Department)
            .Select(s => new StudentDataResponse(s.RegisterNumber, s.Name, s.Gpa, s.Department.Name))
            .ToListAsync();
    }

    public async Task<StudentDataResponse?> GetStudentByRegNum(long regNum) {
        return await _context.Students
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

    public async Task<StudentDataResponse?> AddStudent(StudentCreationResponse response) {

        var department = await _context.Departments.FindAsync(response.departmentId);

        if(department == null) {
            throw new KeyNotFoundException("Department not found");
        }

        var student = new Student {
            Name = response.name,
            DepartmentId = response.departmentId,
            Department = department
        };
        _context.Students.Add(student);

        await _context.SaveChangesAsync();

        return new StudentDataResponse (
            student.RegisterNumber,
            student.Name,
            student.Gpa,
            student.Department.Name
        );
    }

    public async Task<StudentDataResponse?> EditStudent(StudentEditResponse response) {
        var student = await _context.Students.FindAsync(response.registerNumber) ??
            throw new KeyNotFoundException("Student not found");
        
        var department = await _context.Departments.FindAsync(response.departmentId) ??
            throw new KeyNotFoundException("Department not found");

        student.Name = response.name;
        student.Gpa = response.gpa;
        student.DepartmentId = response.departmentId;
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
        var student = await _context.Students.FindAsync(registerNumber);

        if (student is null)
            return false;

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return true; 
    }

    private readonly ApplicationDbContext _context;
}