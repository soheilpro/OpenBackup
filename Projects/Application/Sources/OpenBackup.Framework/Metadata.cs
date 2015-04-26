using System;

namespace OpenBackup.Framework
{
    public class Metadata : IMetadata
    {
        public string Name
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }

        public Metadata(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
