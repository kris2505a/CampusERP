namespace Application.Dto;

public record StudentCreationResponse(
    string name,
    long departmentId,
    string departmentName
) { }