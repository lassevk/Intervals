using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Intervals
{
    /// <summary>
    /// Extension methods for <see cref="IInterval{T}"/> and <see cref="IEnumerable{T}"/> when T is <see cref="IInterval{T}"/>.
    /// </summary>
    public static class IntervalExtensions
    {
        /// <summary>
        /// Gets the span, the difference between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>
        /// for an interval of type <see cref="Int32"/>.
        /// </summary>
        /// <param name="interval">
        /// The interval to get the span of.
        /// </param>
        /// <returns>
        /// The difference between between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>;
        /// or 0 if <paramref name="interval"/> is <c>null</c>.
        /// </returns>
        public static long GetSpan([CanBeNull] this IInterval<int> interval) => interval?.End - (long)(interval?.Start ?? 0) ?? 0L;

        /// <summary>
        /// Gets the span, the difference between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>
        /// for an interval of type <see cref="Byte"/>.
        /// </summary>
        /// <param name="interval">
        /// The interval to get the span of.
        /// </param>
        /// <returns>
        /// The difference between between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>;
        /// or 0 if <paramref name="interval"/> is <c>null</c>.
        /// </returns>
        public static byte GetSpan([CanBeNull] this IInterval<byte> interval) => (byte)(interval?.End - interval?.Start ?? 0);

        /// <summary>
        /// Gets the span, the difference between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>
        /// for an interval of type <see cref="Int64"/>.
        /// </summary>
        /// <param name="interval">
        /// The interval to get the span of.
        /// </param>
        /// <returns>
        /// The difference between between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>;
        /// or 0 if <paramref name="interval"/> is <c>null</c>.
        /// </returns>
        public static long GetSpan([CanBeNull] this IInterval<long> interval) => interval?.End - interval?.Start ?? 0L;

        /// <summary>
        /// Gets the span, the difference between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>
        /// for an interval of type <see cref="Int16"/>.
        /// </summary>
        /// <param name="interval">
        /// The interval to get the span of.
        /// </param>
        /// <returns>
        /// The difference between between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>;
        /// or 0 if <paramref name="interval"/> is <c>null</c>.
        /// </returns>
        public static int GetSpan([CanBeNull] this IInterval<short> interval) => interval?.End - interval?.Start ?? 0;

        /// <summary>
        /// Gets the span, the difference between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>
        /// for an interval of type <see cref="Double"/>.
        /// </summary>
        /// <param name="interval">
        /// The interval to get the span of.
        /// </param>
        /// <returns>
        /// The difference between between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>;
        /// or 0 if <paramref name="interval"/> is <c>null</c>.
        /// </returns>
        public static double GetSpan([CanBeNull] this IInterval<double> interval) => interval?.End - interval?.Start ?? 0.0;

        /// <summary>
        /// Gets the span, the difference between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>
        /// for an interval of type <see cref="Single"/>.
        /// </summary>
        /// <param name="interval">
        /// The interval to get the span of.
        /// </param>
        /// <returns>
        /// The difference between between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>;
        /// or 0 if <paramref name="interval"/> is <c>null</c>.
        /// </returns>
        public static float GetSpan([CanBeNull] this IInterval<float> interval) => interval?.End - interval?.Start ?? 0F;

        /// <summary>
        /// Gets the span, the difference between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>
        /// for an interval of type <see cref="Decimal"/>.
        /// </summary>
        /// <param name="interval">
        /// The interval to get the span of.
        /// </param>
        /// <returns>
        /// The difference between between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>;
        /// or 0 if <paramref name="interval"/> is <c>null</c>.
        /// </returns>
        public static decimal GetSpan([CanBeNull] this IInterval<decimal> interval) => interval?.End - interval?.Start ?? 0M;

        /// <summary>
        /// Gets the span, the difference between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>
        /// for an interval of type <see cref="DateTime"/>.
        /// </summary>
        /// <param name="interval">
        /// The interval to get the span of.
        /// </param>
        /// <returns>
        /// The difference between between <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/>;
        /// or 0 if <paramref name="interval"/> is <c>null</c>.
        /// </returns>
        public static TimeSpan GetSpan([CanBeNull] this IInterval<DateTime> interval) => interval?.End - interval?.Start ?? TimeSpan.Zero;

        /// <summary>
        /// Calculates all the slices of the intervals in the collection. Please see the wiki for a more complete explanation of
        /// the slice operation.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the intervals are based on.
        /// </typeparam>
        /// <param name="intervals">
        /// The intervals to slice.
        /// </param>
        /// <param name="isAlreadyOrdered">
        /// If it is known that all the intervals are already ordered according to the <see cref="IntervalComparer{T}"/>
        /// specification, then this parameter can be set to <c>true</c> to avoid an extra sorting step. The default
        /// value is <c>false</c> and should be left as-is if this cannot be guaranteed. It is undocumented and unsupported
        /// to call this method with this parameter as <c>true</c> if the intervals aren't correctly ordered.
        /// </param>
        /// <returns>
        /// A collection of slices for the intervals.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="intervals"/> is <c>null</c>.
        /// </exception>
        [NotNull, ItemNotNull]
        public static IEnumerable<Slice<T>> Slice<T>([NotNull, ItemNotNull] this IEnumerable<IInterval<T>> intervals, bool isAlreadyOrdered = false)
            where T : struct, IComparable<T>
        {
            if (intervals == null)
                throw new ArgumentNullException(nameof(intervals));

            if (!isAlreadyOrdered)
                intervals = intervals.OrderBy(interval => interval, IntervalComparer<T>.Default);

            return new SliceEnumerator<T>(intervals);
        }

        /// <summary>
        /// Merges overlapping and possibly adjacent intervals into slices.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the intervals are based on.
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
        [NotNull, ItemNotNull]
        public static IEnumerable<Slice<T>> Merge<T>([NotNull, ItemNotNull] this IEnumerable<IInterval<T>> intervals, IntervalMergeBehavior behavior = IntervalMergeBehavior.Default)
            where T : struct, IComparable<T>
        {
            if (intervals == null)
                throw new ArgumentNullException(nameof(intervals));

            return new MergeEnumerator<T>(intervals, behavior);
        }

        /// <summary>
        /// Factory extension method on T to create an interval between two boundary values.
        /// </summary>
        /// <typeparam name="T">
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
        [NotNull]
        public static Interval<T> IntervalTo<T>(this T start, T end)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(start, end);
        }

        /// <summary>
        /// Factory extension method on T to create an interval between two boundary values with a tag.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval will be based on.
        /// </typeparam>
        /// <param name="start">
        /// The starting value for the new interval.
        /// </param>
        /// <param name="end">
        /// The ending value for the new interval.
        /// </param>
        /// <param name="tag">
        /// The tag for the interval.
        /// </param>
        /// <returns>
        /// The newly created interval.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="start"/> has a higher value than <paramref name="end"/>.</para>
        /// </exception>
        [NotNull]
        public static TaggedInterval<T> IntervalTo<T>(this T start, T end, [CanBeNull] object tag)
            where T : struct, IComparable<T>
        {
            return new TaggedInterval<T>(start, end, tag);
        }

        /// <summary>
        /// Compares two intervals and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <returns>
        /// <para>A signed integer that indicates the relative values of <paramref name="interval"/> and <paramref name="other"/>, as shown in the following table.</para>
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <description>Meaning</description>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero</term>
        ///         <description><paramref name="interval"/> comes before <paramref name="other"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term>Zero</term>
        ///         <description><paramref name="interval"/> is equal to <paramref name="other"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero</term>
        ///         <description><paramref name="interval"/> comes after <paramref name="other"/>.</description>
        ///     </item>
        /// </list>
        /// </returns>
        /// <param name="interval">
        /// The first interval to compare.
        /// </param>
        /// <param name="other">
        /// The second interval to compare.
        /// </param>
        /// <remarks>
        /// Note that if both <paramref name="interval"/> and <paramref name="other"/> is <c>null</c>, the return value will be zero, whereas
        /// if only one of them is <c>null</c> then that interval comes first.
        /// </remarks>
        public static int CompareTo<T>([CanBeNull] this IInterval<T> interval, [CanBeNull] IInterval<T> other)
            where T : struct, IComparable<T>
        {
            return IntervalComparer<T>.Default.Compare(interval, other);
        }

        /// <summary>
        /// Compares the two intervals and determines if they are equal when only considering the <see cref="IInterval{T}.Start"/>
        /// and <see cref="IInterval{T}.End"/> properties.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <param name="interval">
        /// The first interval to compare.
        /// </param>
        /// <param name="other">
        /// The second interval to compare.
        /// </param>
        /// <returns>
        /// <c>true</c> if both <paramref name="interval"/> and <paramref name="other"/> is <c>null</c> or if both are non-null and have the same
        /// <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/> values; otherwise, <c>false</c>.
        /// </returns>
        public static bool Equals<T>([CanBeNull] this IInterval<T> interval, [CanBeNull] IInterval<T> other)
            where T : struct, IComparable<T>
        {
            return IntervalEqualityComparer<T>.Default.Equals(interval, other);
        }

        /// <summary>
        /// Determines if the specified value is considered to be part of the specified interval.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <param name="interval">
        /// The interval to examine.
        /// </param>
        /// <param name="value">
        /// The value to determine whether it is inside <paramref name="interval"/> or not.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="value"/> is considered to be part of <paramref name="interval"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// If <paramref name="interval"/> is <c>null</c> the return value will be <c>false</c>.
        /// </remarks>
        public static bool Contains<T>([CanBeNull] this IInterval<T> interval, T value)
            where T : struct, IComparable<T>
        {
            if (interval == null)
                return false;

            return interval.Start.CompareTo(value) <= 0 && interval.End.CompareTo(value) > 0;
        }

        /// <summary>
        /// Determines if the two intervals overlap. Overlap means that there must exist at least one value that
        /// is considered to be part of both intervals.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <param name="interval">
        /// The first interval to compare.
        /// </param>
        /// <param name="other">
        /// The second interval to compare.
        /// </param>
        /// <returns>
        /// <c>true</c> if the two intervals overlap; otherwise <c>false</c>.
        /// </returns>
        public static bool IsOverlapping<T>([CanBeNull] this IInterval<T> interval, [CanBeNull] IInterval<T> other)
            where T : struct, IComparable<T>
        {
            if (interval == null || other == null)
                return false;
            if (ReferenceEquals(interval, other))
                return true;
            return interval.Start.CompareTo(other.End) < 0 && interval.End.CompareTo(other.Start) > 0;
        }

        /// <summary>
        /// Attempts to get the overlapping part between two intervals, meaning it will return a new interval that contains the portion
        /// of the two intervals that are in common / overlap. If the two intervals does not overlap this method will return <c>null</c>
        /// instead.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <param name="interval">
        /// The first interval to compare.
        /// </param>
        /// <param name="other">
        /// The second interval to compare.
        /// </param>
        /// <returns>
        /// A new <see cref="Interval{T}"/> containing the overlapping part between the two intervals or
        /// <c>null</c> if the two intervals does not overlap.
        /// </returns>
        /// <remarks>
        /// Note that if either or both interval is <c>null</c> this method will also return <c>null</c>.
        /// </remarks>
        [CanBeNull]
        public static IInterval<T> TryGetOverlappingInterval<T>([CanBeNull] this IInterval<T> interval, [CanBeNull] IInterval<T> other)
            where T : struct, IComparable<T>
        {
            if (ReferenceEquals(interval, other))
                return interval;
            if (interval == null || other == null)
                return null;

            T start = interval.Start;
            if (other.Start.CompareTo(start) > 0)
                start = other.Start;

            T end = interval.End;
            if (other.End.CompareTo(end) < 0)
                end = other.End;

            if (start.CompareTo(end) >= 0)
                return null;

            return new Interval<T>(start, end);
        }

        /// <summary>
        /// Gets the overlapping part between two intervals, meaning it will return a new interval that contains the portion
        /// of the two intervals that are in common / overlap. If the two intervals does not overlap this method will throw
        /// <see cref="InvalidOperationException"/>.
        /// instead.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <param name="interval">
        /// The first interval to compare.
        /// </param>
        /// <param name="other">
        /// The second interval to compare.
        /// </param>
        /// <returns>
        /// A new <see cref="Interval{T}"/> containing the overlapping part between the two intervals.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The two intervals does not overlap.
        /// </exception>
        /// <remarks>
        /// Note that if either or both interval is <c>null</c> this method will also throw <see cref="InvalidOperationException"/>.
        /// </remarks>
        [NotNull]
        public static IInterval<T> GetOverlappingInterval<T>([NotNull] this IInterval<T> interval, [NotNull] IInterval<T> other)
            where T : struct, IComparable<T>
        {
            var result = TryGetOverlappingInterval(interval, other);
            if (result == null)
                throw new InvalidOperationException($"Unable to get overlapping interval between {interval} and {other}");
            return result;
        }

        /// <summary>
        /// Validates that the specified interval is considered "valid" which means that <see cref="IInterval{T}.Start"/>
        /// is either equal to or less than <see cref="IInterval{T}.End"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <param name="interval">
        /// The interval to validate.
        /// </param>
        /// <returns>
        /// </returns>
        [ContractAnnotation("null => false")]
        public static bool IsValid<T>([CanBeNull] this IInterval<T> interval)
            where T : struct, IComparable<T>
        {
            return interval != null && interval.Start.CompareTo(interval.End) <= 0;
        }

        /// <summary>
        /// Determines if the specified interval is empty or not. An empty interval is an interval where there exists no
        /// value at all that is considered part of the interval. Technically this means that <see cref="IInterval{T}.Start"/>
        /// equals <see cref="IInterval{T}.End"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <param name="interval">
        /// The interval to validate.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="interval"/> is <c>null</c> or if the <see cref="IInterval{T}.Start"/> property equals
        /// the <see cref="IInterval{T}.End"/> property.
        /// </returns>
        [ContractAnnotation("null => true")]
        public static bool IsEmpty<T>([CanBeNull] this IInterval<T> interval)
            where T : struct, IComparable<T>
        {
            return interval == null || interval.Start.CompareTo(interval.End) == 0;
        }

        /// <summary>
        /// Determines if the two intervals are adjacent, meaning that where one interval starts the other ends, or vice versa.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <param name="interval">
        /// The first interval to compare.
        /// </param>
        /// <param name="other">
        /// The second interval to compare.
        /// </param>
        /// <returns>
        /// <c>true</c> if the two intervals are ajdacent; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Note that if either or both <paramref name="interval"/> and <paramref name="other"/> is <c>null</c> this method
        /// will return <c>false</c>.
        /// </remarks>
        [ContractAnnotation("interval:null => false")]
        [ContractAnnotation("other:null => false")]
        public static bool IsAdjacentTo<T>([CanBeNull] this IInterval<T> interval, [CanBeNull] IInterval<T> other)
            where T : struct, IComparable<T>
        {
            if (interval == null || other == null)
                return false;

            return interval.Start.CompareTo(other.End) == 0 || interval.End.CompareTo(other.Start) == 0;
        }

        /// <summary>
        /// Attempts to calculate the union of the overlapping or adjacent intervals, meaning it will return a new interval that
        /// contains the outer boundaries of the two intervals combined. If the two intervals does not overlap
        /// and aren't adjacent this method will return <c>null</c>.
        /// instead.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <param name="interval">
        /// The first interval to compare.
        /// </param>
        /// <param name="other">
        /// The second interval to compare.
        /// </param>
        /// <returns>
        /// A new <see cref="Interval{T}"/> containing the outermost boundaries of the two overlapping or adjacent
        /// intervals; or <c>null</c> if the two intervals does not overlap and aren't adjacent.
        /// </returns>
        /// <remarks>
        /// Note that if either or both interval is <c>null</c> this method will also return <c>null</c>.
        /// </remarks>
        [ContractAnnotation("interval:null => null")]
        [ContractAnnotation("other:null => null")]
        public static Interval<T> TryGetUnion<T>([CanBeNull] this IInterval<T> interval, [CanBeNull] IInterval<T> other)
            where T : struct, IComparable<T>
        {
            if (interval == null || other == null)
                return null;

            if (!interval.IsOverlapping(other) && !interval.IsAdjacentTo(other))
                return null;

            return Interval.Create(Min(interval.Start, other.Start), Max(interval.End, other.End));
        }

        /// <summary>
        /// Calculates the union of the overlapping or adjacent intervals, meaning it will return a new interval that
        /// contains the outer boundaries of the two intervals combined. If the two intervals does not overlap
        /// and aren't adjacent this method will throw <see cref="InvalidOperationException"/>.
        /// instead.
        /// </summary>
        /// <typeparam name="T">
        /// The type of boundary value the interval is based on.
        /// </typeparam>
        /// <param name="interval">
        /// The first interval to compare.
        /// </param>
        /// <param name="other">
        /// The second interval to compare.
        /// </param>
        /// <returns>
        /// A new <see cref="Interval{T}"/> containing the outermost boundaries of the two overlapping or adjacent
        /// intervals.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The two intervals doesn't overlap and aren't adjacent.
        /// </exception>
        /// <remarks>
        /// Note that if either or both interval is <c>null</c> this method will also throw <see cref="InvalidOperationException"/>.
        /// </remarks>
        public static Interval<T> GetUnion<T>([NotNull] this IInterval<T> interval, [NotNull] IInterval<T> other)
            where T : struct, IComparable<T>
        {
            var result = TryGetUnion(interval, other);
            if (result == null)
                throw new InvalidOperationException($"Unable to get the union of {interval} and {other} as they do not overlap nor are they adjacent");
            return result;
        }

        private static T Min<T>(T a, T b) where T : struct, IComparable<T> => a.CompareTo(b) < 0 ? a : b;
        private static T Max<T>(T a, T b) where T : struct, IComparable<T> => a.CompareTo(b) > 0 ? a : b;
    }
}