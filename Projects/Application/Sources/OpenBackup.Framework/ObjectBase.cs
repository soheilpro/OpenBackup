using System;
using System.Collections;

namespace OpenBackup.Framework
{
    public abstract class ObjectBase : IObject
    {
        public virtual object Id
        {
            get;
            private set;
        }

        public Hashtable Metadata
        {
            get;
            private set;
        }

        protected ObjectBase(object id = null)
        {
            Id = id;
            Metadata = new Hashtable();
        }
    }
}
