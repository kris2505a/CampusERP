using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;
public class Member {
    [Key]
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }
    public User User { get; set; } = null!;
    public string Name { get; set; } = string.Empty;

    public long DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

}
