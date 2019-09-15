using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.InfoTables
{
  /// <summary>
  /// Table of InfoScriptableObjects each with a unique GameId
  /// </summary>
  public class InfoTable : ScriptableObject
  {
    [SerializeField]
    private List<InfoScriptableObject> properties = new List<InfoScriptableObject>();

    private Dictionary<int, InfoScriptableObject> _dictionary = null;

    /// <summary>
    /// Number of entries in the table
    /// </summary>
    public int Count => properties.Count;

    /// <summary>
    /// Gets a value out of the table by index
    /// </summary>
    /// <param name="index"></param>
    public InfoScriptableObject this[int index] => properties[index];

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
      _dictionary = new Dictionary<int, InfoScriptableObject>();

      if (properties == null)
      {
        return;
      }
    
      foreach (var property in properties)
      {
        _dictionary.Add(property.Id.Value, property);
      }
    }

    /// <inheritdoc cref="Get(Gameframe.InfoTables.GameId)"/>
    public InfoScriptableObject Get(GameId gameId)
    {
      return Get(gameId.Value);
    }

    /// <summary>
    /// Get an InfoScrpitableObject reference from the table
    /// </summary>
    /// <param name="id">id of the info scriptable object to be retrieved</param>
    /// <returns>InfoScriptableObject associated with id</returns>
    /// <exception cref="Exception">throws exception if id is not found</exception>
    public InfoScriptableObject Get(int id)
    {
      BuildDictionary();
      try
      {
        return _dictionary[id];
      }
      catch (KeyNotFoundException e)
      {
        throw new Exception($"EnumExportableInfoTable<{typeof(InfoScriptableObject)}> {name} does not contain key {id}");
      }
    }
    
    /// <summary>
    /// Try to get an InfoScriptableObject
    /// </summary>
    /// <param name="id">id of the object to be found</param>
    /// <param name="val">val will be assigned the value of the found object</param>
    /// <returns>True if a scriptable object with the given id was found otherwise false</returns>
    public bool TryGet(int id, out InfoScriptableObject val)
    {
      BuildDictionary();
      return _dictionary.TryGetValue(id, out val);
    }

    /// <inheritdoc cref="TryGet(int,out Gameframe.InfoTables.InfoScriptableObject)"/>
    public bool TryGet(GameId gameId, out InfoScriptableObject val)
    {
      return TryGet(gameId.Value, out val);
    }
    
  }
  
  /// <inheritdoc cref="InfoTable"/>
  public class InfoTable<T> : ScriptableObject where T : InfoScriptableObject
  {
    [SerializeField]
    private List<T> properties = new List<T>();

    private Dictionary<int, T> _dictionary = null;

    /// <summary>
    /// Number of entries in the table
    /// </summary>
    public int Count => properties.Count;

    /// <summary>
    /// Gets a value out of the table by index
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

    /// <inheritdoc cref="Get(Gameframe.InfoTables.GameId)"/>
    public T Get(GameId gameId)
    {
      return Get(gameId.Value);
    }

    /// <summary>
    /// Get an InfoScrpitableObject reference from the table
    /// </summary>
    /// <param name="id">id of the info scriptable object to be retrieved</param>
    /// <returns>InfoScriptableObject associated with id</returns>
    /// <exception cref="Exception">throws exception if id is not found</exception>
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
    
    /// <summary>
    /// Try to get an InfoScriptableObject
    /// </summary>
    /// <param name="id">id of the object to be found</param>
    /// <param name="val">val will be assigned the value of the found object</param>
    /// <returns>True if a scriptable object with the given id was found otherwise false</returns>
    public bool TryGet(int id, out T val)
    {
      BuildDictionary();
      return _dictionary.TryGetValue(id, out val);
    }

    /// <inheritdoc cref="TryGet(int,out T)"/>
    public bool TryGet(GameId gameId, out T val)
    {
      return TryGet(gameId.Value, out val);
    }
    
  }
  
}

