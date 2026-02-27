namespace Application.Dto;

public record StudentEditRequest(
    long registerNumber,
    string name,
    float gpa,
    long departmentId
) { }