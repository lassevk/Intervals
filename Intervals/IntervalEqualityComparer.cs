using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Intervals
{
    /// <summary>
    /// Implements <see cref="IEqualityComparer{T}"/> for types that implement the <see cref="IInterval{T}"/> by only
    /// considering the <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/> properties.
    /// </summary>
    /// <typeparam name="T">
    /// The type of boundary value the interval is based on.
    /// </typeparam>
    public sealed class IntervalEqualityComparer<T> : IEqualityComparer<IInterval<T>>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// Holds the default instance for <see cref="IntervalEqualityComparer{T}"/>.
        /// </summary>
        [NotNull]
        public static readonly IntervalEqualityComparer<T> Default = new IntervalEqualityComparer<T>();

        /// <summary>
        /// Compares the two intervals and determines if they are equal when only considering the <see cref="IInterval{T}.Start"/>
        /// and <see cref="IInterval{T}.End"/> properties.
        /// </summary>
        /// <param name="x">
        /// The first interval to compare.
        /// </param>
        /// <param name="y">
        /// The second interval to compare.
        /// </param>
        /// <returns>
        /// <c>true</c> if both <paramref name="x"/> and <paramref name="y"/> is <c>null</c> or if both are non-null and have the same
        /// <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/> values; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals([CanBeNull] IInterval<T> x, [CanBeNull] IInterval<T> y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            return x.Start.CompareTo(y.Start) == 0 && x.End.CompareTo(y.End) == 0;
        }

        /// <summary>
        /// Calculates the hashcode of the interval by only considering the <see cref="IInterval{T}.Start"/>
        /// and <see cref="IInterval{T}.End"/> properties.
        /// </summary>
        /// <param name="obj">
        /// The interval to calculate the hashcode for.
        /// </param>
        /// <returns>
        /// 0 if <paramref name="obj"/> is <c>null</c>; otherwise the hashcode of <see cref="IInterval{T}.Start"/>
        /// and <see cref="IInterval{T}.End"/> combined.
        /// </returns>
        public int GetHashCode([CanBeNull] IInterval<T> obj)
        {
            if (obj == null)
                return 0;

            return (23 + obj.Start.GetHashCode()) * 37 + obj.End.GetHashCode();
        }
    }
}