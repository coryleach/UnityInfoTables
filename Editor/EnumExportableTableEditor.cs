using UnityEngine;
using UnityEditor;

namespace Gameframe.InfoTables.Editor
{
    /// <summary>
    /// Adds an export button to the inspector of all EnumExportableTable object types
    /// </summary>
    [CustomEditor(typeof(EnumExportableTable),true)]
    public class EnumExportableTableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Gather"))
            {
                ((EnumExportableTable)target).GatherExportables();
            }
            
            if (GUILayout.Button("Validate"))
            {
                if (((EnumExportableTable) target).ValidateExportables())
                {
                    Debug.Log("OK!");
                }
            }
            
            if ( GUILayout.Button("Export") )
            {
                ((EnumExportableTable)target).Export();
                return;
            }
            
            EditorGUILayout.EndHorizontal();
            
        }
    }
}

