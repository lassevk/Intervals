using System;

namespace Intervals.Tests;

/// <summary>
/// This class is used to test various value-based comparisons for the intervals.
/// </summary>
public readonly struct NullableComparable : IComparable<NullableComparable>
{
    public NullableComparable(int value)
    {
        Value = value;
    }

    public int Value
    {
        get;
    }

    public int CompareTo(NullableComparable other) => Value.CompareTo(other.Value);
}