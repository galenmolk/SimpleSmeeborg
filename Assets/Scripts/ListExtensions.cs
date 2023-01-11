using System;
using System.Collections.Generic;

public static class ListExtensions
{
    public static bool ContainsElementWhere<T>(this List<T> list, Func<T, bool> predicate)
    {
        for (int i = 0, count = list.Count; i < count; i++)
        {
            if (predicate(list[i]))
            {
                return true;
            }
        }
        return false;
    }
}
