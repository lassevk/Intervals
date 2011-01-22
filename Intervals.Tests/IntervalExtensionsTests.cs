using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Intervals.Tests
{
    [TestFixture]
    public class IntervalExtensionsTests
    {
        [TestCase(0, 10, 10)]
        [TestCase(-1, 10, 11)]
        [TestCase(1, 10, 9)]
        [TestCase(0, 11, 11)]
        [TestCase(0, 9, 9)]
        public void GetSpan_Int32Interval_ProducesCorrectValue(int start, int end, int span)
        {
            IInterval<int> interval = Interval.Create(start, end);

            Assert.That(interval.GetSpan(), Is.EqualTo(span));
        }

        [Test]
        public void GetSpan_NullInterval_ThrowsArgumentNullException()
        {
            IInterval<int> interval = null;

            Assert.Throws<ArgumentNullException>(() => interval.GetSpan());
        }
    }
}