namespace Application.Dto;

public record StudentDataResponse(
    long registerNumber,
    string name,
    float gpa,
    string departmentName
) { }