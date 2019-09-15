using UnityEngine;

namespace Gameframe.InfoTables
{
  public class GenericInfoScriptableObject : ScriptableObject, IEnumExportable
  {
    [SerializeField]
    GameId id;
    public GameId Id
    {
      get => id; 
      set => id = value;
    }

    [SerializeField]
    string displayName;
    public string DisplayName => displayName;

    public string GetEnumExportableName()
    {
      var split = id.Key.Split('_');
      return split[split.Length - 1];
    }

    public int GetEnumExportableValue()
    {
      return id.Value;
    }
  }
}
