using System;
using System.Collections.Generic;

public static class ListExtension
{
    public static void Shuffle<T>(this IList<T> list, int seed)
    {
        Random rng = new Random(seed);
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T GetRandomItem<T>(this IList<T> list, int seed)
    {
        Random rng = new Random(seed);
        int randomIndex = rng.Next(list.Count);
        return list[randomIndex];
    }
}
