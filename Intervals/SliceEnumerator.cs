using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;

using static Intervals.Conditionals;

namespace Intervals
{
    internal class SliceEnumerator<T> : IEnumerable<Slice<T>> 
        where T : struct, IComparable<T>
    {
        [NotNull, ItemNotNull]
        private readonly List<IInterval<T>> _Intervals;

        public SliceEnumerator([NotNull, ItemNotNull] IEnumerable<IInterval<T>> intervals)
        {
            _Intervals = intervals.ToList();
        }

        [SuppressMessage("ReSharper", "CyclomaticComplexity")]
        public IEnumerator<Slice<T>> GetEnumerator()
        {
            var window = new IntervalHeap<T>();

            T windowStart = default(T);
            int index = 0;
            while (index < _Intervals.Count || window.Count > 0)
            {
                IInterval<T> first;
                if (index < _Intervals.Count)
                {
                    // First grab all intervals that start at the same point as our current window
                    if (window.Count == 0)
                    {
                        IInterval<T> r1 = _Intervals[index++];
                        assume(r1 != null);
                        windowStart = r1.Start;
                        window.Add(r1);
                    }

                    while (index < _Intervals.Count && _Intervals[index]?.Start.CompareTo(windowStart) == 0)
                    {
                        window.Add(_Intervals[index]);
                        index++;
                    }

                    first = window[0];

                    // Then, if there are more intervals available, see if the next one starts earlier
                    // than the current window ends
                    if (index < _Intervals.Count)
                    {
                        IInterval<T> next = _Intervals[index];
                        assume(next != null);
                        if (next.Start.CompareTo(first.End) < 0)
                        {
                            yield return new Slice<T>(windowStart, next.Start, window.ToArray());
                            windowStart = next.Start;

                            // Since no interval falls out of the window, leave it as it is
                            continue;
                        }
                    }
                }

                first = window[0];

                // If we get here, then our current is the first one we need to handle
                yield return new Slice<T>(windowStart, first.End, window.ToArray());
                T windowEnd = first.End;

                // Now remove all periods that are no longer relevant
                while (window.Count > 0 && window[0].End.CompareTo(windowEnd) == 0)
                    window.Pop();
                windowStart = windowEnd;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
