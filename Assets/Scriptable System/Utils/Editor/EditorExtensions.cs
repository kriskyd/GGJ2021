using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.ScriptableData.Editor
{
	public static class EditorExtensions
	{
		public static class Collection
		{
			#region Array

			public static void DrawArrayProperty(SerializedObject target, SerializedProperty array, Action<SerializedProperty, int> arrayElementDrawer)
			{
				array.isExpanded = EditorGUILayout.Foldout(array.isExpanded, array.displayName);
				if(array.isExpanded)
				{
					EditorGUI.indentLevel++;
					array.arraySize = EditorGUILayout.DelayedIntField("Size", array.arraySize);

					bool contextCalled = Event.current.type == EventType.ContextClick;
					Rect rect = GUILayoutUtility.GetLastRect();

					for(int i = 0; i < array.arraySize; i++)
					{
						arrayElementDrawer(array.GetArrayElementAtIndex(i), i);

						if(Event.current.type == EventType.ContextClick)
						{
							contextCalled = true;
							rect = GUILayoutUtility.GetLastRect();

							if(rect.Contains(Event.current.mousePosition))
								DrawArrayElementContextMenu(target, array, i);
						}
						/// If event is used up by default context menu of array element,
						/// triggered by <see cref="EditorGUILayout.PropertyField"/>,
						/// go to proper rect area and trigger custom context menu.
						else if(contextCalled && Event.current.type == EventType.Used)
						{
							// rect.height doesn't return proper value for the first element on list
							// so it's better to use default line height.
							rect.y += (i == 0 ? EditorGUIUtility.singleLineHeight : rect.height) + EditorGUIUtility.standardVerticalSpacing;
							if(rect.Contains(Event.current.mousePosition))
								DrawArrayElementContextMenu(target, array, i);
						}
					}
					EditorGUI.indentLevel--;
				}
			}

			public static void DrawArrayElementContextMenu(SerializedObject target, SerializedProperty array, int index)
			{
				if(!array.isArray)
					return;

				GenericMenu menu = GetArrayElementContextMenu(target, array, index);

				menu.ShowAsContext();
			}

			public static void DrawArrayElementContextMenu(SerializedObject target, SerializedProperty array, int index, params KeyValuePair<string, GenericMenu.MenuFunction>[] additionalFunctions)
			{
				if(!array.isArray)
					return;

				GenericMenu menu = GetArrayElementContextMenu(target, array, index);

				menu.AddSeparator("");

				for(int i = 0; i < additionalFunctions.Length; i++)
					menu.AddItem(new GUIContent(additionalFunctions[i].Key), false, additionalFunctions[i].Value);

				menu.ShowAsContext();
			}

			public static GenericMenu GetArrayElementContextMenu(SerializedObject target, SerializedProperty array, int index)
			{
				GenericMenu menu = new GenericMenu();

				menu.AddItem(new GUIContent("Duplicate Array Element"), false,
					() => DuplicateArrayElementAtIndex(target, array, index));
				menu.AddItem(new GUIContent("Delete Array Element"), false,
					() => DeleteArrayElementAtIndex(target, array, index));
				menu.AddSeparator("");

				bool canMoveUp = index != 0;
				bool canMoveDown = index != array.arraySize - 1;

				if(canMoveUp)
					menu.AddItem(new GUIContent("Move Up"), false,
						() => MoveArrayElementUp(target, array, index));
				if(canMoveDown)
					menu.AddItem(new GUIContent("Move Down"), false,
						() => MoveArrayElementDown(target, array, index));

				return menu;
			}

			public static void DuplicateArrayElementAtIndex(SerializedObject target, SerializedProperty array, int index)
			{
				array.InsertArrayElementAtIndex(index);
				target.ApplyModifiedProperties();
			}

			public static void DeleteArrayElementAtIndex(SerializedObject target, SerializedProperty array, int index)
			{
				array.DeleteArrayElementAtIndex(index);
				target.ApplyModifiedProperties();
			}

			public static void MoveArrayElementUp(SerializedObject target, SerializedProperty array, int index)
			{
				array.MoveArrayElement(index, index - 1);
				target.ApplyModifiedProperties();
			}

			public static void MoveArrayElementDown(SerializedObject target, SerializedProperty array, int index)
			{
				array.MoveArrayElement(index, index + 1);
				target.ApplyModifiedProperties();
			}

			#endregion

			#region Dictionary

			public static void DrawDictionaryProperty(SerializedObject target, SerializedProperty keys, SerializedProperty values, Action<SerializedProperty, SerializedProperty, int, bool> dictionaryElementDrawer)
			{
				keys.isExpanded = EditorGUILayout.Foldout(keys.isExpanded, "Dictionary");
				if(keys.isExpanded)
				{
					EditorGUI.indentLevel++;
					int arraySize = keys.arraySize;
					arraySize = EditorGUILayout.DelayedIntField("Size", arraySize);
					keys.arraySize = arraySize;
					values.arraySize = arraySize;

					bool contextCalled = Event.current.type == EventType.ContextClick;
					Rect rect = GUILayoutUtility.GetLastRect();

					for(int i = 0; i < keys.arraySize; i++)
					{
						var key = keys.GetArrayElementAtIndex(i);
						bool keyDuplicated = false;
#if VALIDATE_DICTIONARIES
						for(int j = 0; j < i; j++)
						{
							if(EditorUtils.GetPropertyValue(keys.GetArrayElementAtIndex(j))
								.Equals(EditorUtils.GetPropertyValue(key)))
							{
								keyDuplicated = true;
								break;
							}
						}
#endif
						dictionaryElementDrawer(key, values.GetArrayElementAtIndex(i), i, keyDuplicated);
						if(Event.current.type == EventType.ContextClick)
						{
							contextCalled = true;
							rect = GUILayoutUtility.GetLastRect();

							if(rect.Contains(Event.current.mousePosition))
								DrawDictionaryElementContextMenu(target, keys, values, i);
						}
						/// If event is used up by default context menu of array element,
						/// triggered by <see cref="EditorGUILayout.PropertyField"/>,
						/// go to proper rect area and trigger custom context menu.
						else if(contextCalled && Event.current.type == EventType.Used)
						{
							// rect.height doesn't return proper value for the first element on list
							// so it's better to use default line height.
							rect.y += (i == 0 ? EditorGUIUtility.singleLineHeight : rect.height) + EditorGUIUtility.standardVerticalSpacing;
							if(rect.Contains(Event.current.mousePosition))
								DrawDictionaryElementContextMenu(target, keys, values, i);
						}
					}
					EditorGUI.indentLevel--;
				}
			}


			public static void DrawDictionaryElementContextMenu(SerializedObject target, SerializedProperty keys, SerializedProperty values, int index)
			{
				if(!keys.isArray || !values.isArray)
					return;

				GenericMenu menu = GetDictionaryElementContextMenu(target, keys, values, index);

				menu.ShowAsContext();
			}


			public static GenericMenu GetDictionaryElementContextMenu(SerializedObject target, SerializedProperty keys, SerializedProperty values, int index)
			{
				GenericMenu menu = new GenericMenu();

				menu.AddItem(new GUIContent("Duplicate Array Element"), false,
					() => DuplicateDictionaryElementAtIndex(target, keys, values, index));
				menu.AddItem(new GUIContent("Delete Array Element"), false,
					() => DeleteDictionaryElementAtIndex(target, keys, values, index));
				menu.AddSeparator("");

				bool canMoveUp = index != 0;
				bool canMoveDown = index != keys.arraySize - 1;

				if(canMoveUp)
					menu.AddItem(new GUIContent("Move Up"), false,
						() => MoveDictionaryElementUp(target, keys, values, index));
				if(canMoveDown)
					menu.AddItem(new GUIContent("Move Down"), false,
						() => MoveDictionaryElementDown(target, keys, values, index));

				return menu;
			}


			public static void DuplicateDictionaryElementAtIndex(SerializedObject target, SerializedProperty keys, SerializedProperty values, int index)
			{
				keys.InsertArrayElementAtIndex(index);
				values.InsertArrayElementAtIndex(index);
				target.ApplyModifiedProperties();
			}

			public static void DeleteDictionaryElementAtIndex(SerializedObject target, SerializedProperty keys, SerializedProperty values, int index)
			{
				int length = keys.arraySize;
				keys.DeleteArrayElementAtIndex(index);
				if(length == keys.arraySize)
					keys.DeleteArrayElementAtIndex(index);

				length = values.arraySize;
				values.DeleteArrayElementAtIndex(index);
				if(length == values.arraySize)
					values.DeleteArrayElementAtIndex(index);

				target.ApplyModifiedProperties();
			}

			public static void MoveDictionaryElementUp(SerializedObject target, SerializedProperty keys, SerializedProperty values, int index)
			{
				keys.MoveArrayElement(index, index - 1);
				values.MoveArrayElement(index, index - 1);
				target.ApplyModifiedProperties();
			}

			public static void MoveDictionaryElementDown(SerializedObject target, SerializedProperty keys, SerializedProperty values, int index)
			{
				keys.MoveArrayElement(index, index + 1);
				values.MoveArrayElement(index, index + 1);
				target.ApplyModifiedProperties();
			}



			#endregion
		}

		public static class Drawer
		{
			public static void FoldableObjectField(SerializedProperty property, System.Type objectType, bool allowSceneObjects = false)
			{
				using(new EditorGUILayout.HorizontalScope())
				{
					EditorModifiers.PushLabelWidth(Text.GetEditorLabelWidth(property.displayName, 1));
					property.objectReferenceValue = EditorGUILayout.ObjectField(property.displayName, property.objectReferenceValue, objectType, allowSceneObjects);
					EditorModifiers.PopLabelWidth();
					if(property.objectReferenceValue != null)
						property.isExpanded = EditorGUI.Foldout(GUILayoutUtility.GetLastRect(), property.isExpanded, "");
				}
			}

			public static void QuaternionField(SerializedProperty property)
			{
				var vector4 = new Vector4(property.quaternionValue.x, property.quaternionValue.y, property.quaternionValue.z, property.quaternionValue.w);
				vector4 = EditorGUILayout.Vector4Field(property.displayName, vector4);
				property.quaternionValue = new Quaternion(vector4.x, vector4.y, vector4.z, vector4.w);
			}
		}

		public static class EditorModifiers
		{
			private static Stack<float> labelWidth = new Stack<float>();
			private static Stack<int> indentLevel = new Stack<int>();
			private static Stack<Color> labelColor = new Stack<Color>();

			public static void PushLabelWidth(float newLabelWidth)
			{
				labelWidth.Push(EditorGUIUtility.labelWidth);
				EditorGUIUtility.labelWidth = newLabelWidth;
			}

			public static void PopLabelWidth()
			{
				if(labelWidth.Count == 0) return;
				EditorGUIUtility.labelWidth = labelWidth.Pop();
			}

			public static void PushIndentLevel(int newIndentLevel)
			{
				indentLevel.Push(EditorGUI.indentLevel);
				EditorGUI.indentLevel = newIndentLevel;
			}

			public static void PopIndentLevel()
			{
				if(indentLevel.Count == 0) return;
				EditorGUI.indentLevel = indentLevel.Pop();
			}

			public static void PushLabelColor(Color newLabelColor)
			{
				labelColor.Push(EditorStyles.label.normal.textColor);
				EditorStyles.label.normal.textColor = newLabelColor;
			}

			public static void PopLabelColor()
			{
				if(labelColor.Count == 0) return;
				EditorStyles.label.normal.textColor = labelColor.Pop();
			}

			public class IndentLevel : IDisposable
			{
				public IndentLevel(int indentLevel)
				{
					PushIndentLevel(indentLevel);
				}

				public void Dispose()
				{
					PopIndentLevel();
				}
			}

			public class LabelWidth : IDisposable
			{
				public LabelWidth(float labelWidth)
				{
					PushLabelWidth(labelWidth);
				}

				public void Dispose()
				{
					PopLabelWidth();
				}
			}

			public class LabelColor : IDisposable
			{
				public LabelColor(Color labelColor)
				{
					PushLabelColor(labelColor);
				}

				public void Dispose()
				{
					PopLabelColor();
				}
			}
		}

		public static class Text
		{
			public static float GetEditorTextWidth(string text)
			{
				float width = 0f;
				var font = EditorStyles.standardFont;
				font.RequestCharactersInTexture(text);
				char[] charray = text.ToCharArray();

				foreach(var c in charray)
				{
					font.GetCharacterInfo(c, out CharacterInfo characterInfo);
					width += characterInfo.advance;
				}

				return width;
			}

			public static float GetEditorLabelWidth(string label, int indentOffset = 0)
			{
				float width = 0f;
				var font = EditorStyles.standardFont;
				font.RequestCharactersInTexture(label);
				char[] charray = label.ToCharArray();

				foreach(var c in charray)
				{
					font.GetCharacterInfo(c, out CharacterInfo characterInfo);
					width += characterInfo.advance;
				}

				return width + (EditorGUI.indentLevel + indentOffset) * 20f;
			}
		}
	}
}