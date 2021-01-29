using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtensions
{
	/// <summary>
	/// Returns value from <paramref name="dictionary"/> if <paramref name="key"/> exists
	/// or <paramref name="defaultValue"/> if <paramref name="key"/> doesn't exist.
	/// </summary>
	public static T GetValue<T>(this Dictionary<string, object> dictionary, string key, T defaultValue = default) where T : class
	{
		if(dictionary == null)
			return defaultValue;

		if(dictionary.TryGetValue(key, out object obj))
			return obj as T ?? defaultValue;

		return defaultValue;
	}

	public static string GetString(this Dictionary<string, object> dictionary, string key, string defaultValue = "")
	{
		return dictionary.GetValue(key, defaultValue);
	}

	public static bool GetBool(this Dictionary<string, object> dictionary, string key, bool defaultValue = false)
	{
		if(dictionary == null)
			return defaultValue;

		if(dictionary.TryGetValue(key, out object obj))
			return System.Convert.ToBoolean(obj);

		return defaultValue;
	}

	public static int GetInt(this Dictionary<string, object> dictionary, string key, int defaultValue = 0)
	{
		if(dictionary == null)
			return defaultValue;

		if(dictionary.TryGetValue(key, out object obj))
			return System.Convert.ToInt32(obj);

		return defaultValue;
	}

	public static double GetDouble(this Dictionary<string, object> dictionary, string key, double defaultValue = 0)
	{
		if(dictionary == null)
			return defaultValue;

		if(dictionary.TryGetValue(key, out object obj))
			return System.Convert.ToDouble(obj);

		return defaultValue;
	}

	public static float GetFloat(this Dictionary<string, object> dictionary, string key, float defaultValue = 0)
	{
		return (float)GetDouble(dictionary, key, defaultValue);
	}

	public static void TryAddValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
	{
		if(!dictionary.ContainsKey(key))
			dictionary.Add(key, value);
		else
			Debug.LogError($"Key <{key}> exists in dictionary. Cannot add new value.\n" +
				UnityEngine.StackTraceUtility.ExtractStackTrace());
	}

}
