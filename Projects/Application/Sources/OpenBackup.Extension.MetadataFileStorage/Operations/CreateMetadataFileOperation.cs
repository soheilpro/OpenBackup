using System;
using System.Text;
using System.Xml;
using OpenBackup.Extension.Backup;
using OpenBackup.Framework;

namespace OpenBackup.Extension.MetadataFileStorage
{
    internal class CreateMetadataFileOperation : OperationBase
    {
        private MetadataFileStorageInstance _storageInstance;

        public CreateMetadataFileOperation(MetadataFileStorageInstance storageInstance, IBackupContext context) : base(context.ExecutionContext)
        {
            _storageInstance = storageInstance;
        }

        public override void ExecuteOperation()
        {
            _storageInstance.Writer = new XmlTextWriter(_storageInstance.Path, Encoding.Unicode);
            _storageInstance.Writer.Formatting = Formatting.Indented;
            _storageInstance.Writer.WriteStartElement("Objects");
        }

        public override string ToString()
        {
            return "Create Metadata File: " + _storageInstance.Path;
        }
    }
}
