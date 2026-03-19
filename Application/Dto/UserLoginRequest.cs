namespace Application.Dto;

public record UserLoginRequest (
    string email,
    string password
){}
