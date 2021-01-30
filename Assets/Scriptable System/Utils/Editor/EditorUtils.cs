using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class EditorUtils
{
	public static object GetPropertyValue(SerializedProperty prop)
	{
		if(prop == null) throw new System.ArgumentNullException("prop");

		switch(prop.propertyType)
		{
			case SerializedPropertyType.Integer:
				return prop.intValue;
			case SerializedPropertyType.Boolean:
				return prop.boolValue;
			case SerializedPropertyType.Float:
				return prop.floatValue;
			case SerializedPropertyType.String:
				return prop.stringValue;
			case SerializedPropertyType.Color:
				return prop.colorValue;
			case SerializedPropertyType.ObjectReference:
				return prop.objectReferenceValue;
			case SerializedPropertyType.LayerMask:
				return (LayerMask)prop.intValue;
			case SerializedPropertyType.Enum:
				return prop.enumValueIndex;
			case SerializedPropertyType.Vector2:
				return prop.vector2Value;
			case SerializedPropertyType.Vector3:
				return prop.vector3Value;
			case SerializedPropertyType.Vector4:
				return prop.vector4Value;
			case SerializedPropertyType.Rect:
				return prop.rectValue;
			case SerializedPropertyType.ArraySize:
				return prop.arraySize;
			case SerializedPropertyType.Character:
				return (char)prop.intValue;
			case SerializedPropertyType.AnimationCurve:
				return prop.animationCurveValue;
			case SerializedPropertyType.Bounds:
				return prop.boundsValue;
			case SerializedPropertyType.Gradient:
				throw new System.InvalidOperationException("Can not handle Gradient types.");
		}

		return null;
	}

	public static Type GetTargetType(this SerializedObject obj)
	{
		if(obj == null) return null;

		if(obj.isEditingMultipleObjects)
		{
			var c = obj.targetObjects[0];
			return c.GetType();
		}
		else
		{
			return obj.targetObject.GetType();
		}
	}

	public static Type GetTargetType(this SerializedProperty prop)
	{
		if(prop == null) return null;

		System.Reflection.FieldInfo field;
		switch(prop.propertyType)
		{
			case SerializedPropertyType.Generic:
				return TypeUtils.FindType(prop.type) ?? typeof(object);
			case SerializedPropertyType.Integer:
				return prop.type == "long" ? typeof(long) : typeof(int);
			case SerializedPropertyType.Boolean:
				return typeof(bool);
			case SerializedPropertyType.Float:
				return prop.type == "double" ? typeof(double) : typeof(float);
			case SerializedPropertyType.String:
				return typeof(string);
			case SerializedPropertyType.Color:
				field = GetFieldOfProperty(prop);
				return field != null ? field.FieldType : typeof(Color);
			case SerializedPropertyType.ObjectReference:
				field = GetFieldOfProperty(prop);
				return field != null ? field.FieldType : typeof(UnityEngine.Object);
			case SerializedPropertyType.LayerMask:
				return typeof(LayerMask);
			case SerializedPropertyType.Enum:
				field = GetFieldOfProperty(prop);
				return field != null ? field.FieldType : typeof(System.Enum);
			case SerializedPropertyType.Vector2:
				return typeof(Vector2);
			case SerializedPropertyType.Vector3:
				return typeof(Vector3);
			case SerializedPropertyType.Vector4:
				return typeof(Vector4);
			case SerializedPropertyType.Rect:
				return typeof(Rect);
			case SerializedPropertyType.ArraySize:
				return typeof(int);
			case SerializedPropertyType.Character:
				return typeof(char);
			case SerializedPropertyType.AnimationCurve:
				return typeof(AnimationCurve);
			case SerializedPropertyType.Bounds:
				return typeof(Bounds);
			case SerializedPropertyType.Gradient:
				return typeof(Gradient);
			case SerializedPropertyType.Quaternion:
				return typeof(Quaternion);
			case SerializedPropertyType.ExposedReference:
				field = GetFieldOfProperty(prop);
				return field != null ? field.FieldType : typeof(UnityEngine.Object);
			case SerializedPropertyType.FixedBufferSize:
				return typeof(int);
			case SerializedPropertyType.Vector2Int:
				return typeof(Vector2Int);
			case SerializedPropertyType.Vector3Int:
				return typeof(Vector3Int);
			case SerializedPropertyType.RectInt:
				return typeof(RectInt);
			case SerializedPropertyType.BoundsInt:
				return typeof(BoundsInt);
			default:
				field = GetFieldOfProperty(prop);
				return field != null ? field.FieldType : typeof(object);
		}
	}

	public static FieldInfo GetFieldOfProperty(SerializedProperty prop)
	{
		if(prop == null) return null;

		var tp = GetTargetType(prop.serializedObject);
		if(tp == null) return null;

		var path = prop.propertyPath.Replace(".Array.data[", "[");
		var elements = path.Split('.');
		System.Reflection.FieldInfo field;
		foreach(var element in elements.Take(elements.Length - 1))
		{
			if(element.Contains("["))
			{
				var elementName = element.Substring(0, element.IndexOf("["));
				var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));

				field = tp.GetMember(elementName, MemberTypes.Field, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault() as System.Reflection.FieldInfo;
				if(field == null) return null;
				tp = field.FieldType;
			}
			else
			{
				field = tp.GetMember(element, MemberTypes.Field, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault() as System.Reflection.FieldInfo;
				if(field == null) return null;
				tp = field.FieldType;
			}
		}
		return null;
	}
}

public static class TypeUtils
{
	public static Type FindType(string typeName, bool useFullName = false, bool ignoreCase = false)
	{
		if(string.IsNullOrEmpty(typeName)) return null;

		bool isArray = typeName.EndsWith("[]");
		if(isArray)
			typeName = typeName.Substring(0, typeName.Length - 2);

		StringComparison e = (ignoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
		if(useFullName)
		{
			foreach(var assemb in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach(var t in assemb.GetTypes())
				{
					if(string.Equals(t.FullName, typeName, e))
					{
						if(isArray)
							return t.MakeArrayType();
						else
							return t;
					}
				}
			}
		}
		else
		{
			foreach(var assemb in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach(var t in assemb.GetTypes())
				{
					if(string.Equals(t.Name, typeName, e) || string.Equals(t.FullName, typeName, e))
					{
						if(isArray)
							return t.MakeArrayType();
						else
							return t;
					}
				}
			}
		}
		return null;
	}

	public static Type FindType(string typeName, Type baseType, bool useFullName = false, bool ignoreCase = false)
	{
		if(string.IsNullOrEmpty(typeName)) return null;
		if(baseType == null) throw new System.ArgumentNullException("baseType");

		bool isArray = typeName.EndsWith("[]");
		if(isArray)
			typeName = typeName.Substring(0, typeName.Length - 2);

		StringComparison e = (ignoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
		if(useFullName)
		{
			foreach(var assemb in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach(var t in assemb.GetTypes())
				{
					if(baseType.IsAssignableFrom(t) && string.Equals(t.FullName, typeName, e))
					{
						if(isArray)
							return t.MakeArrayType();
						else
							return t;
					}
				}
			}
		}
		else
		{
			foreach(var assemb in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach(var t in assemb.GetTypes())
				{
					if(baseType.IsAssignableFrom(t) && (string.Equals(t.Name, typeName, e) || string.Equals(t.FullName, typeName, e)))
					{
						if(isArray)
							return t.MakeArrayType();
						else
							return t;
					}
				}
			}
		}

		return null;
	}
}