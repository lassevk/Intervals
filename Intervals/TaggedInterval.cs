using System;
using JetBrains.Annotations;

namespace Intervals
{
    /// <summary>
    /// This class inherits from <see cref="Interval{T}"/> and adds a <see cref="TaggedInterval{T}.Tag"/> property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaggedInterval<T> : Interval<T>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="TaggedInterval{T}"/>.
        /// </summary>
        /// <param name="start">
        /// The starting value for the new interval.
        /// </param>
        /// <param name="end">
        /// The ending value for the new interval.
        /// </param>
        /// <param name="tag">
        /// The tag for the interval.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="start"/> has a higher value than <paramref name="end"/>.</para>
        /// </exception>
        public TaggedInterval(T start, T end, [CanBeNull] object tag)
            : base(start, end)
        {
            Tag = tag;
        }

        /// <summary>
        /// Gets the tag of this interval.
        /// </summary>
        public object Tag { get; }

        /// <summary>
        /// Returns a string that represents the current interval on the format <c>"[Start, End) [Tag]"</c>.
        /// </summary>
        /// <returns>
        /// A string that represents the current interval.
        /// </returns>
        [NotNull]
        public override string ToString() => $"{base.ToString()} [{Tag}]";
    }
}