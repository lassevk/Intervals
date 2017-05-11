using System;
using JetBrains.Annotations;

namespace Intervals
{
    /// <summary>
    /// This static class contains a factory method for easy creation of <see cref="Interval{T}"/>.
    /// </summary>
    public static class Interval
    {
        /// <summary>
        /// Constructs a new instance of <see cref="Interval{T}"/>.
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="start"/> has a higher value than <paramref name="end"/>.</para>
        /// </exception>
        /// <returns>
        /// The newly constructed interval.
        /// </returns>
        [NotNull]
        public static Interval<T> Create<T>(T start, T end)
            where T : struct, IComparable<T>
        {
            return new Interval<T>(start, end);
        }
    }
}