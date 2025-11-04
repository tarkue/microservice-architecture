using Dal;
using Dal.Events;
using Dal.Models;
using Logic.Exceptions;
using MassTransit;

namespace Logic.Consumers;

public class UserUpdateUpdater(AppDbContext dbContext): IConsumer<UserUpdateRequested>
{
    public async Task Consume(ConsumeContext<UserUpdateRequested> context)
    {
        var user = await dbContext.Users.FindAsync(context.Message.UserId);
        
        if (user == null)
        {
            await context.Publish(new UserUpdate
            {
                OldData = context.Message.OldData,
                Success = false,
                Timestamp = DateTime.UtcNow,
                UserId = context.Message.UserId,
                AccessToken = context.Message.AccessToken,
            });
            return;
        }

        dbContext.Users.Update(new UserDal()
        {
            Id = context.Message.UserId,
            Email = context.Message.NewData.Email ?? context.Message.OldData.Email!,
            Name = context.Message.NewData.Name ?? context.Message.OldData.Name!,
            PhotoResourceId = context.Message.NewData.Photo ?? context.Message.OldData.Photo,
            Password = user.Password,
        });
            
        await dbContext.SaveChangesAsync();

        await context.Publish(new UserUpdate
        {
            Success = true,
            UserId = context.Message.UserId,
            Timestamp = DateTime.UtcNow,
            OldData = null,
            AccessToken = null,
        });
    }
}