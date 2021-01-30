using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ColorExtensions
{
	public static Color FromHex(string hexString)
	{
		ColorUtility.TryParseHtmlString(hexString, out Color color);
		return color;
	}

	public static Color With(this Color value, float? r = null, float? g = null, float? b = null, float? a = null)
	{
		var result = new Color();

		result[0] = r ?? value.r;
		result[1] = g ?? value.g;
		result[2] = b ?? value.b;
		result[3] = a ?? value.a;

		return result;
	}

	/// <summary>
	/// Calculates the linear parameter t that produces the interpolant color between start and end.
	/// </summary>
	/// <param name="start"> Start value. </param>
	/// <param name="end"> End value. </param>
	/// <param name="color"> Value between start and end. </param>
	/// <param name="t">Percentage of value between start and end.</param>
	/// <returns></returns>
	public static bool TryInverseColorLerp(Color start, Color end, Color color, out float t)
	{
		t = 0;

		float r = InverseLerpColorValueWithNegativeFail(start.r, end.r, color.r);
		float g = InverseLerpColorValueWithNegativeFail(start.g, end.g, color.g);
		float b = InverseLerpColorValueWithNegativeFail(start.b, end.b, color.b);
		float a = InverseLerpColorValueWithNegativeFail(start.a, end.a, color.a);

		List<float> values = new List<float>() { r, g, b, a };

		for(int i = 0; i < values.Count - 1; i++)
			if(values[i] == -2)
				return false;

		for(int i = 0; i < values.Count - 1; i++)
			for(int j = i + 1; j < values.Count; j++)
				if(values[i] == -1 || values[j] == -1 || values[i] == values[j])
					continue;
				else
					return false;

		t = values.Max();
		t = t == -1 ? 0 : t;

		return true;
	}

	/// <summary>
	/// Inverses color value with special return signals.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="value"></param>
	/// <returns> -1 if all parameters are the same, -2 if lerp is not possible, proper value otherwise </returns>
	private static float InverseLerpColorValueWithNegativeFail(float a, float b, float value)
	{
		if(a == b && a == value)
			return -1; // inverse lerp won't work properly but value is within range

		if(value < Mathf.Min(a, b) || value > Mathf.Max(a, b))
			return -2; // value out of range

		return Mathf.Clamp01((value - a) / (b - a));
	}
}
