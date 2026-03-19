using Domain.Enums;

namespace Application.Dto;

public record CreateUserRequest (
    string email, 
    string password, 
    Role role
) {}
