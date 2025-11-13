using Core.Entities;

namespace Api.Dtos;

public class MessageCreateRequest: IMessageCreate
{
    public required string Content { get; init; }
    public IResource[]? Attachment { get; init; }
    public DateTime CreatedAt { get; init; }
}