using System;
using UnityEngine;

namespace Unitility.SelectionHistory
{
    [Serializable]
    public class HistoryBuffer<T>
    {
        public static HistoryBuffer<T> FromArray(T[] array, int current, int capacity)
        {
            var hb = new HistoryBuffer<T>(capacity);
            if (array == null) return hb;
            foreach (T item in array) hb.Push(item);
            hb.current = current;
            return hb;
        }
        
        private T[] array;
        private int capacity;

        private int current = -1;
        private int first;
        private int last;

        public int Size
        {
            get
            {
                if (this.current < 0) return 0;
                return IndexDistance(this.last, this.first) + 1;
            }
        }

        public HistoryBuffer(int capacity)
        {
            this.array = new T[capacity];
            this.capacity = capacity;
        }

        public void Push(T item)
        {
            if (this.current == -1)
            {
                this.first = this.last = this.current = 0;
            }
            else
            {
                this.current = Wrap(this.current + 1);
                this.first = this.current;
                if (this.current == this.last) this.last = Wrap(this.last + 1);
            }

            this.array[this.current] = item;
        }

        public T Current()
        {
            return this.array[this.current];
        }

        public T Previous()
        {
            if (this.current == this.last) return this.array[this.current];

            this.current = Wrap(this.current - 1);
            return this.array[this.current];
        }

        public T Next()
        {
            if (this.current == this.first) return this.array[this.current];

            this.current = Wrap(this.current + 1);
            return this.array[this.current];
        }

        public void SetCurrent(int index)
        {
            this.current = Wrap(this.last + index);
        }

        public void Clear()
        {
            this.current = -1;
            this.first = 0;
            this.last = 0;
        }

        private int Wrap(int index)
        {
            return (index + this.capacity) % this.capacity;
        }

        public T[] ToArray()
        {
            T[] objArray = new T[this.Size];
            for (int j = this.last, i = 0; i < this.Size; j = Wrap(j + 1), i++)
            {
                objArray[i] = this.array[j];
            }

            return objArray;
        }

        public int GetCurrentArrayIndex()
        {
            if (this.current == -1) return -1;
            return IndexDistance(this.last, this.current);
        }

        private int IndexDistance(int a, int b)
        {
            return Mathf.Abs(b >= a ? b - a : b - (a - this.capacity));
        }
    }

}