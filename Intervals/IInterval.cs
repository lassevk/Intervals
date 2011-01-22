using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals
{
    /// <summary>
    /// This interface must be implemented by classes that will represent a simple
    /// interval of values between two end-points.
    /// </summary>
    /// <typeparam name="T">
    /// The type of dimension to use for the <see cref="Start"/>
    /// and <see cref="End"/> properties.
    /// </typeparam>
    /// <remarks>
    /// Note that <see cref="Start"/> is considered to be part of the interval, whereas
    /// <see cref="End"/> is not.
    /// </remarks>
    public interface IInterval<T> : IComparable<IInterval<T>>, IEquatable<IInterval<T>>
        where T : IComparable<T>
    {
        /// <summary>
        /// Gets the start of the interval. This value is considered part of this <see cref="IInterval{T}"/>.
        /// </summary>
        T Start
        {
            get;
        }

        /// <summary>
        /// Gets the end of the interval. This value is <b>not</b> considered part of this <see cref="IInterval{T}"/>.
        /// </summary>
        T End
        {
            get;
        }
    }
}