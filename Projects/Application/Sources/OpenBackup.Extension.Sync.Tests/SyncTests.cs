using System;
using System.Linq;
using NUnit.Framework;
using OpenBackup.Runtime;

namespace OpenBackup.Extension.Sync.Tests
{
    [TestFixture]
    public class SyncTests
    {
        [Test]
        public void Sync_LeftToRight()
        {
            var a = TestObject.Create("a", "a");
            var b1 = TestObject.Create("b", "b1");
            var b2 = TestObject.Create("b", "b2");
            var c = TestObject.Create("c", "c");
            var d = TestObject.Create("d", "d");

            var leftContainer = new TestContainer();
            leftContainer.Objects.Add(a);
            leftContainer.Objects.Add(b1);
            leftContainer.Objects.Add(c);

            var rightContainer = new TestContainer();
            rightContainer.Objects.Add(a);
            rightContainer.Objects.Add(b2);
            rightContainer.Objects.Add(d);

            var pair = new Pair(leftContainer, rightContainer);

            var syncJob = new SyncJob();
            syncJob.Pairs.Add(pair);

            var engine = new Engine();
            var actual = engine.RunJob(syncJob).ToArray();

            var expected = new IOperation[]
            {
                new UpdateObjectOperation(b2, b1, new ValueChange(), null),
                new AddObjectOperation(c, null),
                new RemoveObjectOperation(d, null),
            };

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
