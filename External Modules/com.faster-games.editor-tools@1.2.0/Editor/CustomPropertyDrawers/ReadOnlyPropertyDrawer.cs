using FasterGames.EditorTools.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace FasterGames.EditorTools.Editor.CustomPropertyDrawers
{
    /// <summary>
    /// A property drawer for <see cref="ReadOnlyAttribute"/>
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}