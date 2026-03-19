using System.Security.Claims;
using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase {
    public AuthController(AuthServices service) : base() {
        _authService = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request) {
        var result = await _authService.LoginUser(request);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserRequest request) {
        var result = await _authService.RegisterUser(request);
        
        if(result == false)
        {
            return BadRequest("Failed to link account");
        }
        return Ok();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new {userId, role});
    }

    private readonly AuthServices _authService;
}