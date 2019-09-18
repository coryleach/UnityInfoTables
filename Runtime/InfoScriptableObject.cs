using UnityEngine;

namespace Gameframe.InfoTables
{
  /// <summary>
  /// Scriptable object with a InfoId
  /// Intended to be used with an InfoTable
  /// </summary>
  public class InfoScriptableObject : ScriptableObject
  {
    [SerializeField,Tooltip("The InfoId's hashed value will be used as the integer value when exported to enum")] 
    private InfoId id;
    public InfoId Id
    {
      get => id; 
      set => id = value;
    }
  }
}


