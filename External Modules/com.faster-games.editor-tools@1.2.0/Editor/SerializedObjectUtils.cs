using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using Object = UnityEngine.Object;

namespace FasterGames.EditorTools.Editor
{
    /// <summary>
    /// Utilities to walk serialized object types.
    /// </summary>
    public static class SerializedObjectUtils
    {
        /// <summary>
        /// Regex for unity SO array syntax
        /// </summary>
        private static Regex arrayRe = new Regex("\\[([0-9]+)\\]");

        /// <summary>
        /// Utility class to parse Unity SO propertyPath values
        /// </summary>
        private class PathParser
        {
            private List<string> parts;
            private int index = 0;
            
            public PathParser(string input)
            {
                parts = input.Split('.').ToList();
            }

            public string Next()
            {
                if (index >= parts.Count)
                {
                    return null;
                }
                
                var chunk = parts[index];

                if (index + 2 >= parts.Count)
                {
                    index++;
                    return chunk;
                }
                
                var next = parts[index + 1];
                var lookahead = parts[index + 2];
                
                if (next == "Array")
                {
                    var match = arrayRe.Match(lookahead);
                    index+= 3;
                    return $"{chunk}{match.Groups[0].Value}";
                }

                return null;
            }
        }
        
        /// <summary>
        /// Finds the type of a serialized property using reflection.
        /// </summary>
        /// <remarks>
        /// Slow, potentially error prone operation.
        /// </remarks>
        /// <param name="prop">property to find</param>
        /// <returns>type or null</returns>
        public static Type FindPropertyType(SerializedProperty prop)
        {
            return FindPropertyType(prop.serializedObject, prop.propertyPath);
        }

        /// <summary>
        /// Finds the type of a serialized property using reflection.
        /// </summary>
        /// <remarks>
        /// Slow, potentially error prone operation.
        /// </remarks>
        /// <param name="obj">object to search</param>
        /// <param name="propertyPath">path to find</param>
        /// <returns>type or null</returns>
        public static Type FindPropertyType(SerializedObject obj, string propertyPath)
        {
            return FindPropertyType(obj.targetObject, propertyPath);
        }

        /// <summary>
        /// Finds the type of a serialized property using reflection.
        /// </summary>
        /// <remarks>
        /// Slow, potentially error prone operation.
        /// </remarks>
        /// <param name="targetObject">object to search</param>
        /// <param name="propertyPath">path to find</param>
        /// <returns>type or null</returns>
        public static Type FindPropertyType(Object targetObject, string propertyPath)
        {
            var parser = new PathParser(propertyPath);
            var currentType = targetObject.GetType();

            var next = parser.Next();
            while (next != null)
            {
                currentType = FindFieldType(currentType, next);
                next = parser.Next();
            }

            return currentType;
        }

        private static Type FindFieldType(Type targetType, string propertyPath)
        {
            // if there's no prop, it's you bb
            if (string.IsNullOrWhiteSpace(propertyPath))
            {
                return targetType;
            }

            var arrayMatch = arrayRe.Match(propertyPath);

            // if it's NOT a match, it's not an array
            if (!arrayMatch.Success)
            {
                // so we just find the field type
                return targetType
                    ?.GetField(propertyPath, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    ?.FieldType;
            }
            
            // otherwise it's an array or list
            var arrayName = propertyPath.Substring(0, propertyPath.IndexOf("[", StringComparison.Ordinal));
            var arrayType = targetType
                .GetField(arrayName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                ?.FieldType;

            // array
            if (arrayType is {IsArray: true})
            {
                return arrayType?.GetElementType();
            }
            // generic list/enumerable/etc (could be unsupported type too, in which case should be null)
            else
            {
                var generics = arrayType?.GetGenericArguments();
                return generics?[0];
            }
        }
    }
}