using System;
using OpenBackup.Extension.FileSystem;
using OpenBackup.Framework;

namespace OpenBackup.Extension.SqlServer
{
    internal interface ISqlDatabaseObject : IObject, IFileSystemStorableObject
    {
        IServer Server
        {
            get;
        }

        string Database
        {
            get;
        }
    }
}
