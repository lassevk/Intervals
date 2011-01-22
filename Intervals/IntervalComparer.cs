using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals
{
    /// <summary>
    /// This class implements <see cref="IComparer{T}"/> for <see cref="IInterval{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of dimension used for the interval.
    /// </typeparam>
    public sealed class IntervalComparer<T> : IComparer<IInterval<T>>
        where T : IComparable<T>
    {
        /// <summary>
        /// Gets the default <see cref="IntervalComparer{T}"/> for the <see cref="IInterval{T}"/> for
        /// the generic type <typeparamref name="T"/>.
        /// </summary>
        public static readonly IntervalComparer<T> Default = new IntervalComparer<T>();

        #region IComparer<IInterval<T>> Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// If return value is less than zero, then <paramref name="x"/> comes before y;
        /// if return value is equal to zero, then <paramref name="x"/> is considered equal to y;
        /// and if return value is greater than zero, then <paramref name="x"/> comes after y.
        /// </returns>
        /// <param name="x">
        /// The first object to compare.
        /// </param>
        /// <param name="y">
        /// The second object to compare.
        /// </param>
        public int Compare(IInterval<T> x, IInterval<T> y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return +1;

            Debug.Assert(x != null, "x should not be null here");
            Debug.Assert(y != null, "y should not be null here");

            int comparisonResult = x.Start.CompareTo(y.Start);
            if (comparisonResult != 0)
                return comparisonResult;

            return x.End.CompareTo(y.End);
        }

        #endregion
    }
}