using UnityEditor;
using UnityEngine;

namespace Gameframe.InfoTables
{
    public class InfoTableSettings : ScriptableObject
    {
        public static InfoTableSettings Get()
        {
            InfoTableSettings settings = null;
            
            var guids = AssetDatabase.FindAssets($"t:{typeof(InfoTableSettings)}");
            if (guids.Length > 0)
            {
                var guid = guids[0];
                string assetPath = AssetDatabase.GUIDToAssetPath( guid );
                settings = AssetDatabase.LoadAssetAtPath<InfoTableSettings>( assetPath );
            }

            if (settings == null)
            {
                //Create an instance with default values
                settings = CreateInstance<InfoTableSettings>();
            }

            return settings;
        }

        public static InfoTableSettings GetOrCreateAndSave()
        {
            InfoTableSettings settings = null;
            
            var guids = AssetDatabase.FindAssets($"t:{typeof(InfoTableSettings)}");
            if (guids.Length > 0)
            {
                var guid = guids[0];
                string assetPath = AssetDatabase.GUIDToAssetPath( guid );
                settings = AssetDatabase.LoadAssetAtPath<InfoTableSettings>( assetPath );
            }

            if (settings == null)
            {
                //Save a new settings file
                settings = CreateInstance<InfoTableSettings>();
                var path = EditorUtility.SaveFilePanel("Save InfoTableSettings", "Assets", "InfoTableSettings", "asset");
                
                if (string.IsNullOrEmpty(path))
                {
                    return null;
                }
                
                if (path.StartsWith(Application.dataPath)) 
                {
                    path =  "Assets" + path.Substring(Application.dataPath.Length);
                }
                
                AssetDatabase.CreateAsset(settings, path);
            }

            return settings;
        }

        public static InfoTableSettings Create()
        {
            //Save a new settings file
            var settings = CreateInstance<InfoTableSettings>();
            var path = EditorUtility.SaveFilePanel("Save InfoTableSettings", "Assets", "InfoTableSettings", "asset");
                
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
                
            if (path.StartsWith(Application.dataPath)) 
            {
                path =  "Assets" + path.Substring(Application.dataPath.Length);
            }
                
            AssetDatabase.CreateAsset(settings, path);
            return settings;
        }

        public string exportPath = "Assets/Exported/InfoTables";
    }
}