using System;
using NUnit.Framework;

namespace Intervals.Tests;

public class TaggedIntervalTests
{
    [Test]
    public void Constructor_WithData_StoresDataIntoProperty()
    {
        var interval = new Interval<int, string>(0, 10, "test");

        Assert.That(interval.Tag, Is.EqualTo("test"));
    }

    [Test]
    public void Create_WithTag_StoresTagIntoProperty()
    {
        var interval = Interval.Create(0, 10, "test");

        Assert.That(interval.Tag, Is.EqualTo("test"));
    }

    [Test]
    public void ToString_ContainsTag()
    {
        string tag = Guid.NewGuid().ToString();
        var interval = Interval.Create(0, 10, tag);

        string output = interval.ToString();

        Assert.That(output, Does.Contain(tag));
    }
}