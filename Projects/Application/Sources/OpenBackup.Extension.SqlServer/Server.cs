using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.SqlServer
{
    internal class Server : IServer
    {
        public string Address
        {
            get;
            private set;
        }

        public int? Port
        {
            get;
            private set;
        }

        public string Instance
        {
            get;
            private set;
        }

        public string Username
        {
            get;
            private set;
        }

        public string Password
        {
            get;
            private set;
        }

        public Server(string address, int? port, string instance, string username, string password)
        {
            Address = address;
            Port = port;
            Instance = instance;
            Username = username;
            Password = password;
        }

        public Server(XElement element, ILoadingContext context)
        {
            Address = element.Element("Address").Value;

            if (element.Element("Port") != null)
                Port = int.Parse(element.Element("Port").Value);

            if (element.Element("Instance") != null)
                Instance = element.Element("Instance").Value;

            if (element.Element("Username") != null)
                Username = element.Element("Username").Value;

            if (element.Element("Password") != null)
                Password = element.Element("Password").Value;
        }

        public XElement ToXml()
        {
            var element = new XElement("Server",
                                       new XElement("Addresss", Address),
                                       Port != null ? new XElement("Port", Port) : null,
                                       Instance != null ? new XElement("Instance", Instance) : null,
                                       Username != null ? new XElement("Username", Username) : null,
                                       Password != null ? new XElement("Password", Password) : null);

            return element;
        }

        public bool Equals(IServer other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (GetType() != other.GetType())
                return false;

            if (Address != other.Address)
                return false;

            if (Port != other.Port)
                return false;

            if (Instance != other.Instance)
                return false;

            if (Username != other.Username)
                return false;

            if (Password != other.Password)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IServer);
        }

        public override int GetHashCode()
        {
            // TODO
            return Address.GetHashCode() ^ Port.GetHashCode() ^ Instance.GetHashCode() ^ Username.GetHashCode() ^ Password.GetHashCode();
        }
    }
}
