using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
	public static T Clone<T, V>(this T enumerable) where T : IEnumerable<V>, new() where V : IClonable<V>
	{
		if(enumerable == null)
			return new T();

		T copy = new T();
		foreach(var item in enumerable)
			if(item != null)
				copy.Append(item.Clone());
		return copy;
	}

	public static List<V> Clone<V>(this List<V> list) where V : IClonable<V>
	{
		if(list == null)
			return new List<V>();

		List<V> copy = new List<V>();
		foreach(var item in list)
			if(item != null)
				copy.Add(item.Clone());
		return copy;
	}

	public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
	{
		int index = 0;
		foreach(var element in collection)
		{
			if(predicate(element))
				return index;
			index++;
		}

		return -1;
	}

}

public interface IClonable<T>
{
	T Clone();
}