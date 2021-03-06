﻿using System;
using System.Linq;
using NUnit.Framework;

// ReSharper disable TestFileNameWarning
// ReSharper disable AssignNullToNotNullAttribute

namespace Intervals.Tests
{
    [TestFixture]
    public class IsOverlappingTests : TestCaseTestsBase
    {
        [TestCase(1,
            "....|-------|.....",
            "....|-------|.....", true)]
        [TestCase(2,
            "....|-------|.....",
            "...|-------|......", true)]
        [TestCase(3,
            "....|-------|.....",
            ".....|-------|....", true)]
        [TestCase(4,
            "....|-------|.....",
            "....|--------|....", true)]
        [TestCase(5,
            "....|-------|.....",
            "...|--------|.....", true)]
        [TestCase(6,
            "....|-------|.....",
            "...|---------|....", true)]
        [TestCase(7,
            "....|-------|.....",
            ".....|-----|......", true)]
        [TestCase(8,
            "....|-------|.....",
            "|---|.............", false)]
        [TestCase(9,
            "....|-------|.....",
            "|--|..............", false)]
        [TestCase(10,
            "....|-------|.....",
            "............|----|", false)]
        [TestCase(11,
            "....|-------|.....",
            ".............|---|", false)]
        public void TestCase(int testIndex, string intervals1, string intervals2, bool expected)
        {
            IInterval<int> interval1 = GetIntervals(intervals1).First();
            IInterval<int> interval2 = GetIntervals(intervals2).First();

            Assert.That(interval1.IsOverlapping(interval2), Is.EqualTo(expected));
        }
    }
}