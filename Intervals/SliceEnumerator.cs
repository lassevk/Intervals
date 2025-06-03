using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Intervals;

internal class SliceEnumerator<TBoundary, TTag> : IEnumerable<Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>>
    where TBoundary : struct, IComparisonOperators<TBoundary, TBoundary, bool>, IComparable<TBoundary>
{
    private readonly Interval<TBoundary, TTag>[] _intervals;

    public SliceEnumerator(IEnumerable<Interval<TBoundary, TTag>> intervals)
    {
        _intervals = intervals.ToArray();
    }

    public IEnumerator<Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>> GetEnumerator()
    {
        var window = new IntervalHeap<TBoundary, TTag>();

        var windowStart = default(TBoundary);
        int index = 0;
        while (index < _intervals.Length || window.Count > 0)
        {
            Interval<TBoundary, TTag> first;
            if (index < _intervals.Length)
            {
                // First grab all intervals that start at the same point as our current window
                if (window.Count == 0)
                {
                    Interval<TBoundary, TTag> r1 = _intervals[index++];
                    windowStart = r1.Start;
                    window.Add(r1);
                }

                while (index < _intervals.Length && _intervals[index].Start == windowStart)
                {
                    window.Add(_intervals[index]);
                    index++;
                }

                first = window[0];

                // Then, if there are more intervals available, see if the next one starts earlier
                // than the current window ends
                if (index < _intervals.Length)
                {
                    Interval<TBoundary, TTag> next = _intervals[index];
                    if (next.Start < first.End)
                    {
                        yield return new Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>(windowStart, next.Start, window.ToArray());

                        windowStart = next.Start;

                        // Since no interval falls out of the window, leave it as it is
                        continue;
                    }
                }
            }

            first = window[0];

            // If we get here, then our current is the first one we need to handle
            yield return new Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>(windowStart, first.End, window.ToArray());

            TBoundary windowEnd = first.End;

            // Now remove all periods that are no longer relevant
            while (window.Count > 0 && window[0].End == windowEnd)
            {
                window.Pop();
            }

            windowStart = windowEnd;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}