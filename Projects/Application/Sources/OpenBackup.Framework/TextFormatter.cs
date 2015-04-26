using System;

namespace OpenBackup.Framework
{
    [ServiceProvider(typeof(ITextFormatter))]
    public class TextFormatter : ITextFormatter
    {
        private IServiceContainer _serviceContainer;

        public TextFormatter(IServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        public string Format(string text)
        {
            var textExpanders = _serviceContainer.GetAll<ITextExpander>();

            foreach (var textExpander in textExpanders)
                text = textExpander.Expand(text);

            return text;
        }
    }
}
