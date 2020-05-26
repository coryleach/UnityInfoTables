using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gameframe.InfoTables
{
  /// <summary>
  /// Base class for all info tables
  /// </summary>
  public abstract class BaseInfoTable : ScriptableObject
  {
#if UNITY_EDITOR
    public virtual void GatherEntries()
    {
    }
    public virtual bool ValidateEntries()
    {
      return true;
    }
#endif
  }
  
  /// <summary>
  /// Table of InfoScriptableObjects each with a unique InfoId
  /// </summary>
  public class InfoTable : BaseInfoTable, IEnumerable<InfoScriptableObject>
  {
    [SerializeField]
    private List<InfoScriptableObject> entries = new List<InfoScriptableObject>();

    private Dictionary<int, InfoScriptableObject> _dictionary = null;

    /// <summary>
    /// Number of entries in the table
    /// </summary>
    public int Count => entries.Count;

    /// <summary>
    /// Gets a value out of the table by index
    /// </summary>
    /// <param name="index"></param>
    public InfoScriptableObject this[int index] => entries[index];

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

      if (entries == null)
      {
        return;
      }
    
      foreach (var property in entries)
      {
        _dictionary.Add(property.Id.Value, property);
      }
    }

    /// <inheritdoc cref="Get(InfoId)"/>
    public InfoScriptableObject Get(InfoId infoId)
    {
      return Get(infoId.Value);
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
        throw new Exception($"EnumExportableInfoTable<{typeof(InfoScriptableObject)}> {name} does not contain key {id}. {e}");
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
    public bool TryGet(InfoId infoId, out InfoScriptableObject val)
    {
      return TryGet(infoId.Value, out val);
    }
    
    public IEnumerator<InfoScriptableObject> GetEnumerator()
    {
      return entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
    
    protected void SortEntries(IComparer<InfoScriptableObject> comparer)
    {
      entries.Sort(comparer);
    }
    
    protected void SortEntries(Comparison<InfoScriptableObject> comparison)
    {
      entries.Sort(comparison);
    }
    
  }
  
  /// <inheritdoc cref="InfoTable"/>
  public class InfoTable<T> : BaseInfoTable, IEnumerable<T> where T : InfoScriptableObject
  {
    [FormerlySerializedAs("properties")]
    [SerializeField]
    private List<T> entries = new List<T>();

    private Dictionary<int, T> _dictionary = null;

    /// <summary>
    /// Number of entries in the table
    /// </summary>
    public int Count => entries.Count;

    /// <summary>
    /// Gets a value out of the table by index
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

    /// <inheritdoc cref="Get(InfoId)"/>
    public T Get(InfoId infoId)
    {
      return Get(infoId.Value);
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
        throw new Exception($"EnumExportableInfoTable<{typeof(T)}> {name} does not contain key {id}. {e}");
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
    public bool TryGet(InfoId infoId, out T val)
    {
      return TryGet(infoId.Value, out val);
    }

    protected void SortEntries(IComparer<T> comparer)
    {
      entries.Sort(comparer);
    }
    
    protected void SortEntries(Comparison<T> comparison)
    {
      entries.Sort(comparison);
    }

#if UNITY_EDITOR
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

    public override bool ValidateEntries()
    {
            if (entries.Contains(null))
            {
                Debug.LogError("Null value found");
                return false;
            }
            
            //Check for duplicate references
            if (entries.Intersect(entries).Count() != entries.Count())
            {
                Debug.LogError("The same entry found twice!");
                return false;
            }

            //Check for duplicate id values
            foreach (var entry in entries)
            {
                var duplicates = entries.Where((x) => x.Id.Value == entry.Id.Value);
                if (duplicates.Count() > 1)
                {
                    Debug.LogError("Duplicate Ids Found");
                    foreach (var dupe in duplicates)
                    {
                        Debug.LogError($"{dupe} ({dupe.Id})");
                    }
                    return false;
                }
            }
            
            var invalids = entries.Count((x) => !x.Id.IsValid());
            if (invalids > 0)
            {
                Debug.LogError("Invalid id found");
                return false;
            }
            
            return true;
    }
#endif

    public IEnumerator<T> GetEnumerator()
    {
      return entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
    
  }
  
}

