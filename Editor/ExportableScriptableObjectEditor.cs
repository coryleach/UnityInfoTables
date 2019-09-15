using UnityEngine;
using UnityEditor;

namespace Gameframe.InfoTables.Editor
{
    [CustomEditor(typeof(EnumExportableScriptableObject),true)]
    public class ExportableScriptableObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if ( GUILayout.Button("Export") )
            {
                ((EnumExportableScriptableObject)target).Export();
            }
        }
    }
}

