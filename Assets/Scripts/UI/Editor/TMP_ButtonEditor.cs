using UnityEditor;
using UnityEditor.UI;
using UnityEditor.AnimatedValues;
using UnityEngine.UI;
using TMPro;

[CustomEditor(typeof(TMP_Button), true)]
[CanEditMultipleObjects]
public class TMPButtonEditor : ButtonEditor
{
	SerializedProperty m_Text;
	SerializedProperty m_TextTransition;
	SerializedProperty m_ColorBlockProperty;

	AnimBool m_ShowTextTint = new AnimBool();

	protected override void OnEnable()
	{
		base.OnEnable();
		m_Text = serializedObject.FindProperty("targetText");
		m_TextTransition = serializedObject.FindProperty("textTransition");
		m_ColorBlockProperty = serializedObject.FindProperty("textColors");

		var trans = GetTransition(m_TextTransition);
		m_ShowTextTint.value = (trans == Selectable.Transition.ColorTint);

		m_ShowTextTint.valueChanged.AddListener(Repaint);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		m_ShowTextTint.valueChanged.RemoveListener(Repaint);
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.Space();

		serializedObject.Update();

		var trans = GetTransition(m_TextTransition);
		var text = m_Text.objectReferenceValue as TextMeshProUGUI;

		m_ShowTextTint.target = (!m_TextTransition.hasMultipleDifferentValues && trans == Button.Transition.ColorTint);

		EditorGUILayout.PropertyField(m_TextTransition);
		if(trans == Selectable.Transition.Animation || trans == Selectable.Transition.SpriteSwap)
		{
			EditorGUILayout.HelpBox("Only color transition supported for text.", MessageType.Warning);
		}

		++EditorGUI.indentLevel;
		{
			EditorGUILayout.PropertyField(m_Text);

			if(text == null)
			{
				EditorGUILayout.HelpBox("You must have a TextMeshProUGUI target in order to use a color transition.", MessageType.Warning);
			}

			if(EditorGUILayout.BeginFadeGroup(m_ShowTextTint.faded))
			{
				EditorGUILayout.PropertyField(m_ColorBlockProperty);
			}
			EditorGUILayout.EndFadeGroup();
		}
		--EditorGUI.indentLevel;

		serializedObject.ApplyModifiedProperties();
	}

	static Selectable.Transition GetTransition(SerializedProperty transition)
	{
		return (Selectable.Transition)transition.enumValueIndex;
	}

}