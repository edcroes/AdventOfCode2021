namespace AoC2021.Common;

public static class DictionaryExtensions
{
    public static void AddOrUpdate<T>(this Dictionary<T,int> dictionary, T key, int value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddOrUpdate<T>(this Dictionary<T, long> dictionary, T key, long value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }
}