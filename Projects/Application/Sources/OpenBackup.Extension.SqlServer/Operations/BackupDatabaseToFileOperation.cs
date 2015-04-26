using System;
using System.Data.SqlClient;
using OpenBackup.Extension.FileSystem;
using OpenBackup.Framework;

namespace OpenBackup.Extension.SqlServer.Operations
{
    internal class BackupDatabaseToFileOperation : OperationBase
    {
        public SqlDatabaseObject SqlDatabase
        {
            get;
            private set;
        }

        public IDirectoryObject Directory
        {
            get;
            private set;
        }

        public IFileSystem FileSystem
        {
            get;
            private set;
        }

        public BackupDatabaseToFileOperation(SqlDatabaseObject sqlDatabase, IDirectoryObject directory, IFileSystem fileSystem, IExecutionContext context) : base(context)
        {
            SqlDatabase = sqlDatabase;
            Directory = directory;
            FileSystem = fileSystem;
        }

        public override void ExecuteOperation()
        {
            var destinationDirectory = Directory.CombineDirectory("Databases");

            if (!destinationDirectory.Exists)
                FileSystem.CreateDirectory(destinationDirectory.Path);

            var destinationFile = destinationDirectory.CombineFile(SqlDatabase.Database + ".bak");

            using (var connection = new SqlConnection(SqlDatabase.Server.GetConnectionString()))
            {
                connection.Open();

                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = string.Format(@"BACKUP DATABASE [{0}] TO DISK = '{1}' WITH DESCRIPTION = '{2:yyyy-MM-dd HH:mm:ss}'", SqlDatabase.Database, destinationFile.Path, Context.ServiceContainer.Get<IDateTimeService>().GetCurrentDateTime());
                command.ExecuteNonQuery();
            }
        }

        public bool Equals(BackupDatabaseToFileOperation other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (!SqlDatabase.Equals(other.SqlDatabase))
                return false;

            if (!Directory.Equals(other.Directory))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BackupDatabaseToFileOperation);
        }

        public override int GetHashCode()
        {
            // TODO
            return SqlDatabase.GetHashCode() ^ Directory.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Backup database '{0}' on server '{1}' to '{2}'", SqlDatabase.Database, SqlDatabase.Server.Address, Directory.Path);
        }
    }
}
