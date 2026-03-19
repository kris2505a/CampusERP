using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class Student {

    [Key]
    public Guid Id { get; set; }
    public long RegisterNumber { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    [Required]
    public string Name { get; set; } = string.Empty;
    public float Gpa { get; set; } = 0.0f;
    public long DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

}