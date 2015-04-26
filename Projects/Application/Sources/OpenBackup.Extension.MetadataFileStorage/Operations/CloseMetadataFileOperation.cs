using System;
using OpenBackup.Extension.Backup;
using OpenBackup.Framework;

namespace OpenBackup.Extension.MetadataFileStorage
{
    internal class CloseMetadataFileOperation : OperationBase
    {
        private MetadataFileStorageInstance _storageInstance;

        public CloseMetadataFileOperation(MetadataFileStorageInstance storageInstance, IBackupContext context) : base(context.ExecutionContext)
        {
            _storageInstance = storageInstance;
        }

        public override void ExecuteOperation()
        {
            _storageInstance.Writer.WriteEndElement();
            _storageInstance.Writer.Close();
        }

        public override string ToString()
        {
            return "Close Metadata File: " + _storageInstance.Path;
        }
    }
}
