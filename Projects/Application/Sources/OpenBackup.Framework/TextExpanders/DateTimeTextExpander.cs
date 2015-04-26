using System;

namespace OpenBackup.Framework.TextExpanders
{
    [ServiceProvider(typeof(ITextExpander))]
    public class DateTimeTextExpander : ITextExpander
    {
        private IServiceContainer _serviceContainer;

        public DateTimeTextExpander(IServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        public string Expand(string text)
        {
            var now = _serviceContainer.Get<IDateTimeService>().GetCurrentDateTime();

            text = text.Replace("%YYYY%", now.Year.ToString("0000"));
            text = text.Replace("%YY%", now.Year.ToString("0000").Substring(2, 2));
            text = text.Replace("%MM%", now.Month.ToString("00"));
            text = text.Replace("%M%", now.Month.ToString("0"));
            text = text.Replace("%DD%", now.Day.ToString("00"));
            text = text.Replace("%D%", now.Day.ToString("0"));
            text = text.Replace("%HH%", now.Hour.ToString("00"));
            text = text.Replace("%H%", now.Hour.ToString("0"));
            text = text.Replace("%mm%", now.Minute.ToString("00"));
            text = text.Replace("%m%", now.Minute.ToString("0"));
            text = text.Replace("%SS%", now.Second.ToString("00"));
            text = text.Replace("%S%", now.Second.ToString("0"));

            return text;
        }
    }
}
