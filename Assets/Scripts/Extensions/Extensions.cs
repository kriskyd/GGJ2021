using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UI;

public static class Extensions
{
	public static void CopyTo<T>(this T copyFrom, ref Component copyTo) where T : Component
	{
		Type type = copyTo.GetType();
		if(type != copyFrom.GetType()) return; // type mis-match
		BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
		PropertyInfo[] pinfos = type.GetProperties(flags);
		foreach(var pinfo in pinfos)
		{
			if(pinfo.CanWrite)
			{
				try
				{
					pinfo.SetValue(copyTo, pinfo.GetValue(copyFrom, null), null);
				}
				catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
			}
		}
		FieldInfo[] finfos = type.GetFields(flags);
		foreach(var finfo in finfos)
		{
			finfo.SetValue(copyTo, finfo.GetValue(copyFrom));
		}
		return;
	}

	public static void SetAutoNavigation(Selectable[] selectables, bool ignoreNotInteractable = true)
	{
		foreach(var v in selectables)
		{
			Navigation nav = v.navigation;
			nav.mode = Navigation.Mode.Explicit;
			v.navigation = nav;
		}

		foreach(var v in selectables)
		{
			Navigation nav = v.navigation;
			nav.selectOnDown = FindSelectable(v, selectables, Vector3.down, ignoreNotInteractable);
			nav.selectOnUp = FindSelectable(v, selectables, Vector3.up, ignoreNotInteractable);
			nav.selectOnLeft = FindSelectable(v, selectables, Vector3.left, ignoreNotInteractable);
			nav.selectOnRight = FindSelectable(v, selectables, Vector3.right, ignoreNotInteractable);
			v.navigation = nav;
		}
	}

	public static void SetNoNavigation(Selectable[] selectables)
	{
		foreach(var v in selectables)
		{
			Navigation nav = v.navigation;
			nav.mode = Navigation.Mode.None;
			v.navigation = nav;
		}
	}

	public static Selectable FindSelectable(Selectable origin, Selectable[] selectables, Vector3 dir, bool ignoreNotInteractable = true)
	{
		dir = dir.normalized;
		Vector3 localDir = Quaternion.Inverse(origin.transform.rotation) * dir;
		Vector3 pos = origin.transform.TransformPoint(GetPointOnRectEdge(origin.transform as RectTransform, localDir));
		float maxScore = Mathf.NegativeInfinity;
		Selectable bestPick = null;
		for(int i = 0; i < selectables.Length; ++i)
		{
			Selectable sel = selectables[i];

			if(sel == origin || sel == null || !sel.gameObject.activeInHierarchy)
				continue;

			if((!sel.IsInteractable() && ignoreNotInteractable) || sel.navigation.mode == Navigation.Mode.None)
				continue;

			var selRect = sel.transform as RectTransform;
			Vector3 selCenter = selRect != null ? (Vector3)selRect.rect.center : Vector3.zero;
			Vector3 myVector = sel.transform.TransformPoint(selCenter) - pos;

			float dot = Vector3.Dot(dir, myVector);

			if(dot <= 0)
				continue;

			float score = dot / myVector.sqrMagnitude;

			if(score > maxScore)
			{
				maxScore = score;
				bestPick = sel;
			}
		}
		return bestPick;
	}

	public static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
	{
		if(rect == null)
			return Vector3.zero;
		if(dir != Vector2.zero)
			dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
		dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
		return dir;
	}
}