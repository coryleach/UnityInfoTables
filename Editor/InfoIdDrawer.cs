using UnityEngine;
using UnityEditor;

namespace Gameframe.InfoTables.Editor
{
  [CustomPropertyDrawer(typeof(InfoId))]
  public class InfoIdDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(position, label, property);
      EditorGUI.PropertyField(position, property.FindPropertyRelative("key"), label);
      EditorGUI.EndProperty();
    }
  }
}