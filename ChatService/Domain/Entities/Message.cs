using Core.Entities;

namespace Domain.Entities;

public class Message: IMessage
{
    public Guid Id { get; init; }
    public required string Content { get; init; }
    public required IResource[]? Attachment { get; init; }
    public DateTime CreatedAt { get; init; } 
}