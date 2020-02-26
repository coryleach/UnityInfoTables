using UnityEngine;

namespace Gameframe.InfoTables
{
  /// <summary>
  /// Scriptable object that can be exported as part of an enum type
  /// </summary>
  public class EnumInfoScriptableObject : InfoScriptableObject, IEnumExportable
  {
    [SerializeField,Tooltip("This string will be used as the value name when exported to enum")] 
    private string exportName = string.Empty;

    /// <summary>
    /// The name of this object when exported to an enum type
    /// </summary>
    public string ExportName
    {
      get => exportName;
      protected set => exportName = value;
    }
    
    #region IEnumExportable
    ///<inheritdoc />
    public string GetEnumExportableName()
    {
      return exportName;
    }

    /// <inheritdoc/>
    public int GetEnumExportableValue()
    {
      return Id.Value;
    }
    #endregion
  }
}
