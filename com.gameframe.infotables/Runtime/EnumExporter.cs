using UnityEditor;
using System.IO;
using System.Text;

namespace Gameframe.InfoTables
{
    public interface IEnumExportable
    {
        string GetEnumExportableName();
        int GetEnumExportableValue();
    }

#if UNITY_EDITOR
    public static class EnumExporter
    {
        public static void BuildEnum(string enumName, IEnumExportable[] enumEntries, string path)
        {
            string filename = path + enumName + ".cs";
            string fileContent = BuildEnumExportString(enumName, enumEntries);
            File.WriteAllText(filename, fileContent);
            AssetDatabase.ImportAsset(filename);
        }

        static string BuildEnumExportString(string enumName, IEnumExportable[] enumEntries)
        {
            var output = new StringBuilder();
            output.AppendFormat("public enum {0} : int \n{{\n", enumName);
            for (int i = 0; i < enumEntries.Length; i++)
            {
                output.AppendFormat("  {0}={1},\n", enumEntries[i].GetEnumExportableName(), enumEntries[i].GetEnumExportableValue());
            }
            output.AppendLine("}");
            return output.ToString();
        }
    }
#endif
}

