using System;
using System.Collections;

namespace OpenBackup.Framework
{
    public interface IObject
    {
        /// <summary>
        /// TODO: Separate?
        /// </summary>
        object Id
        {
            get;
        }

        /// <summary>
        /// TODO: Separate?
        /// </summary>
        Hashtable Metadata
        {
            get;
        }
    }
}
