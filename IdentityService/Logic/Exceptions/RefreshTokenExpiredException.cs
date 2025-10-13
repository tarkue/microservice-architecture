namespace Logic.Exceptions;

public class RefreshTokenExpiredException(
    string? message = "The refresh token has expired."): Exception(message) {}