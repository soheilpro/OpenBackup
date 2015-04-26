using System;
using System.Collections.Generic;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Backup
{
    public class BackupJobInstance : JobInstanceBase
    {
        private BackupJob _job;
        private List<ISourceInstance> _sources = new List<ISourceInstance>();
        private List<IStorageInstance> _storages = new List<IStorageInstance>();

        public override IJob Job
        {
            get
            {
                return _job;
            }
        }

        public BackupJobInstance(BackupJob job, IExecutionContext context)
        {
            _job = job;

            _sources.AddRange(InstantiateSources(context));
            _storages.AddRange(InstantiateStorages(context));
        }

        private IEnumerable<ISourceInstance> InstantiateSources(IExecutionContext context)
        {
            foreach (var source in _job.Sources)
                yield return source.CreateInstance(context);
        }

        private IEnumerable<IStorageInstance> InstantiateStorages(IExecutionContext context)
        {
            foreach (var storage in _job.Storages)
                yield return storage.CreateInstance(context);
        }

        public override IEnumerable<IOperation> RunJob(IExecutionContext context)
        {
            var backupContext = new BackupContext(this, context);

            // Initialize Sources
            foreach (var source in _sources)
                foreach (var operation in source.Initialize(backupContext))
                    yield return operation;

            // Initialize Storages
            foreach (var storage in _storages)
                foreach (var operation in storage.Initialize(backupContext))
                    yield return operation;

            foreach (var source in _sources)
                foreach (var obj in source.GetObjects(backupContext))
                    foreach (var storage in _storages)
                        foreach (var operation in storage.Store(obj, backupContext))
                            yield return operation;

            // Shutdown Sources
            foreach (var source in _sources)
                foreach (var operation in source.Shutdown(backupContext))
                    yield return operation;

            // Shutdown Storages
            foreach (var storage in _storages)
                foreach (var operation in storage.Shutdown(backupContext))
                    yield return operation;
        }
    }
}
