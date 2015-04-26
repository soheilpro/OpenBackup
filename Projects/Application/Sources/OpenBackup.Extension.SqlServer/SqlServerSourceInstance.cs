using System;
using System.Collections.Generic;
using OpenBackup.Extension.Backup;
using OpenBackup.Framework;

namespace OpenBackup.Extension.SqlServer
{
    internal class SqlServerSourceInstance : SourceInstanceBase
    {
        private readonly SqlServerSource _source;

        public SqlServerSourceInstance(SqlServerSource source, IExecutionContext context)
        {
            _source = source;
        }

        public override IEnumerable<IObject> GetObjects(IBackupContext context)
        {
            return GetSqlDatabaseObjects(_source.Databases.Count == 0 ? _source.Server.GetAllDatabaseNames() : _source.Databases);
        }

        private IEnumerable<IObject> GetSqlDatabaseObjects(IEnumerable<string> databases)
        {
            foreach (var database in databases)
                yield return new SqlDatabaseObject(_source.Server, database);
        }
    }
}
