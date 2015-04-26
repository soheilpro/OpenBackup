using System;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Sync.Tests
{
    internal class AddObjectOperation : OperationBase
    {
        public IObject Object
        {
            get;
            set;
        }

        public AddObjectOperation(IObject obj, IExecutionContext context) : base(context)
        {
            Object = obj;
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as AddObjectOperation);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override void ExecuteOperation()
        {
        }

        public override string ToString()
        {
            return string.Format("Add: {0}", Object);
        }

        public static bool AreEqual(AddObjectOperation x, AddObjectOperation y)
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

            if (!Equals(x.Object, y.Object))
                return false;

            return true;
        }
    }
}
