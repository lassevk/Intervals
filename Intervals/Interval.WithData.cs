using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals
{
    /// <summary>
    /// This class is a basic implementation of <see cref="IInterval{T,TData}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of dimension to use for the <see cref="Interval{T}.Start"/>
    /// and <see cref="Interval{T}.End"/> properties.
    /// </typeparam>
    /// <typeparam name="TData">
    /// The type of data to associate with this <see cref="Interval{T,TData}"/>.
    /// </typeparam>
    public class Interval<T, TData> : Interval<T>, IInterval<T, TData>
        where T : IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Interval{T,TData}"/> class.
        /// </summary>
        /// <param name="start">
        /// The start of this <see cref="Interval{T,TData}"/>.
        /// This value is considered to be part of the interval.
        /// </param>
        /// <param name="end">
        /// The end of this <see cref="Interval{T,TData}"/>.
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
            : this(start, end, default(TData))
        {
            // Do nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval{T,TData}"/> class,
        /// with an initial <see cref="Data"/> value.
        /// </summary>
        /// <param name="start">
        /// The start of this <see cref="Interval{T,TData}"/>.
        /// This value is considered to be part of the interval.
        /// </param>
        /// <param name="end">
        /// The end of this <see cref="Interval{T,TData}"/>.
        /// This value is <b>not</b> considered to be part of the interval.
        /// </param>
        /// <param name="data">
        /// The initial value for the <see cref="Data"/> property.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="end"/> is less than or equal to <paramref name="start"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="start"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="end"/> is <c>null</c>.</para>
        /// </exception>
        public Interval(T start, T end, TData data)
            : base(start, end)
        {
            Data = data;
        }

        #region IInterval<T,TData> Members

        /// <summary>
        /// Gets or sets the associated data for the interval.
        /// </summary>
        public TData Data
        {
            get;

            set;
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
        public int CompareTo(IInterval<T, TData> other)
        {
            return base.CompareTo(other);
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
        public bool Equals(IInterval<T, TData> other)
        {
            return IntervalEqualityComparer<T>.Default.Equals(this, other);
        }

        #endregion
    }
}