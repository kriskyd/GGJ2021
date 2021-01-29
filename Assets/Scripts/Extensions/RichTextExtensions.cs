using System.Collections.Generic;
using UnityEngine;

public static class RichText
{
	public static string AddTagColor(this string text, Color c)
	{
		return "<color=#" + ColorUtility.ToHtmlStringRGBA(color: c) + ">" + text + "</color>";
	}

	public static string AddTagColor(this string text, string hexRGBA)
	{
		return "<color=#" + hexRGBA + ">" + text + "</color>";
	}

	public static string AddTagUnderline(this string text)
	{
		return "<u>" + text + "</u>";
	}
}
