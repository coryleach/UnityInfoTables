using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    private bool includeExtensionMethods = true;
    
    [FormerlySerializedAs("properties")]
    [SerializeField]
    protected List<T> entries = new List<T>();

    private Dictionary<int, T> _dictionary = null;

    /// <summary>
    /// Number of entries in the table
    /// </summary>
    public int Count => entries.Count;

    /// <summary>
    /// Get an entry in the table by index
    /// </summary>
    /// <param name="index"></param>
    public T this[int index] => entries[index];

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

      if (entries == null)
      {
        return;
      }
    
      foreach (var property in entries)
      {
        _dictionary.Add(property.Id.Value, property);
      }
    }

    /// <summary>
    /// Get an entry in the table by InfoId value
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
    public T Get(InfoId infoId)
    {
      return Get(infoId.Value);
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
    public bool TryGet(InfoId infoId, out T val)
    {
      BuildDictionary();
      return _dictionary.TryGetValue(infoId.Value, out val);
    }

    #region Enum Export
    protected override IEnumExportable[] GetExportables()
    {
      //Building the dictionary should also validate we have no dupes
      //BuildDictionary(true);
      var list = entries.ConvertAll((property) => property as IEnumExportable);
      return list.ToArray();
    }
    #endregion
    
#if UNITY_EDITOR
    protected override void BuildAndWriteExportables(string enumName, IEnumExportable[] exportables, string path)
    {
      if (includeExtensionMethods)
      {
        EnumExporter.BuildEnumWithExtensionMethods(GetType().ToString(),typeof(T).ToString(),enumName,exportables,path);
      }
      else
      {
        base.BuildAndWriteExportables(enumName,exportables,path);
      }
    }

    public override void GatherEntries()
    {
      //Remove Null Entries
      entries.RemoveAll((x) => x == null);
      
      //Find all assets of type T and add them to our list
      var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
      foreach (var guid in guids)
      {
        string assetPath = AssetDatabase.GUIDToAssetPath( guid );
        T asset = AssetDatabase.LoadAssetAtPath<T>( assetPath );
        if( asset != null && !entries.Contains(asset) )
        {
          entries.Add(asset);
        }
      }
    }
#endif
    
  }
}

