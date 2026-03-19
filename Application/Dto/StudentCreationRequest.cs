namespace Application.Dto;

public record StudentCreationRequest(
    string name,
    long departmentId,
    string departmentName
) { }