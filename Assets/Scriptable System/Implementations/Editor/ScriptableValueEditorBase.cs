using SA.ScriptableData;
using UnityEditor;

public abstract class ScriptableValueEditorBase<T, V> : Editor
	where T : ScriptableValue<V>
{
	protected SerializedProperty value;
	protected new T target => base.target as T;

	protected virtual void OnEnable()
	{
		value = serializedObject.FindProperty("value");
		AttachEvents();
	}

	protected virtual void OnDisable()
	{
		DetachEvents();
	}

	protected virtual void AttachEvents()
	{
		EditorApplication.update += Repaint;
	}

	protected virtual void DetachEvents()
	{
		EditorApplication.update -= Repaint;
	}

	protected virtual void CheckForModifiedProperties()
	{
		if(serializedObject.hasModifiedProperties)
		{
			V prevValue = target.Value;

			serializedObject.ApplyModifiedProperties();

			// Trigger events.
			target.OnValueChangedParams_EDITOR(prevValue, target.Value);
			target.OnValueChanged_EDITOR();
		}
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(value, true);

		CheckForModifiedProperties();
	}
}
