namespace Logic.Exceptions;

public class ChatNotFoundException(string? message = "Chat not found"): Exception(message) {}