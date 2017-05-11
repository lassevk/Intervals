using System.Linq;
using NUnit.Framework;

// ReSharper disable TestFileNameWarning
// ReSharper disable AssignNullToNotNullAttribute

namespace Intervals.Tests
{
    [TestFixture]
    public class GetOverlappingIntervalTests : TestCaseTestsBase
    {
        [TestCase(1,
            "....|-------|.....",
            "....|-------|.....",
            "....|-------|.....")]
        [TestCase(2,
            "....|-------|.....",
            "...|-------|......",
            "....|------|......")]
        [TestCase(3,
            "....|-------|.....",
            ".....|-------|....",
            ".....|------|.....")]
        [TestCase(4,
            "....|-------|.....",
            "....|--------|....",
            "....|-------|.....")]
        [TestCase(5,
            "....|-------|.....",
            "...|--------|.....",
            "....|-------|.....")]
        [TestCase(6,
            "....|-------|.....",
            "...|---------|....",
            "....|-------|.....")]
        [TestCase(7,
            "....|-------|.....",
            ".....|-----|......",
            ".....|-----|......")]
        [TestCase(8,
            "....|-------|.....",
            "|---|.............",
            "..................")]
        [TestCase(9,
            "....|-------|.....",
            "|--|..............",
            "..................")]
        [TestCase(10,
            "....|-------|.....",
            "............|----|",
            "..................")]
        [TestCase(11,
            "....|-------|.....",
            ".............|---|",
            "..................")]
        public void TestCase(int testIndex, string intervals1, string intervals2, string expectedIntervals)
        {
            IInterval<int> interval1 = GetIntervals(intervals1).First();
            IInterval<int> interval2 = GetIntervals(intervals2).First();
            IInterval<int> expected = GetIntervals(expectedIntervals).FirstOrDefault();

            Assert.That(interval1.TryGetOverlappingInterval(interval2), Is.EqualTo(expected));
        }
    }
}