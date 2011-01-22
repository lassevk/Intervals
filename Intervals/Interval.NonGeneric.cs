using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals
{
    /// <summary>
    /// This class implements static methods for <see cref="Interval{T}"/>.
    /// </summary>
    public static class Interval
    {
        /// <summary>
        /// Creates a new <see cref="Interval{T}"/> with the given
        /// <paramref name="start"/> and <paramref name="end"/> values.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="Interval{T}.Start"/>
        /// and <see cref="Interval{T}.End"/> properties.
        /// </typeparam>
        /// <param name="start">
        /// The start of the interval.
        /// </param>
        /// <param name="end">
        /// The end of the interval.
        /// </param>
        /// <returns>
        /// The new <see cref="Interval{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="end"/> is less than or equal to <paramref name="start"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="start"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="end"/> is <c>null</c>.</para>
        /// </exception>
        public static Interval<T> Create<T>(T start, T end) where T : IComparable<T>
        {
            return new Interval<T>(start, end);
        }

        /// <summary>
        /// Creates a new <see cref="Interval{T}"/> with the given
        /// <paramref name="start"/> and <paramref name="end"/> values.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension to use for the <see cref="Interval{T}.Start"/>
        /// and <see cref="Interval{T}.End"/> properties.
        /// </typeparam>
        /// <typeparam name="TData">
        /// The type of data to associate with the <see cref="Interval{T,TData}"/>.
        /// </typeparam>
        /// <param name="start">
        /// The start of the interval.
        /// </param>
        /// <param name="end">
        /// The end of the interval.
        /// </param>
        /// <param name="data">
        /// The initial value for the <see cref="Interval{T,TData}.Data"/> property.
        /// </param>
        /// <returns>
        /// The new <see cref="Interval{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="end"/> is less than or equal to <paramref name="start"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="start"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="end"/> is <c>null</c>.</para>
        /// </exception>
        public static Interval<T, TData> Create<T, TData>(T start, T end, TData data) where T : IComparable<T>
        {
            return new Interval<T, TData>(start, end, data);
        }
    }
}