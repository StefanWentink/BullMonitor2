namespace SWE.Time.Entities
{
    public record struct Range(
        DateTimeOffset From,
        DateTimeOffset Until)
    { }
}