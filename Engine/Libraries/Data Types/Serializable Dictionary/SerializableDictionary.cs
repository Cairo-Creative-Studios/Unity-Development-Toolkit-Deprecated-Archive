using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class SerializableDictionary<Key,Value>
{
    private Dictionary<Key, Value> _dictionary = new Dictionary<Key, Value>();
    [HideInInspector] public List<Key> Keys = new List<Key>();
    [HideInInspector] public List<Value> Values = new List<Value>();
    [HideInInspector] public Key[] KeyArray;
    [HideInInspector] public Value[] ValueArray;
    public List<DictionaryItem> dictionary = new List<DictionaryItem>();

    public Value this[Key key]
    {
        get
        {
            return _dictionary[key];
        }
        set
        {
            if (Keys.Contains(key))
                Keys[FindKeyIndex(key)] = key;
            else
                Keys.Add(key);
            if (Values.Contains(_dictionary[key]))
                Values[FindValueIndex(_dictionary[key])] = value;
            else
                Values.Add(value);
            CreateArrays();
            _dictionary[key] = value;
        }
    }

    public SerializableDictionary()
    {
    }

    public SerializableDictionary(List<DictionaryItem> dictionary)
    {
        foreach(DictionaryItem item in dictionary)
        {
            _dictionary.Add(item.key, item.value);
            Keys.Add(item.key);
            Values.Add(item.value);
            CreateArrays();
        }
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public void Add(Key key, Value value)
    {
        _dictionary.Add(key, value);

        if (Keys.Contains(key))
            Keys[FindKeyIndex(key)] = key;
        else
            Keys.Add(key);
        if (Values.Contains(_dictionary[key]))
            Values[FindValueIndex(_dictionary[key])] = value;
        else
            Values.Add(value);
        CreateArrays();
    }

    public bool TryAdd(Key key, Value value)
    {
        bool result = _dictionary.TryAdd(key, value);

        if (result)
        {
            if (Keys.Contains(key))
                Keys[FindKeyIndex(key)] = key;
            else
                Keys.Add(key);
            if (Values.Contains(_dictionary[key]))
                Values[FindValueIndex(_dictionary[key])] = value;
            else
                Values.Add(value);
            CreateArrays();
        }

        return _dictionary.TryAdd(key, value);
    }

    public void Remove(Key key)
    {
        if(_dictionary.ContainsKey(key))
        {
            Keys.Remove(key);
            Values.Remove(_dictionary[key]);
            _dictionary.Remove(key);
            CreateArrays();
        }
    }

    public bool ContainsKey(Key key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool ContainsValue(Value value)
    {
        return _dictionary.ContainsValue(value);
    }

    public int EnsureCapacity(int capacity)
    {
        return _dictionary.EnsureCapacity(capacity);
    }

    public override int GetHashCode()
    {
        return _dictionary.GetHashCode();
    }

    private void CreateArrays()
    {
        KeyArray = Keys.ToArray();
        ValueArray = Values.ToArray();

        dictionary.Clear();
        foreach(Key key in _dictionary.Keys)
        {
            dictionary.Add(new DictionaryItem(key, _dictionary[key]));
        }
    }
    

    private int FindKeyIndex(Key key)
    {
        for (int i = 0; i < Keys.Count; i++)
        {
            if (Keys[i].Equals(key))
                return i;
        }
        return -1;
    }

    private int FindValueIndex(Value value)
    {
        for(int i = 0; i < Values.Count; i++)
        {
            if (Values[i].Equals(value))
                return i;
        }
        return -1;
    }


    [Serializable]
    public class DictionaryItem
    {
        public Key key;
        public Value value;

        public DictionaryItem(Key key, Value value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
