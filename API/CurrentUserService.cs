using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services;

public class CurrentUserService : ICurrentUserService {

    public CurrentUserService(IHttpContextAccessor accessor) : base() {
        _httpContextAccessor = accessor;
    }

    public Guid UserId => Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User id missing"));
    public string Email => User?.FindFirst(ClaimTypes.Email)?.Value ?? throw new Exception("Email is missing");
    public string Role => User?.FindFirst(ClaimTypes.Role)?.Value ?? throw new Exception("role not found");
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    private readonly IHttpContextAccessor _httpContextAccessor;
    private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User ?? throw new Exception("HttpContext or User is null");
}