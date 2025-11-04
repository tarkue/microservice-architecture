using Dal.Events;
using MassTransit;

namespace Logic.Consumers;

public abstract class ChatWithUserUpdater: IConsumer<ChatWithUserUpdateRequested>
{
    public Task Consume(ConsumeContext<ChatWithUserUpdateRequested> context)
    {
        
    }
}