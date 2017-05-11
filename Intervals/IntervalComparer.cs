using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Intervals
{
    /// <summary>
    /// Implements <see cref="IComparer{T}"/> for types that implement the <see cref="IInterval{T}"/> by only
    /// considering the <see cref="IInterval{T}.Start"/> and <see cref="IInterval{T}.End"/> properties.
    /// </summary>
    /// <typeparam name="T">
    /// The type of boundary value the interval is based on.
    /// </typeparam>
    public sealed class IntervalComparer<T> : IComparer<IInterval<T>>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// Holds the default instance for <see cref="IntervalComparer{T}"/>.
        /// </summary>
        [NotNull]
        public static readonly IntervalComparer<T> Default = new IntervalComparer<T>();

        /// <summary>
        /// Compares two intervals and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// <para>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.</para>
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <description>Meaning</description>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero</term>
        ///         <description><paramref name="x"/> comes before <paramref name="y"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term>Zero</term>
        ///         <description><paramref name="x"/> is equal to <paramref name="y"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero</term>
        ///         <description><paramref name="x"/> comes after <paramref name="y"/>.</description>
        ///     </item>
        /// </list>
        /// </returns>
        /// <param name="x">
        /// The first interval to compare.
        /// </param>
        /// <param name="y">
        /// The second interval to compare.
        /// </param>
        /// <remarks>
        /// Note that if both <paramref name="x"/> and <paramref name="y"/> is <c>null</c>, the return value will be zero, whereas
        /// if only one of them is <c>null</c> then that interval comes first.
        /// </remarks>
        public int Compare([CanBeNull] IInterval<T> x, [CanBeNull] IInterval<T> y)
        {
            if (ReferenceEquals(x, y))
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return +1;

            int result = x.Start.CompareTo(y.Start);
            if (result != 0)
                return result;
            return x.End.CompareTo(y.End);
        }
    }
}
