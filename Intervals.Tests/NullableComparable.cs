using System;
using System.Numerics;

namespace Intervals.Tests;

/// <summary>
/// This class is used to test various value-based comparisons for the intervals.
/// </summary>
public readonly struct NullableComparable : IComparable<NullableComparable>, IComparisonOperators<NullableComparable,NullableComparable,bool>
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
    public static bool operator ==(NullableComparable left, NullableComparable right) => left.Value == right.Value;

    public static bool operator !=(NullableComparable left, NullableComparable right) => left.Value != right.Value;

    public static bool operator >(NullableComparable left, NullableComparable right) => left.Value > right.Value;

    public static bool operator >=(NullableComparable left, NullableComparable right) => left.Value >= right.Value;

    public static bool operator <(NullableComparable left, NullableComparable right) => left.Value < right.Value;

    public static bool operator <=(NullableComparable left, NullableComparable right) => left.Value <= right.Value;
}