using UnityEngine;
using UnityEditor;

namespace Gameframe.InfoTables.Editor
{
    /// <summary>
    /// Adds an export button to the inspector of all EnumExportableTable object types
    /// </summary>
    [CustomEditor(typeof(BaseInfoTable),true)]
    public class InfoTableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Gather"))
            {
                ((BaseInfoTable)target).GatherEntries();
            }

            if (GUILayout.Button("Validate"))
            {
                if (((BaseInfoTable) target).ValidateEntries())
                {
                    Debug.Log("OK!");
                }
            }

            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
