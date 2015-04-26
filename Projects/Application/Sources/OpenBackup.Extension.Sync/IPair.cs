using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.Sync
{
    public interface IPair
    {
        IContainer Left
        {
            get;
            set;
        }

        IContainer Right
        {
            get;
            set;
        }

        IPairInstance CreateInstance(IExecutionContext context);

        XElement ToXml();
    }
}
