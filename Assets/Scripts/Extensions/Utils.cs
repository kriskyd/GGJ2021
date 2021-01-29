using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	/// <summary>
	/// Simple modulo function.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="mod"></param>
	/// <returns>Returns value with the same sign as <paramref name="mod"/></returns>
	public static int Mod(int value, int mod)
	{
		return (value % mod + mod) % mod;
	}
}
