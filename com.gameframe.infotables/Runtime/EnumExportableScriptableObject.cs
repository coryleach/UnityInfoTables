using UnityEngine;

namespace Gameframe.InfoTables
{
    public abstract class EnumExportableScriptableObject : ScriptableObject
    {
        protected const string exportPath = "Assets/Exported/";

        protected abstract string ExportedEnumTypeName { get; }
        protected abstract IEnumExportable[] GetExportables();

#if UNITY_EDITOR
        public virtual void Export()
        {
            //Building the dictionary should also validate we have no dupes
            EnumExporter.BuildEnum(ExportedEnumTypeName, GetExportables(), exportPath);
        }
#endif

    }
}

