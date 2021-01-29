using System;
using System.Text;
using UnityEngine;

public static class StringExtensions
{
	static StringBuilder stringBuilder = new StringBuilder();
	const string charCollection = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_=+";

	public static string ToLower(this string value, int startIndex = 0, int count = 1)
	{
		if(value.IsNullOrEmpty())
			return value;

		if(startIndex < 0 || startIndex >= value.Length)
		{
			throw new IndexOutOfRangeException();
		}

		stringBuilder.Clear();
		int i = 0;

		while(i < startIndex)
		{
			stringBuilder.Append(value[i]);
			i++;
		}

		while(i < value.Length && i < count)
		{
			stringBuilder.Append(char.ToLower(value[i]));
			i++;
		}

		while(i < value.Length)
		{
			stringBuilder.Append(value[i]);
			i++;
		}

		string result = stringBuilder.ToString();
		stringBuilder.Clear();

		return result;
	}

	public static bool IsNullOrEmpty(this string value)
	{
		return string.IsNullOrEmpty(value);
	}

	public static bool IsNotNullOrEmpty(this string value)
	{
		return !string.IsNullOrEmpty(value);
	}

	public static bool IsNull(this string value)
	{
		return value == null;
	}

	public static bool IsNotNull(this string value)
	{
		return value != null;
	}

	public static string RemoveSpaces(this string value)
	{
		return value.Replace(" ", string.Empty);
	}

	public static string ToTitleCase(this string value)
	{
		System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
		string title = textInfo.ToTitleCase(value.ToLower());
		return title;
	}

	public static bool TryFormat(this string s, out string formatted, params string[] arr)
	{
		bool good = true;
		try
		{
			formatted = string.Format(s, arr);
		}
		catch(Exception)
		{
			good = false;
			formatted = s;
		}

		return good;
	}

	public static string RandomString(int length = 32, string customCharacters = null)
	{
		var characterSet = customCharacters ?? charCollection;
		stringBuilder = new StringBuilder(length);

		for(int i = 0; i < length; i++)
		{
			var index = UnityEngine.Random.Range(0, characterSet.Length);
			stringBuilder[i] = characterSet[index];
		}

		return stringBuilder.ToString();
	}

	public static string FormatColor(this string text, Color c)
	{
		return "<color=#" + ColorUtility.ToHtmlStringRGBA(color: c) + ">" + text + "</color>";
	}

	public static string FormatColor(this string text, string hexRGBA)
	{
		return "<color=#" + hexRGBA + ">" + text + "</color>";
	}
}
