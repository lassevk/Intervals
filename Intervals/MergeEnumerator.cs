using System;
using System.Collections;
using System.Collections.Generic;

namespace Intervals;

internal sealed class MergeEnumerator<TBoundary, TTag> : IEnumerable<Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>>
    where TBoundary : struct, IComparable<TBoundary>
{
    private readonly IEnumerable<Interval<TBoundary, TTag>> _intervals;
    private readonly IntervalMergeBehavior _behavior;

    public MergeEnumerator(IEnumerable<Interval<TBoundary, TTag>> intervals, IntervalMergeBehavior behavior)
    {
        _intervals = intervals;
        _behavior = behavior;
    }

    public IEnumerator<Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>> GetEnumerator()
    {
        var window = new List<Interval<TBoundary, TTag>>();
        TBoundary windowEnd = default;
        foreach (Interval<TBoundary, TTag> interval in _intervals)
        {
            bool startNewWindowForThisRange = true;
            if (window.Count == 0)
            {
                startNewWindowForThisRange = false;
            }
            else
            {
                int comparisonResult = interval.Start.CompareTo(windowEnd);
                if (comparisonResult < 0)
                {
                    startNewWindowForThisRange = false;
                }
                else if (comparisonResult == 0 && _behavior == IntervalMergeBehavior.OverlappingAndAdjacent)
                {
                    startNewWindowForThisRange = false;
                }
            }

            if (startNewWindowForThisRange)
            {
                if (window.Count > 0)
                {
                    yield return new Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>(window[0].Start, windowEnd, window.ToArray());
                }

                window.Clear();
            }

            window.Add(interval);
            if (window.Count == 1)
            {
                windowEnd = interval.End;
            }
            else if (interval.End.CompareTo(windowEnd) > 0)
            {
                windowEnd = interval.End;
            }
        }

        if (window.Count > 0)
        {
            yield return new Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>(window[0].Start, windowEnd, window.ToArray());
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}