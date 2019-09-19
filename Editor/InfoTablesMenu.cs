using System;
using UnityEditor;
using UnityEngine;

namespace Gameframe.InfoTables.Editor
{
    public static class InfoTablesMenu
    {
        [MenuItem("Gameframe/InfoTables/ExportPath")]
        public static void SelectExportPath()
        {
            var settings = InfoTableSettings.Get();
            var path = EditorUtility.OpenFolderPanel("Export Folder", settings.exportPath, "");
            
            if (path.StartsWith(Application.dataPath)) {
                path =  "Assets" + path.Substring(Application.dataPath.Length);
            }

            if (string.IsNullOrEmpty(path))
            {
                Debug.Log($"Export path {settings.exportPath}");
                return;
            }
            
            if (EditorUtility.DisplayDialog("Path", $"Export to {path}?", "ok", "cancel"))
            {
                settings = InfoTableSettings.GetOrCreateAndSave();
                if (settings == null)
                {
                    return;
                }
                settings.exportPath = path;
            }

            Debug.Log($"Export path {settings.exportPath}");
        }
        
        [MenuItem("Gameframe/InfoTables/ExportAll")]
        public static void ExportAll()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(EnumExportableTable)}");
            
            if (guids.Length >= 0 && !EditorUtility.DisplayDialog("Export Info Table", "Exporting will write source code. It's recommended that you commit all current changes to version control before continuing. Continue?", "Ok", "Cancel"))
            {
                return;
            }
            
            //Find all assets of type T and add them to our list
            try
            {
                int count = 0;
                foreach (var guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath( guid );
                    EnumExportableTable asset = AssetDatabase.LoadAssetAtPath<EnumExportableTable>( assetPath );
                    Debug.Log($"Exporting {asset.name}");
                    EditorUtility.DisplayProgressBar("Export Info Tables",$"", count / (float)guids.Length);
                    asset.Export(true);
                    count++;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            EditorUtility.ClearProgressBar();
            Debug.Log($"Exported {guids.Length} Table(s)");
        }
    }
}


