namespace Logic.Exceptions;

public class NotFoundException(string? entityName = "Entity"): Exception($"{entityName} not found") {}