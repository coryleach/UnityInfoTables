using UnityEditor;
using System.IO;
using System.Text;

namespace Gameframe.InfoTables
{
    /// <summary>
    /// Classes implementing IEnumExportable have a string and integer pair that can be used by the EnumExporter
    /// to generate an enum with the given type name equal to the paired value
    /// </summary>
    public interface IEnumExportable
    {
        /// <summary>
        /// Gets a string that will be used as the name of the exported enum value.
        /// </summary>
        /// <returns>String which will be used as the name of the exported enum value</returns>
        string GetEnumExportableName();
        
        /// <summary>
        /// Get an integer value representing this object's enum value
        /// </summary>
        /// <returns>Integer value that the named enum value will represent</returns>
        int GetEnumExportableValue();
    }

#if UNITY_EDITOR
    /// <summary>
    /// Contains utility methods used to export C# files containing a programatically generated enum struct
    /// </summary>
    public static class EnumExporter
    {
        /// <summary>
        /// Writes out a .cs file containing an enum with the given name and entries at the given path
        /// filename will be $"{enumName}.cs"
        /// </summary>
        /// <param name="enumName">Name of the enum type</param>
        /// <param name="enumEntries">Array of IEnumExportable that become the enum values</param>
        /// <param name="path">directory where file will be created</param>
        public static void BuildEnum(string enumName, IEnumExportable[] enumEntries, string path)
        {
            string filename = $"{path}/{enumName}.cs";
            string fileContent = BuildEnumExportString(enumName, enumEntries);
            File.WriteAllText(filename, fileContent);
            AssetDatabase.ImportAsset(filename);
        }
        
        /// <summary>
        /// Writes out a .cs file containing extension methods for enum and table class types
        /// </summary>
        /// <param name="tableClassName">Name of hte table class</param>
        /// <param name="infoClassName">Name of the info class</param>
        /// <param name="enumName">Name of the enum</param>
        /// <param name="path">Direcotry where the source code file will be written</param>
        public static void BuildExtensionMethods(string tableClassName, string infoClassName, string enumName, string path) 
        {
            var filename = $"{path}/{tableClassName}Extensions.cs";
            var tableExtensions = BuildTableEnumExtensionString(tableClassName, infoClassName, enumName);
            var infoExtensions = BuildInfoEnumExtensionString(infoClassName, enumName);
            var fileContent = $"{tableExtensions}{infoExtensions}";
            
            File.WriteAllText(filename, fileContent);
            AssetDatabase.ImportAsset(filename);
        }

        /// <summary>
        /// Builds an extension class for the table that allows use of the generated enum as an argument to the Get method
        /// </summary>
        /// <param name="tableClassName">Name of hte table class</param>
        /// <param name="infoClassName">Name of the info class</param>
        /// <param name="enumName">Name of the enum</param>
        /// <returns>Source code for a static class that includes extension methods</returns>
        private static string BuildTableEnumExtensionString(string tableClassName, string infoClassName, string enumName)
        {
            var output = new StringBuilder();
            output.AppendLine("");
            output.AppendLine($"public static class {tableClassName}Extensions \n{{");
            output.AppendLine($"  public static {infoClassName} Get(this {tableClassName} table, {enumName} enumValue)");
            output.AppendLine("  {");
            output.AppendLine("    return table.Get((int) enumValue);");
            output.AppendLine("  }");
            output.AppendLine("}");
            return output.ToString();
        }
        
        /// <summary>
        /// Builds an extension class for the info entries that gets the id types as the enum
        /// </summary>
        /// <param name="infoClassName">Name of the info class</param>
        /// <param name="enumName">Name of the enum</param>
        /// <returns>Source code for a static class that includes extension methods</returns>
        private static string BuildInfoEnumExtensionString(string infoClassName, string enumName)
        {
            var output = new StringBuilder();
            output.AppendLine("");
            output.AppendLine($"public static class {infoClassName}Extensions \n{{");
            output.AppendLine($"  public static {enumName} GetEnumValue(this {infoClassName} info)");
            output.AppendLine("  {");
            output.AppendLine($"    return ({enumName})info.Id.Value;");
            output.AppendLine("  }");
            output.AppendLine("}");
            return output.ToString();
        }

        /// <summary>
        /// Builds a string representing the soruce code for an enum type
        /// </summary>
        /// <param name="enumName">Name of the enum type</param>
        /// <param name="enumEntries">Array of named values for the enum</param>
        /// <returns>source code for the enum type</returns>
        static string BuildEnumExportString(string enumName, IEnumExportable[] enumEntries)
        {
            var output = new StringBuilder();
            output.AppendFormat("public enum {0} : int \n{{\n", enumName);
            foreach (var enumEntry in enumEntries)
            {
                output.AppendFormat("  {0}={1},\n", enumEntry.GetEnumExportableName(), enumEntry.GetEnumExportableValue());
            }
            output.AppendLine("}");
            return output.ToString();
        }
    }
#endif
    
}

