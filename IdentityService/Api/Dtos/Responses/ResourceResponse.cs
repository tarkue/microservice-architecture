using Dal.Models;
using Domain.Entities;
using Domain.Enums;

namespace IdentityService.Dtos.Responses;

public record ResourceResponse: IResource
{
    public Guid Id { get; init; }
    public required string Source { get; init; }
    public ResourceType Type { get; init; }

    public static ResourceResponse FromDal(IResource resource)
    {
        return new ResourceResponse()
        {
            Id = resource.Id,
            Source = resource.Source,
            Type = resource.Type,
        };
    }
}