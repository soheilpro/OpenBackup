using System;

namespace OpenBackup.Extension.Sync
{
    public class ChangeBase : IChange, IEquatable<IChange>
    {
        public bool Equals(IChange other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (GetType() != other.GetType())
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IChange);
        }

        public override int GetHashCode()
        {
            // TODO
            return base.GetHashCode();
        }
    }
}
