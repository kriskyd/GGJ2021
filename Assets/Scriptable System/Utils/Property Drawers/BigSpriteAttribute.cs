using System;
using UnityEngine;

public class BigSpriteAttribute : PropertyAttribute
{
	public float Width { get; }

	public float Height { get; }

	public BigSpriteAttribute()
	{
		Width = 64f;
		Height = 64f;
	}

	public BigSpriteAttribute(float width, float height)
	{
		Width = width;
		Height = height;
	}
}