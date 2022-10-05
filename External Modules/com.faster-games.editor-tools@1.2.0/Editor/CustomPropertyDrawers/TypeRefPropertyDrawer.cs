using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FasterGames.EditorTools.Editor.CustomPropertyDrawers
{
    [CustomPropertyDrawer(typeof(TypeRef), useForChildren: false)]
    public class TypeRefPropertyDrawer : TextPopupPropertyDrawer
    {
        private List<Type> _typeCache = null;
        private string[] _typeNames = null;

        protected override string[] PopupText => _typeNames;
        protected override int CurrentIndex { get; set; } = NoItemIndex;
        
        protected override string RefreshButtonText => "Refresh Types";

        protected override void RefreshContent(SerializedProperty property)
        {
            _typeCache = FindAllTypes();
            _typeNames = _typeCache.Select(t => t.FullName).ToArray();

            var currentValue = property.FindPropertyRelative(TypeRef.FieldNames.TypeAssemblyQualifiedName)?.stringValue;

            if (string.IsNullOrWhiteSpace(currentValue))
            {
                return;
            }
            
            var currentType = Type.GetType(currentValue);
            var currentTypeFullName = currentType?.FullName;

            var foundIndex = _typeNames.ToList().IndexOf(currentTypeFullName);

            if (foundIndex > 0)
            {
                CurrentIndex = foundIndex;
            }
        }

        protected override void OnSelect(SerializedProperty property, string[] text, int oldIndex, int newIndex)
        {
            if (newIndex >= _typeCache.Count)
            {
                // refresh (on next GUI update)
                _typeCache = null;
                CurrentIndex = NoItemIndex;
            }
            else
            {
                // update
                property.FindPropertyRelative(TypeRef.FieldNames.TypeAssemblyQualifiedName).stringValue =
                    _typeCache[newIndex].AssemblyQualifiedName;
            }
        }

        private static List<Type> FindAllTypes() =>
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsPublic).ToList();
    }
}