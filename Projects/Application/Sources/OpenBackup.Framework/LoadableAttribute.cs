using System;

namespace OpenBackup.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LoadableAttribute : Attribute
    {
        public string Name
        {
            get;
            private set;
        }

        public LoadableAttribute(string name)
        {
            Name = name;
        }
    }
}
