using Dal;
using Dal.Events;
using MassTransit;

namespace Logic.Consumers;

public class UserUpdateCoordinator(AppDbContext dbContext): IConsumer<UserUpdateStarted>
{
    public async Task Consume(ConsumeContext<UserUpdateStarted> context)
    {
        var user = await dbContext.Users.FindAsync(context.Message.UserId);
        var timestamp = DateTime.UtcNow;

        if (user == null)
        {
            await context.Publish(new UserUpdateFailed
            {
                UserId = context.Message.UserId,
                Reason = "User not found",
                Timestamp = timestamp,
            });
            return;
        }

        var oldData = new UserUpdateData
        {
            Email = user.Email,
            Name = user.Name,
            Photo = user.PhotoResourceId,
        };

        await context.Publish(new UserUpdateRequested
        {
            UserId = context.Message.UserId,
            NewData = new UserUpdateData
            {
                Email = context.Message.Email,
                Name = context.Message.Name,
                Photo = context.Message.Photo,
            },
            OldData = oldData,
            Timestamp = timestamp,
            AccessToken = context.Message.AccessToken,
        });

        await context.Publish(new ChatWithUserUpdateRequested
        {
            UserId = context.Message.UserId,
            Name = context.Message.Name,
            Photo = context.Message.Photo,
            AccessToken = context.Message.AccessToken,
            Timestamp = timestamp,
            OldData = oldData
        });
    }
}