namespace Application.Dto;

public record StudentEditResponse(
    long registerNumber,
    string name,
    float gpa,
    long departmentId
) { }