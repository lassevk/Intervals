using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Intervals
{
    /// <summary>
    /// This is the concrete and most basic implementation of <see cref="IInterval{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Interval<T> : IInterval<T> 
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="IInterval{T}"/>.
        /// </summary>
        /// <param name="start">
        /// The starting value for the new interval.
        /// </param>
        /// <param name="end">
        /// The ending value for the new interval.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="start"/> has a higher value than <paramref name="end"/>.</para>
        /// </exception>
        public Interval(T start, T end)
        {
            if (end.CompareTo(start) < 0)
                throw new ArgumentOutOfRangeException(nameof(end), $"end must be greater than start ({start}..{end})");

            Start = start;
            End = end;
        }

        /// <summary>
        /// The starting value of the interval. This value is considered to be part of the interval.
        /// </summary>
        public T Start { get; }

        /// <summary>
        /// The ending value of the interval. This value is not considered to be part of the interval.
        /// </summary>
        public T End { get; }

        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Object"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Object"/> is equal to the current <see cref="Object"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals([CanBeNull] object obj)
        {
            return IntervalEqualityComparer<T>.Default.Equals(this, obj as IInterval<T>);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return IntervalEqualityComparer<T>.Default.GetHashCode(this);
        }

        /// <summary>
        /// Returns a string that represents the current interval on the format <c>"[Start, End)"</c>.
        /// </summary>
        /// <returns>
        /// A string that represents the current interval.
        /// </returns>
        [NotNull]
        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "[{0}, {1})", Start, End);
    }
}