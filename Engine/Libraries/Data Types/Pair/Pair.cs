//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;

[Serializable]
public class PairCollection<A,B>
{
    List<object> a = new List<object>();
    List<object> b = new List<object>();
    public List<Pair> serializedPairs = new List<Pair>();

    public A this[B b]
    {
        /// <summary>
        /// Gets the A value of the Pair from the given B value
        /// </summary>
        /// <returns>The item.</returns>
        get
        {
            for(int i = 0; i < a.Count; i++)
            {
                if(this.b[i] == (object)b)
                {
                    return (A)a[i];
                }
            }
            return default(A);
        }
        /// <summary>
        /// Sets the A value from the given B value
        /// </summary>
        set
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (this.b[i] == (object)b)
                {
                    a[i] = value;
                }
            }
        }
    }

    public B this[A a]
    {
        /// <summary>
        /// Gets the B value of the Pair from the given A value
        /// </summary>
        /// <returns>The item.</returns>
        get
        {
            for (int i = 0; i < b.Count; i++)
            {
                if (this.a[i] == (object)a)
                {
                    return (B)b[i];
                }
            }
            return default(B);
        }
        /// <summary>
        /// Sets the B value from the given A value
        /// </summary>
        set
        {
            for (int i = 0; i < b.Count; i++)
            {
                if (this.a[i] == (object)a)
                {
                    b[i] = value;
                }
            }
        }
    }

    /// <summary>
    /// Returns true if the Pair Contains the given value
    /// </summary>
    /// <returns>The contains.</returns>
    public bool Contains(A a)
    {
        foreach(A item in this.a)
        {
            if((object) item == (object)a)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns true if the Pair Contains the given value
    /// </summary>
    /// <returns>The contains.</returns>
    public bool Contains(B b)
    {
        foreach (B item in this.b)
        {
            if ((object)item == (object)b)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Gets the A Values of the pair as an Arrays
    /// </summary>
    /// <returns>The o array.</returns>
    public A[] AToArray()
    {
        List<A> AList = new List<A>();

        foreach(object item in a)
        {
            AList.Add((A)item);
        }

        return AList.ToArray();
    }

    /// <summary>
    /// Gets the B values of the Pair as an Array
    /// </summary>
    /// <returns>The o array.</returns>
    public B[] BToArray()
    {
        List<B> BList = new List<B>();

        foreach (object item in b)
        {
            BList.Add((B)item);
        }

        return BList.ToArray();
    }

    public void OnBeforeSerialize()
    {
        serializedPairs.Clear();

        foreach (var value in a)
        {
            serializedPairs.Add(new Pair((A)value, this[(A)value]));
        }
    }

    public void OnAfterDeserialize()
    {
        a.Clear();
        b.Clear();

        for (var i = 0; i < serializedPairs.Count; ++i)
        {
            //TODO: Add Serialized Pair values to the A and B Lists
        }
    }

    [Serializable]
    public class Pair
    {
        public A a;
        public B b;

        public Pair(A a, B b)
        {
            this.a = a;
            this.b = b;
        }
    }
}