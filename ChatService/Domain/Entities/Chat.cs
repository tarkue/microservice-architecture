using Core.Entities;

namespace Domain.Entities;

public class Chat: IChat
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public short UnreadMessagesCount { get; init; }
}