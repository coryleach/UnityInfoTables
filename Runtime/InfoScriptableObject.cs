using UnityEngine;

namespace Gameframe.InfoTables
{
  /// <summary>
  /// Scriptable object with a GameId
  /// Intended to be used with an InfoTable
  /// </summary>
  public class InfoScriptableObject : ScriptableObject
  {
    [SerializeField,Tooltip("The GameId's hashed value will be used as the integer value when exported to enum")] 
    private GameId id;
    public GameId Id
    {
      get => id; 
      set => id = value;
    }
  }
}


