using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameframe.InfoTables
{
    [CreateAssetMenu(menuName = "Gameframe/InfoTableProvider")]
    public class InfoTableProvider : ScriptableObject
    {
        private static InfoTableProvider _current = null;

        public static InfoTableProvider Current
        {
            get => _current;
            set => _current = value;
        }
        
        [SerializeField]
        private List<BaseInfoTable> tables = new List<BaseInfoTable>();

        [NonSerialized]
        private readonly Dictionary<Type, List<BaseInfoTable>> _tableDictionary = new Dictionary<Type, List<BaseInfoTable>>();
        
        private void OnEnable()
        {
            if (_current == null)
            {
                _current = this;
            }
            
            for (int i = 0; i < tables.Count; i++)
            {
                var table = tables[i];
                
                if (table == null)
                {
                    continue;
                }

                var tableType = table.GetType();
                
                if (!_tableDictionary.TryGetValue(tableType, out var list))
                {
                    list = new List<BaseInfoTable>();
                    _tableDictionary.Add(tableType,list);
                }
                
                list.Add(table);
            }
        }
        
        public T Get<T>() where T : BaseInfoTable
        {
            if (!_tableDictionary.TryGetValue(typeof(T), out var val))
            {
                return null;
            }
            
            if (val.Count == 1)
            {
                return (T)val[0];
            }
            
            return null;
        }
        
        public IEnumerable<T> GetAll<T>() where T : BaseInfoTable
        {
            if (!_tableDictionary.TryGetValue(typeof(T), out var val))
            {
                return null;
            }
            return val.Cast<T>();
        }
        
        #if UNITY_EDITOR
        
        public void GatherEntries()
        {
            //Remove Null Entries
            tables.RemoveAll((x) => x == null);
      
            //Find all assets of type T and add them to our list
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(BaseInfoTable)}");
            foreach (var guid in guids)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath( guid );
                BaseInfoTable asset = UnityEditor.AssetDatabase.LoadAssetAtPath<BaseInfoTable>( assetPath );
                if( asset != null && !tables.Contains(asset) )
                {
                    tables.Add(asset);
                }
            }
        }
        
        #endif
        
    }
}

