using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Intervals
{
    /// <summary>
    /// This class is a basic implementation of <see cref="IInterval{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of dimension to use for the <see cref="Interval{T}.Start"/>
    /// and <see cref="Interval{T}.End"/> properties.
    /// </typeparam>
    public class Interval<T> : IInterval<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// This is the backing field for the <see cref="End"/> property.
        /// </summary>
        private readonly T _End;

        /// <summary>
        /// This is the backing field for the <see cref="Start"/> property.
        /// </summary>
        private readonly T _Start;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="start">
        /// The start of this <see cref="Interval{T}"/>.
        /// This value is considered to be part of the interval.
        /// </param>
        /// <param name="end">
        /// The end of this <see cref="Interval{T}"/>.
        /// This value is <b>not</b> considered to be part of the interval.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="end"/> is less than or equal to <paramref name="start"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="start"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="end"/> is <c>null</c>.</para>
        /// </exception>
        public Interval(T start, T end)
        {
            if (start == null)
                throw new ArgumentNullException("start");
            if (end == null)
                throw new ArgumentNullException("end");
            if (end.CompareTo(start) <= 0)
                throw new ArgumentOutOfRangeException("end", end, "end must be greater than start");

            _Start = start;
            _End = end;
        }

        #region IInterval<T> Members

        /// <summary>
        /// Gets the start of the interval. This value is considered part of this <see cref="IInterval{T}"/>.
        /// </summary>
        public T Start
        {
            get
            {
                return _Start;
            }
        }

        /// <summary>
        /// Gets the end of the interval. This value is <b>not</b> considered part of this <see cref="IInterval{T}"/>.
        /// </summary>
        public T End
        {
            get
            {
                return _End;
            }
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        public int CompareTo(IInterval<T> other)
        {
            return IntervalComparer<T>.Default.Compare(this, other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        public bool Equals(IInterval<T> other)
        {
            return IntervalEqualityComparer<T>.Default.Equals(this, other);
        }

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Interval{T}"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Object"/> is equal to the current <see cref="Interval{T}"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <param name="obj">
        /// The <see cref="Object"/> to compare with the current <see cref="Interval{T}"/>. 
        /// </param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as IInterval<T>;
            if (other == null)
                return false;

            return Equals(other);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="Interval{T}"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return IntervalEqualityComparer<T>.Default.GetHashCode(this);
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="Interval{T}"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> that represents the current <see cref="Interval{T}"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}..{1}", Start, End);
        }
    }
}