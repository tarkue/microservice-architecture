using Core.Entities;
using Dal;
using Dal.Events;
using Logic.Exceptions;
using MassTransit;

namespace Logic.Consumers;

public class UserUpdateCoordinator(AppDbContext dbContext): IConsumer<UserUpdateStarted>
{
    public async Task Consume(ConsumeContext<UserUpdateStarted> context)
    {
        var user = await dbContext.Users.FindAsync(context.Message.UserId);

        if (user == null)
        {
            await context.Publish(new UserUpdateFailed
            {
                UserId = context.Message.UserId,
                Reason = "User not found",
                Timestamp = DateTime.UtcNow
            });
            return;
        }

        await context.Publish(new ChatWithUserUpdateRequested
        {
            UserId = context.Message.UserId,
            Name = context.Message.Name,
            Photo = context.Message.Photo,
        });


    }
}