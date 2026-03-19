using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;


[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase {

    public StudentController(StudentService service) {
        _studentService = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get() {
        var students = await _studentService.GetAllStudents();
        
        return Ok(students);
    }

    [HttpGet("{regNum}")]
    public async Task<IActionResult> GetByRegisterNumber(long regNum) {
        var student = await _studentService.GetStudentByRegNum(regNum);
        if(student == null) {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPost]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddStudent(StudentCreationRequest response) {

        try {
            var result = await _studentService.AddStudent(response);
            return Ok(result);
        }

        catch(KeyNotFoundException e) {
            return NotFound(e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> EditStudent(StudentEditRequest response) {
        try {
            var result = await _studentService.EditStudent(response);
            return Ok(result);
        }
        catch(KeyNotFoundException e) {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{registerNumber}")]
    public async Task<IActionResult> Delete(long registerNumber) {
        var result = await _studentService.DeleteStudent(registerNumber);

        if(result is false) {
            return NotFound();
        }

        return Ok("Deleted");
    }

    private StudentService _studentService;
}
