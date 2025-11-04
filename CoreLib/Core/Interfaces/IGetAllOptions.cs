namespace Core.Interfaces;

public interface IGetAllOptions
{
    public short Limit { get; init; }
    public short Page { get; init; }
    public string? Search { get; init; }
}