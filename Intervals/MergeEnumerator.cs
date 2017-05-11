using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

using static Intervals.Conditionals;

namespace Intervals
{
    internal sealed class MergeEnumerator<T> : IEnumerable<Slice<T>>
        where T : struct, IComparable<T>
    {
        [NotNull, ItemNotNull]
        private readonly IEnumerable<IInterval<T>> _Intervals;
        private readonly IntervalMergeBehavior _Behavior;

        public MergeEnumerator([NotNull, ItemNotNull] IEnumerable<IInterval<T>>  intervals, IntervalMergeBehavior behavior)
        {
            _Intervals = intervals;
            _Behavior = behavior;
        }

        [SuppressMessage("ReSharper", "CyclomaticComplexity")]
        public IEnumerator<Slice<T>> GetEnumerator()
        {
            var window = new List<IInterval<T>>();
            T windowEnd = default(T);
            foreach (IInterval<T> interval in _Intervals)
            {
                bool startNewWindowForThisRange = true;
                if (window.Count == 0)
                    startNewWindowForThisRange = false;
                else
                {
                    int comparisonResult = interval.Start.CompareTo(windowEnd);
                    if (comparisonResult < 0)
                        startNewWindowForThisRange = false;
                    else if (comparisonResult == 0 && _Behavior == IntervalMergeBehavior.OverlappingAndAdjacent)
                        startNewWindowForThisRange = false;
                }

                if (startNewWindowForThisRange)
                {
                    if (window.Count > 0)
                    {
                        assume(window[0] != null);
                        yield return new Slice<T>(window[0].Start, windowEnd, window.ToArray());
                    }
                    window.Clear();
                }

                window.Add(interval);
                if (window.Count == 1)
                    windowEnd = interval.End;
                else if (interval.End.CompareTo(windowEnd) > 0)
                    windowEnd = interval.End;
            }

            if (window.Count > 0)
            {
                assume(window[0] != null);
                yield return new Slice<T>(window[0].Start, windowEnd, window.ToArray());
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
