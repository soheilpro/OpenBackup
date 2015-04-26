using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenBackup
{
    public interface ISolution
    {
        List<IJob> Jobs
        {
            get;
        }

        XElement ToXml();
    }
}
