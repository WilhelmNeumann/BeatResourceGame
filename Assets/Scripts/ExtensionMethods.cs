using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionMethods
{
    // List extensions

    public static T Find<T>(this List<T> list, Func<T, int, bool> predicate)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (predicate(list[i], i))
            {
                return list[i];
            }
        }
        return default;
    }

    public static List<T> FindAll<T>(this List<T> list, Func<T, int, bool> predicate)
    {
        List<T> result = new List<T>();
        for (int i = 0; i < list.Count; i++)
        {
            if (predicate(list[i], i))
            {
                result.Add(list[i]);
            }
        }
        return result;
    }

    public static int FindIndex<T>(this List<T> list, Func<T, int, bool> predicate)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (predicate(list[i], i))
            {
                return i;
            }
        }
        return -1;
    }

    public static void ForEach<T>(this List<T> list, Action<T, int> action)
    {
        for (int i = 0; i < list.Count; i++)
        {
            action(list[i], i);
        }
    }

    public static List<S> ConvertAll<T, S>(this List<T> list, Func<T, int, S> predicate)
    {
        List<S> result = new List<S>();
        for (int i = 0; i < list.Count; i++)
        {
            result.Add(predicate(list[i], i));
        }
        return result;
    }

    // Dictionary extensions
	
    public static S SafeGet<T, S>(this Dictionary<T, S> dictionary, T key, S defaultValue = default)
    {
        return dictionary.ContainsKey(key) ? dictionary[key] : defaultValue;
    }

    public static S AddOrSet<T, S>(this Dictionary<T, S> dictionary, T key, S value)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, value);
        }
        else
        {
            dictionary[key] = value;
        }
        return value;
    }

    public static void ForEach<Key, Value>(this Dictionary<Key, Value> dictionary, Action<Key, Value> action)
    {
        dictionary.Keys.ToList().ForEach(a => action(a, dictionary[a]));
    }

    public static Dictionary<NewKey, NewValue> ConvertAll<OldKey, OldValue, NewKey, NewValue>(this Dictionary<OldKey, OldValue> dictionary, Func<OldKey, OldValue, (NewKey, NewValue)> predicate)
    {
        Dictionary<NewKey, NewValue> result = new Dictionary<NewKey, NewValue>();
        dictionary.ForEach((key, value) =>
        {
            (NewKey key, NewValue value) newItem = predicate(key, value);
            result.Add(newItem.key, newItem.value);
        });
        return result;
    }
}
