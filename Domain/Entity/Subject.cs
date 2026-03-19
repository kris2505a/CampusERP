using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class Subject {
    [Key]
    public long Code { get; set; }
    public string? Name { get; set; }
}