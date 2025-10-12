using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Dtos;

public class PaginatedRequest
{
    [FromQuery(Name = "page")] public int? PageIndex { get; set; }
    [FromQuery(Name = "limit")] public int? PageSize { get; set;  }
}

public class PaginatedWithSearchRequest : PaginatedRequest
{
    [FromQuery(Name = "search")] public string? Search { get; set; }
}
