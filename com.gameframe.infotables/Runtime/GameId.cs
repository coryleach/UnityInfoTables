using System;
using UnityEngine;

namespace Gameframe.InfoTables
{
  /// <summary>
  /// GameId converts a human readable string to a numerical id
  /// Generally to be used as a key to associate data with assets and static data
  /// </summary>
  [Serializable]
  public class GameId : ISerializationCallbackReceiver
  {
    static GameId invalid = null;
    public static GameId Invalid
    {
      get
      {
        if (invalid == null)
        {
          invalid = new GameId();
        }
        return invalid;
      }
    }

    public GameId()
    {
    }

    public GameId(string key)
    {
      this.key = key;
      id = key.GetHashCode();
    }

    [SerializeField]
    string key = string.Empty;
    public string Key => key;

    [SerializeField, HideInInspector]
    int id = 0;

    public int Value => id;

    public bool IsValid()
    {
      return !string.IsNullOrEmpty(key);
    }

    public override bool Equals(object obj)
    {
      if ( !(obj is GameId) )
      {
        return false;
      }

      var other = (GameId)obj;
      return other.Value == this.Value;
    }

    public override int GetHashCode()
    {
      return Value;
    }

    public void OnBeforeSerialize()
    {
      id = string.IsNullOrEmpty(key) ? 0 : GetStableHashCode(key);
    }

    public void OnAfterDeserialize()
    {
      id = string.IsNullOrEmpty(key) ? 0 : GetStableHashCode(key);
    }
    
    private static int GetStableHashCode(string str)
    {
      unchecked
      {
        int hash1 = 5381;
        int hash2 = hash1;

        for(int i = 0; i < str.Length && str[i] != '\0'; i += 2)
        {
          hash1 = ((hash1 << 5) + hash1) ^ str[i];
          if (i == str.Length - 1 || str[i+1] == '\0')
            break;
          hash2 = ((hash2 << 5) + hash2) ^ str[i+1];
        }

        return hash1 + (hash2*1566083941);
      }
    }

    public override string ToString()
    {
      return $"GameId({Key}:{Value})";
    }
  }

}