using Domain.Enums;

namespace Application.Dto;

public record AuthResponse (
    string token,
    Role role,
    Guid userId
){}
