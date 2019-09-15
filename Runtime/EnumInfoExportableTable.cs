using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.InfoTables
{
  /// <summary>
  /// Base class for an info table
  /// EnumExportableInfoTables contain a list of EnumInfoScriptableObject references
  /// </summary>
  /// <typeparam name="T">Type that inherits from EnumInfoScriptableObject</typeparam>
  public abstract class EnumInfoExportableTable<T> : EnumExportableTable where T : EnumInfoScriptableObject
  {
    [SerializeField]
    protected List<T> properties = new List<T>();

    private Dictionary<int, T> _dictionary = null;

    /// <summary>
    /// Number of entries in the table
    /// </summary>
    public int Count => properties.Count;

    /// <summary>
    /// Get an entry in the table by index
    /// </summary>
    /// <param name="index"></param>
    public T this[int index] => properties[index];

    private void OnEnable()
    {
      BuildDictionary();
    }

    private void BuildDictionary(bool force = false)
    {
      if (_dictionary != null && !force)
      {
        return;
      }
      _dictionary = new Dictionary<int, T>();

      if (properties == null)
      {
        return;
      }
    
      foreach (var property in properties)
      {
        _dictionary.Add(property.Id.Value, property);
      }
    }

    /// <summary>
    /// Get an entry in the table by GameId value
    /// </summary>
    /// <param name="id">id of entry to retrieve</param>
    /// <returns>Entry in the table with the given id</returns>
    /// <exception cref="Exception">Throws exception when id is not found in the table</exception>
    public T Get(int id)
    {
      BuildDictionary();
      try
      {
        return _dictionary[id];
      }
      catch (KeyNotFoundException e)
      {
        throw new Exception($"EnumExportableInfoTable<{typeof(T)}> {name} does not contain key {id}");
      }
    }

    /// <inheritdoc cref="Get(int)"/>
    public T Get(GameId gameId)
    {
      return Get(gameId.Value);
    }
    
    /// <summary>
    /// Try to get an entry out of the table
    /// </summary>
    /// <param name="id">id of entry</param>
    /// <param name="val">Will be assigned with reference to entry if found</param>
    /// <returns>True if entry is found. False otherwise</returns>
    public bool TryGet(int id, out T val)
    {
      BuildDictionary();
      return _dictionary.TryGetValue(id, out val);
    }
    
    /// <inheritdoc cref="TryGet(int,out T)"/>
    public bool TryGet(GameId gameId, out T val)
    {
      BuildDictionary();
      return _dictionary.TryGetValue(gameId.Value, out val);
    }

    #region Enum Export
    protected override IEnumExportable[] GetExportables()
    {
      //Building the dictionary should also validate we have no dupes
      BuildDictionary(true);
      var list = properties.ConvertAll((property) => property as IEnumExportable);
      return list.ToArray();
    }
    #endregion
    
  }
}

