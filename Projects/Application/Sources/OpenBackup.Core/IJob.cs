using System;
using System.Xml.Linq;

namespace OpenBackup
{
    public interface IJob
    {
        string Name
        {
            get;
        }

        bool IsEnabled
        {
            get;
        }

        IJobInstance CreateInstance(IExecutionContext context);

        XElement ToXml();
    }
}
