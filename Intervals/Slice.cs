using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace Intervals
{
    /// <summary>
    /// This type is returned from <see cref="IntervalExtensions.Slice{T}"/> containing a single slice from the slice operation.
    /// </summary>
    /// <typeparam name="T">
    /// The type of boundary value the intervals and the slice are based on.
    /// </typeparam>
    public class Slice<T> : Interval<T>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="Slice{T}"/>.
        /// </summary>
        /// <param name="start">
        /// The starting value for the new interval.
        /// </param>
        /// <param name="end">
        /// The ending value for the new interval.
        /// </param>
        /// <param name="intervalsInSlice">
        /// The intervals that overlap this slice.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="start"/> has a higher value than <paramref name="end"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="intervalsInSlice"/> is <c>null</c>.</para>
        /// </exception>
        public Slice(T start, T end, [NotNull, ItemNotNull] IList<IInterval<T>> intervalsInSlice)
            : base(start, end)
        {
            if (intervalsInSlice == null)
                throw new ArgumentNullException(nameof(intervalsInSlice));

            IntervalsInSlice = new ReadOnlyCollection<IInterval<T>>(intervalsInSlice);
        }

        /// <summary>
        /// Gets a collection of the intervals that overlap this slice.
        /// </summary>
        [NotNull, ItemNotNull]
        public ReadOnlyCollection<IInterval<T>> IntervalsInSlice
        {
            get;
        }
    }
}
