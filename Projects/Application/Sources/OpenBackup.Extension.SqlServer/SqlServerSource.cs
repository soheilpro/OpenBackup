using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OpenBackup.Extension.Backup;
using OpenBackup.Framework;

namespace OpenBackup.Extension.SqlServer
{
    [Loadable("SqlServer")]
    public class SqlServerSource : SourceBase
    {
        public IServer Server
        {
            get;
            set;
        }

        public List<string> Databases
        {
            get;
            set;
        }

        public SqlServerSource(IServer server, IEnumerable<string> databases)
        {
            Initialize();

            Server = server;

            if (databases != null)
                Databases.AddRange(databases);
        }

        public SqlServerSource(XElement element, ILoadingContext context) : base(element, context)
        {
            Initialize();

            Server = new Server(element.Element("Server"), context);

            if (element.Element("Databases") != null)
                Databases.AddRange(LoadDatabases(element.Element("Databases"), context));
        }

        private IEnumerable<string> LoadDatabases(XElement element, ILoadingContext context)
        {
            foreach (var databaseElement in element.Elements("Database"))
                yield return databaseElement.Value;
        }

        private void Initialize()
        {
            Databases = new List<string>();
        }

        public override ISourceInstance CreateInstance(IExecutionContext context)
        {
            return new SqlServerSourceInstance(this, context);
        }

        public override XElement ToXml()
        {
            var element = new XElement("SqlServer",
                                       Server.ToXml(),
                                       new XElement("Databases", Databases.Select(database => new XElement("Database",  database))).ToNullIfEmpty());

            return element;
        }
    }
}
