using System;
using System.Collections.Generic;
using System.Numerics;

namespace Intervals;

internal sealed class IntervalHeap<TBoundary, TTag>
    where TBoundary : struct, IComparisonOperators<TBoundary, TBoundary, bool>, IComparable<TBoundary>
{
    private readonly List<Interval<TBoundary, TTag>> _elements = new();

    public Interval<TBoundary, TTag> this[int index] => _elements[index];

    public int Count => _elements.Count;

    public void Add(Interval<TBoundary, TTag> interval)
    {
        _elements.Add(interval);
        SiftDown(0, _elements.Count - 1);
    }

    public Interval<TBoundary, TTag>[] ToArray() => _elements.ToArray();

    public Interval<TBoundary, TTag> Pop()
    {
        if (_elements.Count == 0)
        {
            throw new InvalidOperationException("Cannot pop from the heap, it is currently empty");
        }

        Interval<TBoundary, TTag> lastElement = _elements[^1];
        _elements.RemoveAt(_elements.Count - 1);
        Interval<TBoundary, TTag> returnItem;
        if (_elements.Count > 0)
        {
            returnItem = _elements[0];
            _elements[0] = lastElement;
            SiftUp(0);
        }
        else
        {
            returnItem = lastElement;
        }

        return returnItem;
    }

    private void SiftDown(int startPos, int pos)
    {
        Interval<TBoundary, TTag> newItem = _elements[pos];

        while (pos > startPos)
        {
            int parentPos = (pos - 1) / 2;
            Interval<TBoundary, TTag> parent = _elements[parentPos];

            if (parent.End <= newItem.End)
            {
                break;
            }

            _elements[pos] = parent;
            pos = parentPos;
        }

        _elements[pos] = newItem;
    }

    private void SiftUp(int pos)
    {
        int endPos = Count;
        int startPos = pos;
        Interval<TBoundary, TTag> newItem = _elements[pos];

        // Bubble up the smaller child until hitting a leaf.
        int childPos = 2 * pos + 1;

        while (childPos < endPos)
        {
            // Set childpos to index of smaller child.
            int rightPos = childPos + 1;

            if (rightPos < endPos)
            {
                if (_elements[rightPos].End <= _elements[childPos].End)
                {
                    childPos = rightPos;
                }
            }

            // Move the smaller child up.
            _elements[pos] = _elements[childPos];
            pos = childPos;
            childPos = 2 * pos + 1;
        }

        _elements[pos] = newItem;
        SiftDown(startPos, pos);
    }
}