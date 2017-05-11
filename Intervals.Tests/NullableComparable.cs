using System;

namespace Intervals.Tests
{
    /// <summary>
    /// This class is used to test various value-based comparisons for the intervals.
    /// </summary>
    public struct NullableComparable : IComparable<NullableComparable>
    {
        public NullableComparable(int value)
        {
            Value = value;
        }

        public int Value
        {
            get;
        }

        #region IComparable<NullableComparable> Members

        public int CompareTo(NullableComparable other)
        {
            return Value.CompareTo(other.Value);
        }

        #endregion
    }
}