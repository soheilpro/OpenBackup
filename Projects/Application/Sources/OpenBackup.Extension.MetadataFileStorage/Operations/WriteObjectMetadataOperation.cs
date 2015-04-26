using System;
using OpenBackup.Extension.Backup;
using OpenBackup.Framework;

namespace OpenBackup.Extension.MetadataFileStorage
{
    internal class WriteObjectMetadataOperation : OperationBase
    {
        private MetadataFileStorageInstance _storageInstance;

        private IObject _object;

        public WriteObjectMetadataOperation(IObject obj, MetadataFileStorageInstance storageInstance, IBackupContext context) : base(context.ExecutionContext)
        {
            _storageInstance = storageInstance;
            _object = obj;
        }

        public override void ExecuteOperation()
        {
            _storageInstance.Writer.WriteStartElement("Object");
            _storageInstance.Writer.WriteElementString("Type", _object.GetType().Name);

            WriteObjectMetadata(_object as IMetadataProvider);

            _storageInstance.Writer.WriteEndElement();
        }

        private void WriteObjectMetadata(IMetadataProvider obj)
        {
            if (obj == null)
                return;

            foreach (var metadata in obj.GetMetadata())
                _storageInstance.Writer.WriteElementString(metadata.Name, metadata.Value != null ? metadata.Value.ToString() : string.Empty);
        }

        public override string ToString()
        {
            return "Write Metadata for: " + _object;
        }
    }
}
