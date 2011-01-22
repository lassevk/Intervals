using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals
{
    /// <summary>
    /// This interface, which implies <see cref="IInterval{T}"/>, must be implemented by classes that
    /// will represent an interval of values between two end-points, and which has associated data.
    /// </summary>
    /// <typeparam name="T">
    /// The type of dimension to use for the <see cref="IInterval{T}.Start"/>
    /// and <see cref="IInterval{T}.End"/> properties.
    /// </typeparam>
    /// <typeparam name="TData">
    /// The type of data to associate with the interval.
    /// </typeparam>
    public interface IInterval<T, TData> : IInterval<T>, IComparable<IInterval<T, TData>>, IEquatable<IInterval<T, TData>>
        where T : IComparable<T>
    {
        /// <summary>
        /// Gets or sets the associated data for the interval.
        /// </summary>
        TData Data
        {
            get;
            set;
        }
    }
}