
using NUnit.Framework;

namespace Unitility.SelectionHistory.Tests
{
    public class SelectionHistoryMangerTests
    {
        [Test]
        public void BackWithEmptyHistoryDoesNotThrow()
        {
            SelectionHistoryManager.Clear();
            SelectionHistoryManager.Back();
        }
        
        [Test]
        public void ForwardWithEmptyHistoryDoesNotThrow()
        {
            SelectionHistoryManager.Clear();
            SelectionHistoryManager.Forward();
        }
    }
}
