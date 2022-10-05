using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace FasterGames.EditorTools.Editor.CustomPropertyDrawers
{
    [CustomPropertyDrawer(typeof(TypeRef<>), useForChildren: true)]
    public class GenericTypeRefPropertyDrawer : TextPopupPropertyDrawer
    {
        private List<Type> _typeCache = null;
        private string[] _typeNames = null;

        protected override string[] PopupText => _typeNames;
        protected override int CurrentIndex { get; set; } = NoItemIndex;

        protected override string RefreshButtonText => "Refresh Types";

        protected override void RefreshContent(SerializedProperty property)
        {
            // get the base type
            var baseTypeName = property.FindPropertyRelative(TypeRef<Object>.FieldNames.BaseTypeAssemblyQualifiedName)?.stringValue;
            if (string.IsNullOrWhiteSpace(baseTypeName))
            {
                // should not be possible, since allocating the object sets this value
                // and allocation can only be done with a valid base type
                throw new InvalidOperationException(
                    $"baseTypeName was null or empty, this is an unhandled runtime error");
            }

            var baseType = Type.GetType(baseTypeName);

            _typeCache = FindAllSubTypes(baseType);
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

        private static List<Type> FindAllSubTypes(Type baseType) =>
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t =>
                t == baseType ||
                (t is {IsPublic: true} && t.BaseType == baseType)).ToList();
    }
}