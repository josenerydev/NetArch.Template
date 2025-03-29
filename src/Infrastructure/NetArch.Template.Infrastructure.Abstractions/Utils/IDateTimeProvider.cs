namespace NetArch.Template.Infrastructure.Abstractions.Utils
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
        DateTime Today { get; }
    }
}
