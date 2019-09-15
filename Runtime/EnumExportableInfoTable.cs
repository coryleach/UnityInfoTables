using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.InfoTables
{
  public abstract class EnumExportableInfoTable<T> : EnumExportableScriptableObject where T : GenericInfoScriptableObject
  {
    [SerializeField]
    protected List<T> properties = new List<T>();

    public int Count => properties.Count;

    public T this[int i] => properties[i];

    private Dictionary<int, T> _dictionary = null;

    private void OnEnable()
    {
      BuildDictionary();
    }

    protected virtual void BuildDictionary(bool force = false)
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

    public virtual T Get(int id)
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
    
    public virtual bool TryGet(int id, out T val)
    {
      BuildDictionary();
      return _dictionary.TryGetValue(id, out val);
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

