using NetArch.Template.Infrastructure.Abstractions.Utils;

namespace NetArch.Template.Infrastructure.Utils
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Today => DateTime.Today;
    }
}
