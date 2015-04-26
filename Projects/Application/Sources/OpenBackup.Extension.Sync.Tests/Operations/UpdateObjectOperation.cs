using System;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Sync.Tests
{
    internal class UpdateObjectOperation : OperationBase
    {
        public IObject Object
        {
            get;
            set;
        }

        public IObject BaseObject
        {
            get;
            set;
        }

        public IChange Change
        {
            get;
            set;
        }

        public UpdateObjectOperation(IObject obj, IObject baseObject, IChange change, IExecutionContext context) : base(context)
        {
            Object = obj;
            BaseObject = baseObject;
            Change = change;
        }

        public override void ExecuteOperation()
        {
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as UpdateObjectOperation);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("Update: {0} <- {1} ({2})", Object, BaseObject, Change);
        }

        public static bool AreEqual(UpdateObjectOperation x, UpdateObjectOperation y)
        {
            // Both are null, or both are same instance?
            if (ReferenceEquals(x, y))
                return true;

            // x is null?
            if (ReferenceEquals(x, null))
                return false;

            // y is null?
            if (ReferenceEquals(y, null))
                return false;

            if (!Equals(x.BaseObject, y.BaseObject))
                return false;

            if (!Equals(x.Object, y.Object))
                return false;

            if (!Equals(x.Change, y.Change))
                return false;

            return true;
        }
    }
}
