using UnityEngine;
using System.Linq;
using Directory = System.IO.Directory;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gameframe.InfoTables
{
    /// <summary>
    /// Serves as the base class for a scriptable object that can export an enum type
    /// </summary>
    public abstract class EnumExportableTable : BaseInfoTable
    {        
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
        
        public void Export(bool skipDialog = false)
        {
            if (!ValidateEntries())
            {
                Debug.LogError("Export Failed");
                return;
            }
            
            if (!skipDialog && !EditorUtility.DisplayDialog("Export Info Table", "Exporting will write source code. It's recommended that you commit all current changes to version control before continuing. Continue?", "Ok", "Cancel"))
            {
                return;
            }

            var settings = InfoTableSettings.Get();
            if (settings == null)
            {
                return;
            }
            
            if (!Directory.Exists(settings.exportPath))
            {
                if (!EditorUtility.DisplayDialog("Export Info Table", "Export directory does not exist. Create it?", "Ok", "Cancel"))
                {
                    return;
                }
                //Create export directory
                Directory.CreateDirectory(settings.exportPath);
            }
            
            var entries = GetExportables();

            if (exportInvalidEntry)
            {
                entries = entries.Append(new InvalidEntry(invalidEntryName)).ToArray();
            }
            
            //Building the dictionary should also validate we have no dupes
            BuildAndWriteExportables(ExportedEnumTypeName, entries, settings.exportPath);
        }

        protected virtual void BuildAndWriteExportables(string enumName, IEnumExportable[] exportables, string path)
        {
            var tableNamespace = GetType().Namespace;
            EnumExporter.BuildEnum(tableNamespace,enumName,exportables,path);
        }
        
        public override bool ValidateEntries()
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
            
            //Check for duplicate names
            foreach (var exportable in exportables)
            {
                var valueName = exportable.GetEnumExportableName();
                var duplicates = exportables.Where((x) => x.GetEnumExportableName() == valueName);
                if (duplicates.Count() > 1)
                {
                    Debug.LogError($"Duplicate Enum Value Names Found in {name}");
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
#endif
    }
}

