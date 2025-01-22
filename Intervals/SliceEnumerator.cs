using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Intervals;

internal class SliceEnumerator<TBoundary, TTag> : IEnumerable<Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>>
    where TBoundary : struct, IComparable<TBoundary>
{
    private readonly List<Interval<TBoundary, TTag>> _intervals;

    public SliceEnumerator(IEnumerable<Interval<TBoundary, TTag>> intervals)
    {
        _intervals = intervals.ToList();
    }

    public IEnumerator<Interval<TBoundary, IReadOnlyList<Interval<TBoundary, TTag>>>> GetEnumerator()
    {
        var window = new IntervalHeap<TBoundary, TTag>();

        var windowStart = default(TBoundary);
        int index = 0;
        while (index < _intervals.Count || window.Count > 0)
        {
            Interval<TBoundary, TTag> first;
            if (index < _intervals.Count)
            {
                // First grab all intervals that start at the same point as our current window
                if (window.Count == 0)
                {
                    Interval<TBoundary, TTag> r1 = _intervals[index++];
                    windowStart = r1.Start;
                    window.Add(r1);
                }

                while (index < _intervals.Count && _intervals[index].Start.CompareTo(windowStart) == 0)
                {
                    window.Add(_intervals[index]);
                    index++;
                }

                first = window[0];

                // Then, if there are more intervals available, see if the next one starts earlier
                // than the current window ends
                if (index < _intervals.Count)
                {
                    Interval<TBoundary, TTag> next = _intervals[index];
                    if (next.Start.CompareTo(first.End) < 0)
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
            while (window.Count > 0 && window[0].End.CompareTo(windowEnd) == 0)
            {
                window.Pop();
            }

            windowStart = windowEnd;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}