using System;
using System.Collections.Generic;
using System.Xml;
using OpenBackup.Extension.Backup;
using OpenBackup.Framework;

namespace OpenBackup.Extension.MetadataFileStorage
{
    internal class MetadataFileStorageInstance : StorageInstanceBase
    {
        private MetadataFileStorage _storage;

        internal string Path
        {
            get;
            private set;
        }

        internal XmlTextWriter Writer
        {
            get;
            set;
        }

        public MetadataFileStorageInstance(MetadataFileStorage storage, IExecutionContext context)
        {
            var textFormatter = context.ServiceContainer.Get<ITextFormatter>();
            Path = textFormatter.Format(storage.Path);

            _storage = storage;
        }

        public override IEnumerable<IOperation> Initialize(IBackupContext context)
        {
            yield return new CreateMetadataFileOperation(this, context);
        }

        public override IEnumerable<IOperation> Store(IObject obj, IBackupContext context)
        {
            yield return new WriteObjectMetadataOperation(obj, this, context);
        }

        public override IEnumerable<IOperation> Shutdown(IBackupContext context)
        {
            yield return new CloseMetadataFileOperation(this, context);
        }
    }
}
