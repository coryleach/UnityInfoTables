using UnityEngine;

namespace Gameframe.InfoTables
{
    public class InfoTableUtility : MonoBehaviour
    {
        public static string SanitizeStringForId(string key)
        {
            return key.ToLowerInvariant().Replace(' ', '_').Replace("\'", "");
        }
    
        public static string SanitizeStringForEnum(string key)
        {
            return key.Replace(" ", "").Replace("\'", "");
        }
    }
}


