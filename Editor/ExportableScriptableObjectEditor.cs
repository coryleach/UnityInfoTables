using UnityEngine;
using UnityEditor;

namespace Gameframe.InfoTables.Editor
{
    /// <summary>
    /// Adds an export button to the inspector of all EnumExportableScriptable object types
    /// </summary>
    [CustomEditor(typeof(EnumExportableTable),true)]
    public class ExportableScriptableObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if ( GUILayout.Button("Export") )
            {
                ((EnumExportableTable)target).Export();
            }
        }
    }
}

