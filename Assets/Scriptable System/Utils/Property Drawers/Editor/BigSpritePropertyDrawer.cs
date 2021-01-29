using UnityEngine;
using UnityEditor;

namespace SA.ScriptableData.Editor
{
	[CustomPropertyDrawer(typeof(BigSpriteAttribute))]
	public class BigSpritePropertyDrawer : PropertyDrawer
	{
		private bool useAttributeSize;
		private float width;
		private float height;

		public BigSpritePropertyDrawer() : base()
		{
			useAttributeSize = true;
		}

		public BigSpritePropertyDrawer(float width, float height) : base()
		{
			useAttributeSize = false;
			this.width = width;
			this.height = height;
		}

		private static GUIStyle s_TempStyle = new GUIStyle();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorExtensions.EditorModifiers.PushIndentLevel(0);

			Rect spriteRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			property.objectReferenceValue = EditorGUI.ObjectField(spriteRect, property.displayName, property.objectReferenceValue, typeof(Sprite), false);

			if(Event.current.type != EventType.Repaint || property.objectReferenceValue == null)
				return;

			//draw a sprite
			Sprite sp = property.objectReferenceValue as Sprite;

			spriteRect.y += EditorGUIUtility.singleLineHeight + 4;

			if(useAttributeSize)
			{
				BigSpriteAttribute bigSprite = (BigSpriteAttribute)attribute;
				this.width = bigSprite.Width;
				this.height = bigSprite.Height;
			}

			spriteRect.width = this.width;
			spriteRect.height = this.height;
			s_TempStyle.normal.background = sp.texture;
			s_TempStyle.Draw(spriteRect, GUIContent.none, false, false, false, false);

			EditorExtensions.EditorModifiers.PopIndentLevel();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) + height;
		}
	}
}