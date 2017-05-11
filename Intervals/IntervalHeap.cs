using System;
using System.Collections.Generic;
using JetBrains.Annotations;

using static Intervals.Conditionals;

namespace Intervals
{
    internal sealed class IntervalHeap<T>
        where T : struct, IComparable<T>
    {
        [NotNull, ItemNotNull]
        private readonly List<IInterval<T>> _Elements = new List<IInterval<T>>();

        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        public IInterval<T> this[int index] => _Elements[index];

        public int Count => _Elements.Count;

        public void Add([NotNull] IInterval<T> interval)
        {
            _Elements.Add(interval);
            SiftDown(0, _Elements.Count - 1);
        }

        [NotNull, ItemNotNull]
        public IInterval<T>[] ToArray()
        {
            return _Elements.ToArray();
        }

        [NotNull]
        public IInterval<T> Pop()
        {
            if (_Elements.Count == 0)
                throw new InvalidOperationException("Cannot pop from the heap, it is currently empty");

            IInterval<T> lastElement = _Elements[_Elements.Count - 1];
            assume(lastElement != null);

            _Elements.RemoveAt(_Elements.Count - 1);
            IInterval<T> returnItem;
            if (_Elements.Count > 0)
            {
                returnItem = _Elements[0];
                assume(returnItem != null);
                _Elements[0] = lastElement;
                SiftUp(0);
            }
            else
                returnItem = lastElement;

            return returnItem;
        }

        private void SiftDown(int startPos, int pos)
        {
            IInterval<T> newItem = _Elements[pos];
            assume(newItem != null);

            while (pos > startPos)
            {
                int parentPos = (pos - 1) / 2;
                IInterval<T> parent = _Elements[parentPos];

                assume(parent != null);
                if (parent.End.CompareTo(newItem.End) <= 0)
                    break;
                _Elements[pos] = parent;
                pos = parentPos;
            }

            _Elements[pos] = newItem;
        }

        private void SiftUp(int pos)
        {
            int endPos = Count;
            int startPos = pos;
            IInterval<T> newItem = _Elements[pos];

            // Bubble up the smaller child until hitting a leaf.
            int childPos = 2 * pos + 1;

            while (childPos < endPos)
            {
                // Set childpos to index of smaller child.
                int rightPos = childPos + 1;

                if (rightPos < endPos)
                {
                    assume(_Elements[rightPos] != null && _Elements[childPos] != null);
                    if (_Elements[rightPos].End.CompareTo(_Elements[childPos].End) <= 0)
                        childPos = rightPos;
                }

                // Move the smaller child up.
                _Elements[pos] = _Elements[childPos];
                pos = childPos;
                childPos = 2 * pos + 1;
            }

            _Elements[pos] = newItem;
            SiftDown(startPos, pos);
        }
    }
}