using System;
using System.Windows.Input;

namespace Canvas
{
    internal class StylusPointQueue
    {
        public StylusPointQueue(int capacity)
        {
            this._array = new StylusPoint[capacity];
            this._head = 0;
            this._tail = 0;
            this._size = 0;
            this._arrayLength = capacity;
        }

        public int Count
        {
            get
            {
                return this._size;
            }
        }

        public StylusPoint this[int n]
        {
            get
            {
                return this._array[(this._head + n) % this._arrayLength];
            }
            set
            {
                this._array[(this._head + n) % this._arrayLength] = value;
            }
        }

        public void Enqueue(StylusPoint item)
        {
            this._array[this._tail] = item;
            this._tail = (this._tail + 1) % this._arrayLength;
            this._size++;
            this._version++;
        }

        public StylusPoint Dequeue()
        {
            StylusPoint result = this._array[this._head];
            this._head = (this._head + 1) % this._arrayLength;
            this._size--;
            this._version++;
            return result;
        }

        public void CopyToStylusPointCollection(StylusPointCollection strokeTipCollection)
        {
            if (strokeTipCollection == null)
            {
                throw new ArgumentNullException("strokeTipCollection");
            }
            strokeTipCollection.Clear();
            for(int i = 0; i < _arrayLength; i++)
            {
                if (_array[i].Description.PropertyCount == 3)
                {
                    continue;
                }
                strokeTipCollection.Add(_array[i]);
            }
        }

        private readonly StylusPoint[] _array;

        private int _head;

        private int _tail;

        private int _size;

        private int _version;

        private readonly int _arrayLength;
    }
}
