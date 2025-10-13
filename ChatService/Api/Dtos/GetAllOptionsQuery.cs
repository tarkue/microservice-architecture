using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Dtos;

public class GetAllOptionsQuery: IGetAllOptions
{
    [FromQuery]
    public short Limit { get; init; }
    [FromQuery]
    public short Page { get; init; }
    [FromQuery]
    public string? Search { get; init; }
}