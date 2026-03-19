using Application.Dto;
using Domain.Entity;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class AuthServices {

    public AuthServices(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task RegisterUser(CreateUserRequest request)
    {
        var pswHash = BCrypt.Net.BCrypt.HashPassword(request.password);
        var user = new User
        {
            Email = request.email,
            PasswordHash = pswHash,
            Role = request.role
        };
    }

    public async Task<AuthResponse> LoginUser(UserLoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.Email == request.email
        );

        if (user is null)
        {
            throw new Exception("Invalid Credentials");
        }

        var valid = BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash);

        if (!valid)
        {
            throw new Exception("Invalid Credentials");
        }

        string newToken = string.Empty;

        return new AuthResponse (
            newToken,
            user.Role,
            user.Id
        );

    }


    private ApplicationDbContext _context;
}
