using System;
using System.Collections.Generic;

namespace OpenBackup.Framework
{
    public interface IMetadataProvider
    {
        IEnumerable<IMetadata> GetMetadata();
    }
}
