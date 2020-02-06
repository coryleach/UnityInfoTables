using UnityEditor;
using UnityEngine;

namespace Gameframe.InfoTables.Editor
{
    [CustomEditor(typeof(InfoTableProvider))]
    public class InfoTableProviderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Gather"))
            {
                ((InfoTableProvider)target).GatherEntries();
            }
            
            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
            
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}

