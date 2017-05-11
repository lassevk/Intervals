using System;
using System.Collections.Generic;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace Intervals.Tests
{
    [TestFixture]
    public class SliceTests
    {
        [Test]
        public void Constructor_StartAfterEnd_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Slice<int>(10, 5, new List<IInterval<int>>()));
        }

        [Test]
        public void Constructor_NullintervalsInSlice_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Slice<int>(5, 10, null));
        }
    }
}
