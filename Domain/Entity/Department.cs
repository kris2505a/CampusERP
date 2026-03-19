using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class Department {
    public long Id { get; set; }
    [Required]
    [MinLength(20)]
    public string Name { get; set; } = string.Empty;
    public ICollection<Member> Members { get; set; } = new List<Member>();
}