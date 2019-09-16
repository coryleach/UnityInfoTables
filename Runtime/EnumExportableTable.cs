using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine.Windows;
using Directory = System.IO.Directory;

namespace Gameframe.InfoTables
{
    /// <summary>
    /// Serves as the base class for a scriptable object that can export an enum type
    /// </summary>
    public abstract class EnumExportableTable : ScriptableObject
    {
        protected virtual string ExportPath => "Assets/Exported/InfoTables/";
        
        protected abstract string ExportedEnumTypeName { get; }
        protected abstract IEnumExportable[] GetExportables();

        [SerializeField]
        private bool exportInvalidEntry = false;
        
        [SerializeField]
        private string invalidEntryName = "Invalid";

        private class InvalidEntry : IEnumExportable
        {
            private string name = "Invalid";
            public InvalidEntry(string entryName)
            {
                name = entryName;
            }
            public string GetEnumExportableName() => name;
            public int GetEnumExportableValue() => 0;
        }
        
#if UNITY_EDITOR
        public void Export()
        {
            if (!ValidateExportables())
            {
                Debug.LogError("Export Failed");
                return;
            }
            
            if (!Directory.Exists(ExportPath))
            {
                if (!EditorUtility.DisplayDialog("Export", "Export directory does not exist. Create it?", "Ok", "Cancel"))
                {
                    return;
                }
                //Create export directory
                Directory.CreateDirectory(ExportPath);
            }

            var entries = GetExportables();

            if (exportInvalidEntry)
            {
                entries = entries.Append(new InvalidEntry(invalidEntryName)).ToArray();
            }
            
            //Building the dictionary should also validate we have no dupes
            BuildAndWriteExportables(ExportedEnumTypeName, entries, ExportPath);
        }

        protected virtual void BuildAndWriteExportables(string enumName, IEnumExportable[] exportables, string path)
        {
            EnumExporter.BuildEnum(enumName,exportables,path);
        }
        
        public bool ValidateExportables()
        {
            var exportables = GetExportables();

            if (exportables.Contains(null))
            {
                Debug.LogError("Null value found");
                return false;
            }
            
            //Check for duplicate references
            if (exportables.Intersect(exportables).Count() != exportables.Count())
            {
                Debug.LogError("The same entry found twice!");
                return false;
            }
            
            //Check for duplicate values
            foreach (var exportable in exportables)
            {
                var value = exportable.GetEnumExportableValue();
                var duplicates = exportables.Where((x) => x.GetEnumExportableValue() == value);
                if (duplicates.Count() > 1)
                {
                    Debug.LogError("Duplicate Ids Found");
                    foreach (var dupe in duplicates)
                    {
                        Debug.LogError($"{dupe} ({dupe.GetEnumExportableName()}:{dupe.GetEnumExportableValue()})");
                    }
                    return false;
                }
            }

            int invalids = exportables.Count((x) => x.GetEnumExportableValue() == 0);
            if (invalids > 0)
            {
                Debug.LogError("Invalid id found");
                return false;
            }
            
            return true;
        }
        
        public virtual void GatherExportables()
        {
            
        }
#endif
    }
}

