using System;

namespace Intervals
{
    /// <summary>
    /// This interface can be implemented by types that should function as intervals in the context of using
    /// them with this class library.
    /// </summary>
    /// <typeparam name="T">
    /// The type of boundary value the interval will be based on.
    /// </typeparam>
    public interface IInterval<out T>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// The starting value of the interval. This value is considered to be part of the interval.
        /// </summary>
        T Start { get; }

        /// <summary>
        /// The ending value of the interval. This value is not considered to be part of the interval.
        /// </summary>
        T End { get; }
    }
}