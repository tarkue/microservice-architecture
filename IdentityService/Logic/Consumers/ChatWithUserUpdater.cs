using ChatConnectionLib.ConnectionServices.Interfaces;
using Dal.Events;
using Logic.Helpers;
using MassTransit;

namespace Logic.Consumers;

public abstract class ChatWithUserUpdater(UpdateUserPolly updateUserPolly): IConsumer<ChatWithUserUpdateRequested>
{
    public async Task Consume(ConsumeContext<ChatWithUserUpdateRequested> context)
    {
        try
        {
            await updateUserPolly.UpdateUser(context.Message);
            await context.Publish(new ChatWithUserUpdate
            {
                UserId = context.Message.UserId,
                Success = true,
                Timestamp = context.Message.Timestamp,
                OldData = null
            });
        }
        catch (Exception)
        {
            await context.Publish(new ChatWithUserUpdate
            {
                UserId = context.Message.UserId,
                Success = false,
                Timestamp = context.Message.Timestamp,
                OldData = context.Message.OldData
            });
        }
    }
}