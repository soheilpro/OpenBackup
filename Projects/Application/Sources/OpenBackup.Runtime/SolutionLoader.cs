using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace OpenBackup.Runtime
{
    public class SolutionLoader
    {
        private List<IExtensionProvider> _extensionProviders = new List<IExtensionProvider>();

        public void RegisterExtensionProvider(IExtensionProvider extensionProvider)
        {
            _extensionProviders.Add(extensionProvider);
        }

        public ISolution LoadSolution(string path)
        {
            LoadExtensions();

            var serviceContainer = new AppDomainServiceContainer();
            var context = new LoadingContext(serviceContainer);

            var document = XDocument.Load(path);

            return new Solution(document.Element("Solution"), context);
        }

        private void LoadExtensions()
        {
            foreach (var extensionProvider in _extensionProviders)
            {
                foreach (var extension in extensionProvider.GetExtensions())
                {
                    try
                    {
                        Assembly.LoadFile(extension);

                        DefaultTraceSource.TraceVerbose("Loaded extension: '{0}'", extension);
                    }
                    catch (Exception exception)
                    {
                        DefaultTraceSource.TraceError(exception);
                    }
                }
            }
        }
    }
}
