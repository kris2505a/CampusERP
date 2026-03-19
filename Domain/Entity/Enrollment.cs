namespace Domain.Entity;

public class Enrollment {
    public long Id { get; set; }

    
    public Guid StudentId { get; set; }
    public long SubjectCode { get; set; }

    public Student Student { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
}