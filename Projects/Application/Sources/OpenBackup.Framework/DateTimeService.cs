using System;

namespace OpenBackup.Framework
{
    [ServiceProvider(typeof(IDateTimeService))]
    public class DateTimeService : IDateTimeService
    {
        public DateTimeService(IServiceContainer serviceContainer)
        {
        }

        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
