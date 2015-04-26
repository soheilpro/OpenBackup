using System;
using System.Collections.Generic;
using OpenBackup.Extension.Backup;
using OpenBackup.Extension.FileSystem;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystemSource
{
    public class FileSystemSourceInstance : SourceInstanceBase
    {
        private FileSystemSource _source;

        public FileSystemSourceInstance(FileSystemSource source, IExecutionContext context)
        {
            _source = source;
        }

        public override IEnumerable<IObject> GetObjects(IBackupContext context)
        {
            var objectProvider = new FileSystemObjectProvider(_source.Roots, _source.Include, _source.Exclude);

            return objectProvider.GetObjects(context.ExecutionContext);
        }
    }
}
