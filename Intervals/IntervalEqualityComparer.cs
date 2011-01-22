using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals
{
    /// <summary>
    /// This class implements <see cref="IEqualityComparer{T}"/> for <see cref="IInterval{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of dimension used for the interval.
    /// </typeparam>
    public sealed class IntervalEqualityComparer<T> : IEqualityComparer<IInterval<T>>
        where T : IComparable<T>
    {
        /// <summary>
        /// Gets the default <see cref="IEqualityComparer{T}"/> for <see cref="IInterval{T}"/>
        /// for the specified <typeparamref name="T"/>.
        /// </summary>
        public static readonly IntervalEqualityComparer<T> Default = new IntervalEqualityComparer<T>();

        #region IEqualityComparer<IInterval<T>> Members

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the specified objects are equal;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <param name="x">
        /// The first object of type <typeparamref name="T"/> to compare.
        /// </param>
        /// <param name="y">
        /// The second object of type <typeparamref name="T"/> to compare.
        /// </param>
        public bool Equals(IInterval<T> x, IInterval<T> y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            Debug.Assert(x != null, "x should not be null here");
            Debug.Assert(y != null, "y should not be null here");

            return x.Start.CompareTo(y.Start) == 0 && x.End.CompareTo(y.End) == 0;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">
        /// The <see cref="Object"/> for which a hash code is to be returned.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        /// </exception>
        public int GetHashCode(IInterval<T> obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            int result = 37;

            result *= 23;
            if (obj.Start != null)
                result += obj.Start.GetHashCode();

            result *= 23;
            if (obj.End != null)
                result += obj.End.GetHashCode();

            return result;
        }

        #endregion
    }
}