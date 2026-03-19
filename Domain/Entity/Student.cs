using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class Student : Member {

    public long RegisterNumber { get; set; }
    [Required]
    public float Gpa { get; set; } = 0.0f;

}