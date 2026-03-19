namespace Domain.Entity;
using Domain.Enums;

public class User {
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateCreated { get; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
}