using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.SqlServer
{
    public interface IServer
    {
        string Address
        {
            get;
        }

        int? Port
        {
            get;
        }

        string Instance
        {
            get;
        }

        string Username
        {
            get;
        }

        string Password
        {
            get;
        }

        XElement ToXml();
    }
}
