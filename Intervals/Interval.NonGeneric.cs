using System;
using System.Numerics;

namespace Intervals;

/// <summary>
/// This static class contains a factory method for easy creation of <see cref="Interval{TBoundary,TTag}"/>.
/// </summary>
public static class Interval
{
    /// <summary>
    /// Constructs a new instance of <see cref="Interval{TBoundary,TTag}"/>.
    /// </summary>
    /// <typeparam name="TBoundary">
    /// The type of boundary value the interval will be based on.
    /// </typeparam>
    /// <param name="start">
    /// The starting value for the new interval.
    /// </param>
    /// <param name="end">
    /// The ending value for the new interval.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="start"/> has a higher value than <paramref name="end"/>.</para>
    /// </exception>
    /// <returns>
    /// The newly constructed interval.
    /// </returns>
    public static Interval<TBoundary, bool> Create<TBoundary>(TBoundary start, TBoundary end)
        where TBoundary : struct, IComparisonOperators<TBoundary, TBoundary, bool>, IComparable<TBoundary>
        => new(start, end, true);

    /// <summary>
    /// Constructs a new instance of <see cref="Interval{TBoundary,TTag}"/>.
    /// </summary>
    /// <typeparam name="TBoundary">
    /// The type of boundary value the interval will be based on.
    /// </typeparam>
    /// <typeparam name="TTag">
    /// The type of tag to associate with the interval.
    /// </typeparam>
    /// <param name="start">
    /// The starting value for the new interval.
    /// </param>
    /// <param name="end">
    /// The ending value for the new interval.
    /// </param>
    /// <param name="tag">
    /// The tag to associate with the interval.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="start"/> has a higher value than <paramref name="end"/>.</para>
    /// </exception>
    /// <returns>
    /// The newly constructed interval.
    /// </returns>
    public static Interval<TBoundary, TTag> Create<TBoundary, TTag>(TBoundary start, TBoundary end, TTag tag)
        where TBoundary : struct, IComparisonOperators<TBoundary, TBoundary, bool>, IComparable<TBoundary>
        => new(start, end, tag);
}