using System;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Sync.Tests
{
    internal class TestObject<TId, TValue> : ObjectBase
    {
        public TValue Value
        {
            get;
            private set;
        }

        public TestObject(TId id, TValue value) : base(id)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public override bool Equals(object obj)
        {
            return TestObject.AreEqual(this, obj as TestObject<TId, TValue>);
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }
    }

    internal class TestObject
    {
        public static TestObject<TId, TValue> Create<TId, TValue>(TId id, TValue value)
        {
            return new TestObject<TId, TValue>(id, value);
        }

        public static bool AreEqual<TId, TValue>(TestObject<TId, TValue> x, TestObject<TId, TValue> y)
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

            if (!Equals(x.Value, y.Value))
                return false;

            return true;
        }
    }
}
