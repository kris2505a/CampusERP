using Application.Dto;
using Domain.Entity;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Application.Services;
public class AuthServices {

    public AuthServices(ApplicationDbContext context, StudentService service, TokenService tokenService)
    {
        _context = context;
        _studentService = service;
        _tokenService = tokenService;
    }
    public async Task<bool> RegisterUser(CreateUserRequest request) {

        if(await _context.Users.AnyAsync(u => u.Email == request.email))
        {
            return false;
        }

        var pswHash = BCrypt.Net.BCrypt.HashPassword(request.password);

        Member? member = null;

        if (request.role == Role.Student)
        {
            member = await _context.Set<Student>().FirstOrDefaultAsync(s => s.RegisterNumber == request.id);
        }

        else
        {
            //for staff
        }

        if(member == null || member.UserId != null)
        {
            return false;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.email,
            PasswordHash = pswHash,
            Role = request.role
        };

        if (member == null)
        {
            return false;
        }

        await _context.Users.AddAsync(user);
        member.UserId = user.Id;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<AuthResponse> LoginUser(UserLoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.Email == request.email
        );

        if (user == null)
        {
            throw new Exception("Invalid Credentials");
        }

        var valid = BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash);

        if (!valid)
        {
            throw new Exception("Invalid Credentials");
        }

        string newToken = _tokenService.Generate(user);

        return new AuthResponse (
            newToken,
            user.Role,
            user.Id
        );

    }

    public async Task<StudentDataResponse?> CurrentStudentData() {
        return await _studentService.GetCurrentStudent();
    }


    private ApplicationDbContext _context;
    private StudentService _studentService;
    private readonly TokenService _tokenService;
}
