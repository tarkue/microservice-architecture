using ChatConnectionLib.ConnectionServices.Dto.UpdateChatsWithUser;
using Dal.Events;
using MassTransit;

namespace Dal.Sagas;

public class UserUpdateSaga : MassTransitStateMachine<UserUpdateSagaState>
{
    public State UpdatingChats { get; private set; } = null!;
    public State Completed { get; private set; } = null!;
    public State Failed { get; private set; } = null!;
    
    public Event<UserUpdateStarted> UpdateStarted { get; private set; } = null!;
    public Event<ChatWithUserUpdate> ChatWithUserUpdated { get; private set; } = null!;
    public Event<UserUpdateFailed> UpdateFailed { get; private set; } = null!;

    public UserUpdateSaga()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => UpdateStarted, e => e.CorrelateById(context => context.Message.UserId));
        Event(() => ChatWithUserUpdated, e => e.CorrelateById(context => context.Message.UserId));
        Event(() => UpdateFailed, e => e.CorrelateById(context => context.Message.UserId));
    
        Initially(
            When(UpdateStarted)
                .Then(context =>
                {
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.CreatedAt = DateTime.UtcNow;
                    context.Saga.Name = context.Message.Name;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.Photo = context.Message.Photo;
                    context.Saga.AccessToken =  context.Message.AccessToken;
                    
                })
                .PublishAsync(context => context.Init<UpdateChatsWithUserRequest>(new UpdateChatsWithUserRequest
                    {
                        AccessToken = context.Saga.AccessToken,
                        Name = context.Saga.Name,
                        Photo = context.Saga.Photo,
                        UserId = context.Saga.UserId,
                        
                    }))
                .TransitionTo(UpdatingChats)
        );

        During(UpdatingChats, When(ChatWithUserUpdated)
            .Then<UserUpdateSagaState, ChatWithUserUpdate>(context =>
            {
                context.Saga.ChatUpdated = context.Message.Success;
                context.Saga.CompletedAt = DateTime.UtcNow;
            })
            .IfElse(context => context.Message.Success,
                then => then
                    .PublishAsync(context => context.Init<UserUpdateCompleted>(
                        new UserUpdateCompleted
                        {
                            UserId = context.Message.UserId,
                            Timestamp = context.Message.Timestamp,
                        }))
                    .TransitionTo(Completed),
                elseThen => elseThen
                    .PublishAsync(context => context.Init<UserUpdateFailed>(
                        new UserUpdateFailed
                        {
                            UserId = context.Message.UserId,
                            Timestamp = context.Message.Timestamp,
                            Reason = "Failed to update chats"
                        }))
                    .TransitionTo(Failed)
            )
        );
        
        DuringAny(
            When(UpdateFailed)
                .Then(context =>
                {
                    context.Saga.FailureReason = context.Message.Reason;
                    context.Saga.CompletedAt = DateTime.UtcNow;
                })
                .TransitionTo(Failed)
        );
        
        SetCompletedWhenFinalized();
    }
}