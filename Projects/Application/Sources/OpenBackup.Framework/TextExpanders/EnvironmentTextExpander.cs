using System;

namespace OpenBackup.Framework.TextExpanders
{
    [ServiceProvider(typeof(ITextExpander))]
    public class EnvironmentTextExpander : ITextExpander
    {
        public EnvironmentTextExpander(IServiceContainer serviceContainer)
        {
        }

        public string Expand(string text)
        {
            return Environment.ExpandEnvironmentVariables(text);
        }
    }
}
