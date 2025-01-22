using System.Linq;
using NUnit.Framework;

namespace Intervals.Tests;

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
        Interval<int, bool> interval1 = GetIntervals(intervals1).First();
        Interval<int, bool> interval2 = GetIntervals(intervals2).First();
        Interval<int, bool>? expected = GetIntervals(expectedIntervals).Select(i => (Interval<int, bool>?)i).FirstOrDefault();

        Interval<int, bool>? output = interval1.TryGetOverlappingInterval(interval2, (_, _) => true);

        Assert.That(output, Is.EqualTo(expected));
    }
}