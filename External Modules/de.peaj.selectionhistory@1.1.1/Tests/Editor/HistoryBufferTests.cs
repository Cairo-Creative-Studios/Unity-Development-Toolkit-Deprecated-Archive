using NUnit.Framework;

namespace Unitility.SelectionHistory.Tests
{
    public class HistoryBufferTests
    {
        [Test]
        public void Wrap()
        {
            var buffer = new HistoryBuffer<int>(3);
            buffer.Push(1);
            buffer.Push(2);
            buffer.Push(3);
            buffer.Push(4);

            var prev = buffer.Previous();
            Assert.AreEqual(3, prev);
            prev = buffer.Previous();
            Assert.AreEqual(2, prev);
            prev = buffer.Previous();
            Assert.AreEqual(2, prev);
        }

        [Test]
        public void GetPreviousThenNext()
        {
            var buffer = new HistoryBuffer<int>(3);
            buffer.Push(1);
            buffer.Push(2);
            buffer.Push(3);
            buffer.Push(4);

            var item = buffer.Previous();
            Assert.AreEqual(3, item);
            item = buffer.Previous();
            Assert.AreEqual(2, item);
            item = buffer.Next();
            Assert.AreEqual(3, item);
            item = buffer.Next();
            Assert.AreEqual(4, item);
        }

        [Test]
        public void OverwriteFuture()
        {
            var buffer = new HistoryBuffer<int>(3);
            buffer.Push(1);
            buffer.Push(2);
            buffer.Push(3);
            buffer.Push(4);

            var item = buffer.Previous();
            Assert.AreEqual(3, item);
            item = buffer.Previous();
            Assert.AreEqual(2, item);
            item = buffer.Next();

            buffer.Push(10);
            buffer.Push(11);

            item = buffer.Next();
            Assert.AreEqual(11, item);
            item = buffer.Previous();
            Assert.AreEqual(10, item);
        }

        [Test]
        public void CreateFromNullArray()
        {
            var history = HistoryBuffer<int>.FromArray(null, 2, 5);
            Assert.AreEqual(0, history.Size);
        }
    }
}