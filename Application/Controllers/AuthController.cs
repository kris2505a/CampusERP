using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
class AuthController : ControllerBase {
    AuthController(AuthServices service) : base() {
        _authService = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request) {
        var result = await _authService.LoginUser(request);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserRequest request) {
        await _authService.RegisterUser(request);
        return Ok();
    }

    private readonly AuthServices _authService;
}