using MassTransit;

namespace Dal.Sagas;

public class UserUpdateSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = string.Empty;
    
    public Guid UserId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public Guid? Photo { get; set; }
    
    public required string AccessToken { get; set; }
    
    public bool ChatUpdated { get; set; }
    public string? FailureReason { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}