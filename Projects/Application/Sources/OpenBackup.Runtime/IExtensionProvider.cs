using System;
using System.Collections.Generic;

namespace OpenBackup.Runtime
{
    public interface IExtensionProvider
    {
        IEnumerable<string> GetExtensions();
    }
}
