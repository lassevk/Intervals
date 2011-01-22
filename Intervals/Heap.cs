using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals
{
    internal class Heap<T> : ICollection<T>
    {
        internal const int DefaultSize = 10;

        private readonly IComparer<T> _Comparer;
        private int _Count;
        private T[] _Elements;
        private volatile int _Version;

        public Heap(IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            _Comparer = comparer;
            _Elements = new T[DefaultSize];
        }

        public int Version
        {
            get
            {
                return _Version;
            }
        }

        public int Capacity
        {
            get
            {
                return _Elements.Length;
            }

            set
            {
                if (value < 0)
                    throw new InvalidOperationException("Capacity must be greater than 0");
                if (value < _Count)
                    throw new InvalidOperationException("Capacity must be greater than or equal to the current number of elements in the heap");

                int newSize = Math.Max(value, DefaultSize);
                if (newSize != _Elements.Length)
                {
                    var newElements = new T[newSize];
                    if (_Elements.Length > 0)
                        Array.Copy(_Elements, 0, newElements, 0, Math.Min(_Elements.Length, value));
                    _Elements = newElements;
                    unchecked
                    {
                        _Version++;
                    }
                }
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _Count)
                    throw new ArgumentOutOfRangeException("index");
                return _Elements[index];
            }
        }

        public bool IsEmpty
        {
            get
            {
                return _Count == 0;
            }
        }

        #region ICollection<T> Members

        public int Count
        {
            get
            {
                return _Count;
            }
        }

        public virtual void Add(T value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (_Count == _Elements.Length)
                Capacity = Capacity * 2;
            Debug.Assert(_Count < _Elements.Length, "_Count must be less than _Elements.Length here");
            _Elements[_Count++] = value;

            SiftDown(0, _Count - 1);
            NewVersion();
        }

        public virtual bool Remove(T value)
        {
            if (Count == 0)
                return false;

            int index = IndexOf(value);
            if (index < 0)
                return false;

            RemoveAt(index);

            return true;
        }

        public virtual bool Contains(T value)
        {
            return IndexOf(value) >= 0;
        }

        public void Clear()
        {
            for (int index = 0; index < _Count; index++)
                _Elements[index] = default(T);
            _Count = 0;
            NewVersion();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int index = 0; index < _Count; index++)
                array[arrayIndex + index] = _Elements[index];
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            int version = _Version;
            for (int index = 0; index < _Count; index++)
            {
                if (version != _Version)
                    throw new InvalidOperationException("Cannot continue enumerator for heap, underlying heap was changed");

                yield return _Elements[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public T[] ToArray()
        {
            var result = new T[_Count];
            Array.Copy(_Elements, result, _Count);
            return result;
        }

        public virtual void RemoveAt(int index)
        {
            if (index < 0 || index >= _Count)
                throw new ArgumentOutOfRangeException("index");

            // Copy last element over element that is to be removed
            if (index < _Count - 1)
                _Elements[index] = _Elements[_Count - 1];

            // Get rid of last, duplicate, element
            _Count--;
            _Elements[_Count] = default(T);

            // Preserve heap criteria
            if (index < _Count)
                SiftUp(index);

            NewVersion();
        }

        public virtual int IndexOf(T value)
        {
            for (int index = 0; index < _Count; index++)
                if (_Comparer.Compare(value, _Elements[index]) == 0)
                    return index;

            return -1;
        }

        public virtual int ParentIndex(int index)
        {
            if (index < 0 || index >= _Count)
                throw new ArgumentOutOfRangeException("index");

            if (_Count == 0)
                return -1;
            if (index == 0)
                return -1;
            return (index - 1) / 2;
        }

        public virtual int LeftChildIndex(int index)
        {
            if (index < 0 || index >= _Count)
                throw new ArgumentOutOfRangeException("index");

            index = index * 2 + 1;
            if (index >= _Count)
                return -1;
            return index;
        }

        public virtual int RightChildIndex(int index)
        {
            if (index < 0 || index >= _Count)
                throw new ArgumentOutOfRangeException("index");

            index = index * 2 + 2;
            if (index >= _Count)
                return -1;
            return index;
        }

        public virtual void Push(T value)
        {
            Add(value);
        }

        public virtual T Pop()
        {
            if (_Count == 0)
                throw new InvalidOperationException("Cannot pop from the heap, it is currently empty");

            T lastElement = _Elements[--_Count];
            _Elements[_Count] = default(T);
            T returnItem;
            if (_Count > 0)
            {
                returnItem = _Elements[0];
                _Elements[0] = lastElement;
                SiftUp(0);
            }
            else
                returnItem = lastElement;

            NewVersion();

            return returnItem;
        }

        public virtual T ReplaceAt(int index, T newValue)
        {
            if (index < 0 || index >= _Count)
                throw new ArgumentOutOfRangeException("index");

            T returnElement = _Elements[index];
            if (index == 0)
            {
                _Elements[0] = newValue;
                SiftUp(0);

                NewVersion();
            }
            else
            {
                RemoveAt(index);
                Add(newValue);
            }

            return returnElement;
        }

        public virtual bool Replace(T value, T newValue)
        {
            int index = IndexOf(value);
            if (index < 0)
                return false;

            ReplaceAt(index, newValue);
            return true;
        }

        public virtual void TrimExcess()
        {
            if (_Count < _Elements.Length * 0.9)
                Capacity = _Count;
        }

        public Heap<T> Clone()
        {
            return (Heap<T>)((ICloneable)this).Clone();
        }

        protected virtual void SiftDown(int startPos, int pos)
        {
            T newItem = _Elements[pos];
            while (pos > startPos)
            {
                int parentPos = (pos - 1) / 2;
                T parent = _Elements[parentPos];
                if (_Comparer.Compare(parent, newItem) <= 0)
                    break;
                _Elements[pos] = parent;
                pos = parentPos;
            }

            _Elements[pos] = newItem;
            NewVersion();
        }

        protected virtual void SiftUp(int pos)
        {
            int endPos = _Count;
            int startPos = pos;
            T newItem = _Elements[pos];

            // Bubble up the smaller child until hitting a leaf.
            int childPos = 2 * pos + 1;

            while (childPos < endPos)
            {
                // Set childpos to index of smaller child.
                int rightPos = childPos + 1;
                if (rightPos < endPos && _Comparer.Compare(_Elements[rightPos], _Elements[childPos]) <= 0)
                    childPos = rightPos;

                // Move the smaller child up.
                _Elements[pos] = _Elements[childPos];
                pos = childPos;
                childPos = 2 * pos + 1;
            }

            _Elements[pos] = newItem;
            SiftDown(startPos, pos);
            NewVersion();
        }

        protected void NewVersion()
        {
            unchecked
            {
                _Version++;
            }
        }
    }
}