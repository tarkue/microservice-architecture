using System.Collections.Concurrent;
using ChatConnectionLib.ConnectionServices.Dto.UpdateChatsWithUser;
using Dal;
using Dal.Events;
using Dal.Models;
using Logic.Helpers;
using Logic.Models;
using MassTransit;

namespace Logic.Consumers;

public class UserUpdateFinalizer(AppDbContext dbContext, UpdateUserPolly updateUserPolly): IConsumer<UserUpdate>, IConsumer<ChatWithUserUpdate>
{
    private readonly ConcurrentDictionary<IdWithTimestamp, bool> _userUpdateStatus = new();
    private readonly ConcurrentDictionary<IdWithTimestamp, bool> _chatUpdateStatus = new();
    
    public async Task Consume(ConsumeContext<UserUpdate> context)
    {
        var idWithTimestamp = new IdWithTimestamp
        {
            Id = context.Message.UserId,
            Timestamp = context.Message.Timestamp,
        };
        var status = _userUpdateStatus.AddOrUpdate(
            idWithTimestamp, 
            context.Message.Success,
            (k, v) => context.Message.Success);
        
        if (status) return;
        
        await updateUserPolly.UpdateUser(new UpdateChatsWithUserRequest()
        {
            UserId = context.Message.UserId,
            AccessToken = context.Message.AccessToken
        });

        await context.Publish(new UserUpdateFailed
        {
            UserId = context.Message.UserId,
            Reason = "User not updated in service IdentityService",
            Timestamp = context.Message.Timestamp,
        });

        _userUpdateStatus.TryRemove(idWithTimestamp, out _);
        _chatUpdateStatus.TryRemove(idWithTimestamp, out _);
    }

    public async Task Consume(ConsumeContext<ChatWithUserUpdate> context)
    {
        var idWithTimestamp = new IdWithTimestamp
        {
            Id = context.Message.UserId,
            Timestamp = context.Message.Timestamp,
        };
        var status = _chatUpdateStatus.AddOrUpdate(
            idWithTimestamp, 
            context.Message.Success,
            (k, v) => context.Message.Success);

        if (status)
        {
            await context.Publish(new UserUpdateCompleted
            {
                UserId = context.Message.UserId,
                Timestamp = context.Message.Timestamp,
            });
        }
        else
        {
            var user = await dbContext.Users.FindAsync(context.Message.UserId);

            if (user == null)
            {
                await context.Publish(new UserUpdateFailed
                {
                    UserId = context.Message.UserId,
                    Reason = "User not found before updating",
                    Timestamp = context.Message.Timestamp,
                });
                return;
            }

            dbContext.Users.Update(new UserDal
            {
                Id = context.Message.UserId,
                Name = context.Message.OldData.Name!,
                Email = context.Message.OldData.Email!,
                PhotoResourceId = context.Message.OldData.Photo,
                Password = user.Password
            });
            await dbContext.SaveChangesAsync();

            await context.Publish(new UserUpdateFailed
            {
                UserId = context.Message.UserId,
                Reason = "User not updated in service ChatService",
                Timestamp = context.Message.Timestamp,
            });
        }
        
        _userUpdateStatus.TryRemove(idWithTimestamp, out _);
        _chatUpdateStatus.TryRemove(idWithTimestamp, out _);
    }
}