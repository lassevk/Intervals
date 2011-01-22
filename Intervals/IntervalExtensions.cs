using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals
{
    /// <summary>
    /// This class holds extension methods for <see cref="IInterval{T}"/> and
    /// <see cref="IInterval{T,TData}"/>.
    /// </summary>
    public static class IntervalExtensions
    {
        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static long GetSpan(this IInterval<int> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - (long)interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static byte GetSpan(this IInterval<byte> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return (byte)(interval.End - interval.Start);
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static long GetSpan(this IInterval<long> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static int GetSpan(this IInterval<short> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static double GetSpan(this IInterval<double> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static float GetSpan(this IInterval<float> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static decimal GetSpan(this IInterval<decimal> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Calculates the span of values the interval covers.
        /// </summary>
        /// <param name="interval">
        /// The interval to calculate the span for.
        /// </param>
        /// <returns>
        /// The span of the interval.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static TimeSpan GetSpan(this IInterval<DateTime> interval)
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.End - interval.Start;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified <paramref name="value"/>
        /// is considered to be a part of the specified <paramref name="interval"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension for the interval.
        /// </typeparam>
        /// <param name="interval">
        /// The interval to check the <paramref name="value"/> against.
        /// </param>
        /// <param name="value">
        /// The value to check against the <paramref name="interval"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="value"/> is considered to be a part of the <paramref name="interval"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Note that the <see cref="IInterval{T}.End"/> value is not considered to be a part of the interval.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval"/> is <c>null</c>.</para>
        /// </exception>
        public static bool Contains<T>(this IInterval<T> interval, T value) where T : IComparable<T>
        {
            if (interval == null)
                throw new ArgumentNullException("interval");

            return interval.Start.CompareTo(value) <= 0 && interval.End.CompareTo(value) > 0;
        }

        /// <summary>
        /// Returns <c>true</c> if the two intervals overlap, meaning that at least one value exists
        /// that is considered to be part of both intervals.
        /// </summary>
        /// <typeparam name="T">
        /// The type of dimension for the intervals.
        /// </typeparam>
        /// <param name="interval1">
        /// The first interval, to compare against <paramref name="interval2"/>.
        /// </param>
        /// <param name="interval2">
        /// The second interval, to compare against <paramref name="interval1"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the two intervals overlaps;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="interval1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="interval2"/> is <c>null</c>.</para>
        /// </exception>
        public static bool IsOverlapping<T>(this IInterval<T> interval1, IInterval<T> interval2) where T : IComparable<T>
        {
            if (interval1 == null)
                throw new ArgumentNullException("interval1");
            if (interval2 == null)
                throw new ArgumentNullException("interval2");

            return interval1.Start.CompareTo(interval2.End) >= 0 || interval1.End.CompareTo(interval2.Start) <= 0;
        }
    }
}