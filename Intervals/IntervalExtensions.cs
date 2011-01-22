using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals
{
    /// <summary>
    /// This class holds extension methods for <see cref="IInterval{T}"/> and
    /// <see cref="IInterval{T,TData}"/>.
    /// </summary>
    public static class IntervalExtensions
    {
        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static long GetSpan(this IInterval<int> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - (long)interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static byte GetSpan(this IInterval<byte> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return (byte)(interval.End - interval.Start);
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static long GetSpan(this IInterval<long> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static int GetSpan(this IInterval<short> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static double GetSpan(this IInterval<double> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static float GetSpan(this IInterval<float> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static decimal GetSpan(this IInterval<decimal> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static TimeSpan GetSpan(this IInterval<DateTime> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified <paramref name="value"/>
        /// is considered to be a part of the specified <paramref name="interval"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension for the interval.
        /// </typeparam>
        /// <param name="interval">
        /// The interval to check the <paramref name="value"/> against.
        /// </param>
        /// <param name="value">
        /// The value to check against the <paramref name="interval"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="value"/> is considered to be a part of the <paramref name="interval"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Note that the <see cref="IInterval{T}.End"/> value is not considered to be a part of the interval.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static bool Contains<T>(this IInterval<T> interval, T value) where T : IComparable<T>
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.Start.CompareTo(value) <= 0 && interval.End.CompareTo(value) > 0;
        }

        /// <summary>
        /// Returns <c>true</c> if the two intervals overlap, meaning that at least one value exists
        /// that is considered to be part of both intervals.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension for the intervals.
        /// </typeparam>
        /// <param name="interval1">
        /// The first interval, to compare against <paramref name="interval2"/>.
        /// </param>
        /// <param name="interval2">
        /// The second interval, to compare against <paramref name="interval1"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the two intervals overlaps;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="interval2"/> is <c>null</c>.</para>
        /// </exception>
        public static bool IsOverlapping<T>(this IInterval<T> interval1, IInterval<T> interval2) where T : IComparable<T>
        {
            if (interval1 == null)
                throw new ArgumentNullException("interval1");
            if (interval2 == null)
                throw new ArgumentNullException("interval2");

            return interval1.Start.CompareTo(interval2.End) < 0 && interval1.End.CompareTo(interval2.Start) > 0;
        }

        /// <summary>
        /// Gets the overlapping interval that is common between the two intervals,
        /// or <c>null</c> if the two doesn't overlap.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension for the intervals.
        /// </typeparam>
        /// <param name="interval1">
        /// The first interval, to compare against <paramref name="interval2"/>.
        /// </param>
        /// <param name="interval2">
        /// The second interval, to compare against <paramref name="interval1"/>.
        /// </param>
        /// <returns>
        /// <c>null</c> if the two intervals doesn't overlap;
        /// otherwise, a new <see cref="Interval{T}"/> that maps to the common region between the two.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="interval2"/> is <c>null</c>.</para>
        /// </exception>
        public static IInterval<T> GetOverlappingInterval<T>(this IInterval<T> interval1, IInterval<T> interval2) where T : IComparable<T>
        {
            if (interval1 == null)
                throw new ArgumentNullException("interval1");
            if (interval2 == null)
                throw new ArgumentNullException("interval2");

            T start = interval1.Start;
            if (interval2.Start.CompareTo(start) > 0)
                start = interval2.Start;

            T end = interval1.End;
            if (interval2.End.CompareTo(end) < 0)
                end = interval2.End;

            if (start.CompareTo(end) >= 0)
                return null;

            return new Interval<T>(start, end);
        }

        /// <summary>
        /// Produces an ordered collection of the intervals, by sorting them according to the normal
        /// interval sorting order. See <see cref="IntervalComparer{T}.Default"/> for a definition of what that
        /// sort order is.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="IInterval{T}.Start"/> and
        /// <see cref="IInterval{T}.End"/> properties.
        /// </typeparam>
        /// <param name="collection">
        /// The collection of intervals to sort.
        /// </param>
        /// <returns>
        /// A sorted collection of intervals.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        public static IEnumerable<IInterval<T>> Ordered<T>(this IEnumerable<IInterval<T>> collection) where T : IComparable<T>
        {
            return collection.OrderBy(r => r, IntervalComparer<T>.Default);
        }

        /// <summary>
        /// Produces an ordered collection of the intervals, by sorting them according to the normal
        /// interval sorting order. See <see cref="IntervalComparer{T}.Default"/> for a definition of what that
        /// sort order is.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="IInterval{T}.Start"/> and
        /// <see cref="IInterval{T}.End"/> properties.
        /// </typeparam>
        /// <typeparam name="TData">
        /// The type of data to associate with the interval.
        /// </typeparam>
        /// <param name="collection">
        /// The collection of intervals to sort.
        /// </param>
        /// <returns>
        /// A sorted collection of intervals.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        public static IEnumerable<IInterval<T, TData>> Ordered<T, TData>(this IEnumerable<IInterval<T, TData>> collection) where T : IComparable<T>
        {
            return collection.OrderBy(r => r, IntervalComparer<T>.Default);
        }

        /// <summary>
        /// Slices up the intervals, which has to be sorted. See remarks for what the "Slice" operation really does.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="IInterval{T}.Start"/> and
        /// <see cref="IInterval{T}.End"/> properties.
        /// </typeparam>
        /// <param name="intervals">
        /// The intervals to slice.
        /// </param>
        /// <returns>
        /// A collection of slices.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="intervals"/> is <c>null</c>.</para>
        /// </exception>
        /// <remarks>
        /// <para>Note that the intervalss must be sorted according to normal interval sort order.</para>
        /// <para>Slicing means to look at all the intervals as though they had been drawn on a timeline, find all
        /// overlapping intervals, and produce slices, where each slice contains one or more source intervals, and all the source
        /// intervals cover that slice totally.</para>
        /// <para>As an example, consider the two intervals A=0..10 and B=5..15. These would produce 3 slices. The first slice
        /// would be 0..5, and be associated with only the A slice. The second slice would be 5..10, and be
        /// associated with both A and B (since they both totally cover that area). The third and final slice would be
        /// 10..15 and only be associated with B.</para>
        /// </remarks>
        public static IEnumerable<IInterval<T, IInterval<T>[]>> Slice<T>(this IEnumerable<IInterval<T>> intervals) where T : IComparable<T>
        {
            if (intervals == null)
                throw new ArgumentNullException("intervals");

            return InternalSlice<T, IInterval<T>>(intervals);
        }

        /// <summary>
        /// Slices up the intervals, which has to be sorted. See remarks for what the "Slice" operation really does.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="IInterval{T}.Start"/> and
        /// <see cref="IInterval{T}.End"/> properties.
        /// </typeparam>
        /// <typeparam name="TData">
        /// The type of data to associate with the interval.
        /// </typeparam>
        /// <param name="intervals">
        /// The intervals to slice.
        /// </param>
        /// <returns>
        /// A collection of slices.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="intervals"/> is <c>null</c>.</para>
        /// </exception>
        /// <remarks>
        /// <para>Note that the intervals must be sorted according to normal interval sort order.</para>
        /// <para>Slicing means to look at all the intervals as though they had been drawn on a timeline, find all
        /// overlapping intervals, and produce slices, where each slice contains one or more source intervals, and all the source
        /// intervals cover that slice totally.</para>
        /// <para>As an example, consider the two intervals A=0..10 and B=5..15. These would produce 3 slices. The first slice
        /// would be 0..5, and be associated with only the A slice. The second slice would be 5..10, and be
        /// associated with both A and B (since they both totally cover that area). The third and final slice would be
        /// 10..15 and only be associated with B.</para>
        /// </remarks>
        public static IEnumerable<IInterval<T, IInterval<T, TData>[]>> Slice<T, TData>(this IEnumerable<IInterval<T, TData>> intervals)
            where T : IComparable<T>
        {
            if (intervals == null)
                throw new ArgumentNullException("intervals");

            return InternalSlice<T, IInterval<T, TData>>(intervals);
        }

        /// <summary>
        /// Merge overlapping, and optionally adjacent, intervals into one interval, with associations back to the
        /// original intervals, using the default merge behavior.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="IInterval{T}.Start"/> and
        /// <see cref="IInterval{T}.End"/> properties.
        /// </typeparam>
        /// <param name="intervals">
        /// The intervals to merge.
        /// </param>
        /// <returns>
        /// A collection of merged intervals.
        /// </returns>
        /// <remarks>
        /// <para>Note that the intervals must be sorted according to normal interval sort order.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="intervals"/> is <c>null</c>.</para>
        /// </exception>
        public static IEnumerable<IInterval<T, IInterval<T>[]>> Merge<T>(this IEnumerable<IInterval<T>> intervals) where T : IComparable<T>
        {
            return Merge(intervals, IntervalMergeBehavior.Default);
        }

        /// <summary>
        /// Merge overlapping, and optionally adjacent, intervals into one interval, with associations back to the
        /// original intervals.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="IInterval{T}.Start"/> and
        /// <see cref="IInterval{T}.End"/> properties.
        /// </typeparam>
        /// <param name="intervals">
        /// The intervals to merge.
        /// </param>
        /// <param name="behavior">
        /// The merge behavior, see <see cref="IntervalMergeBehavior"/> for more information.
        /// </param>
        /// <returns>
        /// A collection of merged intervals.
        /// </returns>
        /// <remarks>
        /// <para>Note that the intervals must be sorted according to normal interval sort order.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="intervals"/> is <c>null</c>.</para>
        /// </exception>
        public static IEnumerable<IInterval<T, IInterval<T>[]>> Merge<T>(this IEnumerable<IInterval<T>> intervals, IntervalMergeBehavior behavior)
            where T : IComparable<T>
        {
            if (intervals == null)
                throw new ArgumentNullException("intervals");

            return InternalMerge<T, IInterval<T>>(intervals, behavior);
        }

        /// <summary>
        /// Merge overlapping, and optionally adjacent, intervals into one interval, with associations back to the
        /// original intervals, using the default merge behavior.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="IInterval{T}.Start"/> and
        /// <see cref="IInterval{T}.End"/> properties.
        /// </typeparam>
        /// <typeparam name="TData">
        /// The type of data to associate with the interval.
        /// </typeparam>
        /// <param name="intervals">
        /// The intervals to merge.
        /// </param>
        /// <returns>
        /// A collection of merged intervals.
        /// </returns>
        /// <remarks>
        /// <para>Note that the intervals must be sorted according to normal interval sort order.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="intervals"/> is <c>null</c>.</para>
        /// </exception>
        public static IEnumerable<IInterval<T, IInterval<T, TData>[]>> Merge<T, TData>(this IEnumerable<IInterval<T, TData>> intervals)
            where T : IComparable<T>
        {
            return Merge(intervals, IntervalMergeBehavior.Default);
        }

        /// <summary>
        /// Merge overlapping, and optionally adjacent, intervals into one interval, with associations back to the
        /// original intervals.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="IInterval{T}.Start"/> and
        /// <see cref="IInterval{T}.End"/> properties.
        /// </typeparam>
        /// <typeparam name="TData">
        /// The type of data to associate with the interval.
        /// </typeparam>
        /// <param name="intervals">
        /// The intervals to merge.
        /// </param>
        /// <param name="behavior">
        /// The merge behavior, see <see cref="IntervalMergeBehavior"/> for more information.
        /// </param>
        /// <returns>
        /// A collection of merged intervals.
        /// </returns>
        /// <remarks>
        /// <para>Note that the intervals must be sorted according to normal interval sort order.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="intervals"/> is <c>null</c>.</para>
        /// </exception>
        public static IEnumerable<IInterval<T, IInterval<T, TData>[]>> Merge<T, TData>(
            this IEnumerable<IInterval<T, TData>> intervals, IntervalMergeBehavior behavior) where T : IComparable<T>
        {
            if (intervals == null)
                throw new ArgumentNullException("intervals");

            return InternalMerge<T, IInterval<T, TData>>(intervals, behavior);
        }

        /// <summary>
        /// Reduces the associated data by running it through a reduction function, returning the reduced results,
        /// typically used to convert an array of intervals (from the slice function) into a number or other type
        /// of value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="IInterval{T}.Start"/> and
        /// <see cref="IInterval{T}.End"/> properties.
        /// </typeparam>
        /// <typeparam name="TInput">
        /// The type of data to associate with the input intervals.
        /// </typeparam>
        /// <typeparam name="TOutput">
        /// The type of data to associate with the output intervals.
        /// </typeparam>
        /// <param name="intervals">
        /// The intervals to reduce.
        /// </param>
        /// <param name="converter">
        /// A <see cref="Converter{T1,T2}"/> that will converter the data from the input intervals in order to produce
        /// the data to associate with the output intervals.
        /// </param>
        /// <returns>
        /// A collection of new interval objects with the reduced data, but same start and end points.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="intervals"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="converter"/> is <c>null</c>.</para>
        /// </exception>
        public static IEnumerable<IInterval<T, TOutput>> Reduce<T, TInput, TOutput>(
            this IEnumerable<IInterval<T, TInput>> intervals, Converter<TInput, TOutput> converter) where T : IComparable<T>
        {
            if (intervals == null)
                throw new ArgumentNullException("intervals");
            if (converter == null)
                throw new ArgumentNullException("converter");

            return InternalReduce(intervals, converter);
        }

        private static IEnumerable<IInterval<T, R[]>> InternalSlice<T, R>(this IEnumerable<R> intervals) where R : IInterval<T>
            where T : IComparable<T>
        {
            R[] rangeArray = intervals.ToArray();
            var window = new Heap<R>(new IntervalEndComparer<R, T>());

            T windowStart = default(T);
            int index = 0;
            while (index < rangeArray.Length || window.Count > 0)
            {
                R first;
                if (index < rangeArray.Length)
                {
                    // First grab all intervals that start at the same point as our current window
                    if (window.Count == 0)
                    {
                        R r1 = rangeArray[index++];
                        windowStart = r1.Start;
                        window.Add(r1);
                    }

                    while (index < rangeArray.Length && rangeArray[index].Start.CompareTo(windowStart) == 0)
                    {
                        window.Add(rangeArray[index]);
                        index++;
                    }

                    first = window[0];

                    // Then, if there are more intervals available, see if the next one starts earlier
                    // than the current window ends
                    if (index < rangeArray.Length)
                    {
                        R next = rangeArray[index];
                        if (next.Start.CompareTo(first.End) < 0)
                        {
                            yield return new Interval<T, R[]>(windowStart, next.Start, window.ToArray());
                            windowStart = next.Start;

                            // Since no interval falls out of the window, leave it as it is
                            continue;
                        }
                    }
                }

                first = window[0];

                // If we get here, then our current is the first one we need to handle
                yield return new Interval<T, R[]>(windowStart, first.End, window.ToArray());
                T windowEnd = first.End;

                // Now remove all periods that are no longer relevant
                while (window.Count > 0 && window[0].End.CompareTo(windowEnd) == 0)
                    window.Pop();
                windowStart = windowEnd;
            }
        }

        private static IEnumerable<IInterval<T, R[]>> InternalMerge<T, R>(this IEnumerable<R> intervals, IntervalMergeBehavior behavior)
            where R : IInterval<T> where T : IComparable<T>
        {
            var window = new List<R>();
            T windowEnd = default(T);
            foreach (R interval in intervals)
            {
                bool startNewWindowForThisRange = true;
                if (window.Count == 0)
                    startNewWindowForThisRange = false;
                else
                {
                    int comparisonResult = interval.Start.CompareTo(windowEnd);
                    if (comparisonResult < 0)
                        startNewWindowForThisRange = false;
                    else if (comparisonResult == 0 && behavior == IntervalMergeBehavior.OverlappingAndAdjacent)
                        startNewWindowForThisRange = false;
                }

                if (startNewWindowForThisRange)
                {
                    if (window.Count > 0)
                        yield return new Interval<T, R[]>(window[0].Start, windowEnd, window.ToArray());
                    window.Clear();
                }

                window.Add(interval);
                if (window.Count == 1)
                    windowEnd = interval.End;
                else if (interval.End.CompareTo(windowEnd) > 0)
                    windowEnd = interval.End;
            }

            if (window.Count > 0)
                yield return new Interval<T, R[]>(window[0].Start, windowEnd, window.ToArray());
        }

        private static IEnumerable<IInterval<T, TOutput>> InternalReduce<T, TInput, TOutput>(
            IEnumerable<IInterval<T, TInput>> intervals, Converter<TInput, TOutput> converter) where T : IComparable<T>
        {
            foreach (var interval in intervals)
                yield return new Interval<T, TOutput>(interval.Start, interval.End, converter(interval.Data));
        }

        #region Nested type: IntervalEndComparer

        private class IntervalEndComparer<TInterval, T> : IComparer<TInterval>
            where TInterval : IInterval<T> where T : IComparable<T>
        {
            #region IComparer<TInterval> Members

            public int Compare(TInterval x, TInterval y)
            {
                return x.End.CompareTo(y.End);
            }

            #endregion
        }

        #endregion
    }
}