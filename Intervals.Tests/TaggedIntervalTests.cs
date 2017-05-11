using System;
using NUnit.Framework;

namespace Intervals.Tests
{
    using System.Diagnostics.CodeAnalysis;

    public class TaggedIntervalTests
    {
        [Test]
        public void Constructor_WithData_StoresDataIntoProperty()
        {
            var interval = new TaggedInterval<int>(0, 10, "test");

            Assert.That(interval.Tag, Is.EqualTo("test"));
        }

        [Test]
        public void Create_WithTag_StoresTagIntoProperty()
        {
            var interval = TaggedInterval.Create(0, 10, "test");

            Assert.That(interval.Tag, Is.EqualTo("test"));
        }

        [Test]
        public void ToString_ContainsTag()
        {
            var tag = Guid.NewGuid().ToString();
            var interval = TaggedInterval.Create(0, 10, tag);

            var output = interval.ToString();

            Assert.That(output, Is.StringContaining(tag));
        }
    }
}