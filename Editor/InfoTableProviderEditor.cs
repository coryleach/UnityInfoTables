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
            if (GUILayout.Button("Gather"))
            {
                ((InfoTableProvider)target).GatherEntries();
            }
        }
    }
}

