using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Intervals;

/// <summary>
/// Extension methods for <see cref="Interval{TBoundary,TTag}"/> and <see cref="IEnumerable{T}"/> when T is <see cref="Interval{TBoundary,TTag}"/>.
/// </summary>
public static class IntervalExtensions
{
    /// <summary>
    /// Calculates all the slices of the intervals in the collection. Please see the wiki for a more complete explanation of
    /// the slice operation.
    /// </summary>
    /// <typeparam name="TBoundary">
    /// The type of boundary value the intervals are based on.
    /// </typeparam>
    /// <typeparam name="TTag">
    /// The type of tag associated with the intervals.
    /// </typeparam>
    /// <param name="intervals">
    /// The intervals to slice.
    /// </param>
    /// <param name="isAlreadyOrdered">
    /// If it is known that all the intervals are already ordered according to the rule for intervals, then this parameter
    /// can be set to <c>true</c> to avoid an extra sorting step. The default value is <c>false</c> and should be
    /// left as-is if this cannot be guaranteed. It is undocumented and unsupported
    /// to call this method with this parameter as <c>true</c> if the intervals aren't correctly ordered.
    /// </param>
    /// <returns>
    /// A collection of slices for the intervals.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="intervals"/> is <c>null</c>.
    /// </exception>
    public static IEnumerable<Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>> Slice<TBoundary, TTag>(
        this IEnumerable<Interval<TBoundary, TTag>> intervals,
        bool isAlreadyOrdered = false)
        where TBoundary : struct, IComparisonOperators<TBoundary, TBoundary, bool>, IComparable<TBoundary>
    {
        if (intervals == null)
        {
            throw new ArgumentNullException(nameof(intervals));
        }

        if (!isAlreadyOrdered)
        {
            intervals = intervals.OrderBy(interval => interval);
        }

        return new SliceEnumerator<TBoundary, TTag>(intervals);
    }

    /// <summary>
    /// Merges overlapping and possibly adjacent intervals into slices.
    /// </summary>
    /// <typeparam name="TBoundary">
    /// The type of boundary value the intervals are based on.
    /// </typeparam>
    /// <typeparam name="TTag">
    /// The type of tag associated with the intervals.
    /// </typeparam>
    /// <param name="intervals">
    /// The intervals to merge.
    /// </param>
    /// <param name="behavior">
    /// The merge behavior, specifies whether only overlapping intervals should be merged or if also adjacent intervals should be merged.
    /// See the <see cref="IntervalMergeBehavior"/> enum for more information.
    /// </param>
    /// <returns>
    /// A collection of slices containing the merged intervals.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="intervals"/> is <c>null</c>.
    /// </exception>
    public static IEnumerable<Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>> Merge<TBoundary, TTag>(
        this IEnumerable<Interval<TBoundary, TTag>> intervals,
        IntervalMergeBehavior behavior = IntervalMergeBehavior.Default)
        where TBoundary : struct, IComparisonOperators<TBoundary, TBoundary, bool>, IComparable<TBoundary>
    {
        if (intervals == null)
        {
            throw new ArgumentNullException(nameof(intervals));
        }

        return new MergeEnumerator<TBoundary, TTag>(intervals, behavior);
    }

    /// <summary>
    /// Factory extension method on T to create an interval between two boundary values.
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
    /// <returns>
    /// The newly created interval.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="start"/> has a higher value than <paramref name="end"/>.</para>
    /// </exception>
    public static Interval<TBoundary, bool> IntervalTo<TBoundary>(this TBoundary start, TBoundary end)
        where TBoundary : struct, IComparisonOperators<TBoundary, TBoundary, bool>, IComparable<TBoundary>
        => new(start, end, true);

    /// <summary>
    /// Factory extension method on T to create an interval between two boundary values.
    /// </summary>
    /// <typeparam name="TBoundary">
    /// The type of boundary value the interval will be based on.
    /// </typeparam>
    /// <typeparam name="TTag">
    /// The type of boundary value the interval will be based on.
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
    /// <returns>
    /// The newly created interval.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="start"/> has a higher value than <paramref name="end"/>.</para>
    /// </exception>
    public static Interval<TBoundary, TTag> IntervalTo<TBoundary, TTag>(this TBoundary start, TBoundary end, TTag tag)
        where TBoundary : struct, IComparisonOperators<TBoundary, TBoundary, bool>, IComparable<TBoundary>
        => new(start, end, tag);
}