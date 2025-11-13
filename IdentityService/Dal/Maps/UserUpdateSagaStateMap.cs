using Dal.Sagas;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.Maps;

public class UserUpdateSagaStateMap: SagaClassMap<UserUpdateSagaState>
{
    protected override void Configure(EntityTypeBuilder<UserUpdateSagaState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState).HasMaxLength(64);
        entity.Property(x => x.Name).HasColumnType("text");
        entity.Property(x => x.Email).HasColumnType("text");
        entity.Property(x => x.Photo).HasColumnType("text");
        entity.Property(x => x.AccessToken).HasColumnType("text");
        entity.Property(x => x.FailureReason).HasMaxLength(500);
        
        entity.HasIndex(x => x.UserId).IsUnique();
    }

}