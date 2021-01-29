using UnityEditor;
using UnityEngine;

namespace ObjectPooling
{

    [CustomEditor(typeof(ObjectPool))]
    public class ObjectPoolEditor : Editor
    {
        ObjectPool objectPool;
        Color guiBackgroundColor;
        float totalPoolUsage;
        float currentPoolUsage;

        private void OnEnable()
        {
            objectPool = (ObjectPool)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            guiBackgroundColor = GUI.backgroundColor;

            currentPoolUsage = (objectPool.PoolSize > 0 ? (float)objectPool.UsedObjectCount / objectPool.PoolSize : 0);
            if (currentPoolUsage > 0.5f) GUI.backgroundColor = Color.Lerp(Color.yellow, Color.red, (currentPoolUsage - 0.5f) * 2.0f);
            else GUI.backgroundColor = Color.Lerp(guiBackgroundColor, Color.yellow, currentPoolUsage * 2.0f);
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), currentPoolUsage, $"Current pool Usage: {Mathf.RoundToInt(currentPoolUsage * 100)}%");

            totalPoolUsage = (objectPool.PoolSize > 0 ? (float)objectPool.UsedObjectCount / objectPool.MaxPoolSize : 0);
            if (totalPoolUsage > 0.5f) GUI.backgroundColor = Color.Lerp(Color.yellow, Color.red, (totalPoolUsage - 0.5f) * 2.0f);
            else GUI.backgroundColor = Color.Lerp(guiBackgroundColor, Color.yellow, totalPoolUsage * 2.0f);
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), totalPoolUsage, $"Total pool Usage: {Mathf.RoundToInt(totalPoolUsage * 100)}%");

            GUI.backgroundColor = guiBackgroundColor;

            EditorGUILayout.LabelField($"Objects Available: {objectPool.UnUsedObjectCount} / {objectPool.PoolSize}");
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }
    }
}