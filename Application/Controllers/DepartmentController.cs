using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Application.Dto;

namespace Application.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentController : ControllerBase {
    public DepartmentController(DepartmentServices services) {
        _departmentServices = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartment() {
        var departments = await _departmentServices.GetDepartments();
        if(departments.Count is 0) {
            return Ok("No departments in the database");
        }
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartmentById(long id) {
        var department = await _departmentServices.GetDepartmentById(id);
        if(department is null) {
            return NotFound();
        }
        return Ok(department);
    }

    [HttpPost]
    public async Task<IActionResult> AddDepartment(DepartmentCreationRequest request) {
        var result = await _departmentServices.AddDepartment(request);
        if(result is null) {
            return BadRequest("Department already exists");
        }
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> EditDepartment(DepartmentEditRequest request) {
        var result = await _departmentServices.EditDepartments(request);
        if(result is null) {
            return BadRequest("Department not found");
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(long id) {
        var result = await _departmentServices.DeleteDepartment(id);

        if(result is false) {
            return NotFound();
        }

        return Ok("Deleted Successfully");
    }

    DepartmentServices _departmentServices;
}