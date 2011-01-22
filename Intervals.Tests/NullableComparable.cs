using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals.Tests
{
    /// <summary>
    /// This class is used to test various value-based comparisons for the intervals.
    /// </summary>
    public class NullableComparable : IComparable<NullableComparable>
    {
        private readonly int _Value;

        public NullableComparable(int value)
        {
            _Value = value;
        }

        public int Value
        {
            get
            {
                return _Value;
            }
        }

        #region IComparable<NullableComparable> Members

        public int CompareTo(NullableComparable other)
        {
            if (other == null)
                return +1;

            return Value.CompareTo(other.Value);
        }

        #endregion
    }
}