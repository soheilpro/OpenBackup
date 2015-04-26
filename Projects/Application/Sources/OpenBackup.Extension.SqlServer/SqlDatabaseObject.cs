using System;
using System.Collections.Generic;
using OpenBackup.Extension.FileSystem;
using OpenBackup.Extension.SqlServer.Operations;
using OpenBackup.Framework;

namespace OpenBackup.Extension.SqlServer
{
    internal class SqlDatabaseObject : ObjectBase, ISqlDatabaseObject
    {
        public IServer Server
        {
            get;
            private set;
        }

        public string Database
        {
            get;
            private set;
        }

        public SqlDatabaseObject(IServer server, string database)
        {
            Server = server;
            Database = database;
        }

        public IEnumerable<IOperation> Store(IDirectoryObject directory, IFileSystem fileSystem, IExecutionContext context)
        {
            yield return new BackupDatabaseToFileOperation(this, directory, fileSystem, context);
        }

        public bool Equals(SqlDatabaseObject other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (!Server.Equals(other.Server))
                return false;

            if (!Database.Equals(other.Database))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SqlDatabaseObject);
        }

        public override int GetHashCode()
        {
            // TODO
            return Server.GetHashCode() ^ Database.GetHashCode();
        }
    }
}
