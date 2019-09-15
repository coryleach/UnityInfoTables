using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.Windows;
using Directory = System.IO.Directory;

namespace Gameframe.InfoTables
{
    /// <summary>
    /// Serves as the base class for a scriptable object that can export an enum type
    /// </summary>
    public abstract class EnumExportableTable : ScriptableObject
    {
        protected virtual string ExportPath => "Assets/Exported/";
        protected abstract string ExportedEnumTypeName { get; }
        protected abstract IEnumExportable[] GetExportables();

#if UNITY_EDITOR
        public virtual void Export()
        {
            if (!Directory.Exists(ExportPath))
            {
                if (!EditorUtility.DisplayDialog("Export", "Export directory does not exist. Create it?", "Ok", "Cancel"))
                {
                    return;
                }
                //Create export directory
                Directory.CreateDirectory(ExportPath);
            }
            //Building the dictionary should also validate we have no dupes
            EnumExporter.BuildEnum(ExportedEnumTypeName, GetExportables(), ExportPath);
        }
#endif
    }
}

