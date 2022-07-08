using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace AillieoUtils
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
    internal class SerializableDictionaryEditor : PropertyDrawer
    {
        private SerializedProperty serializedProperty;
        private GUIContent label;
        private ReorderableList reorderableList;

        private static readonly float spaceHeight = EditorGUIUtility.singleLineHeight * 0.5f;
        private static readonly float dropAreaHeight = EditorGUIUtility.singleLineHeight * 2f;
        private static readonly Color lightGray = new Color(0.9f, 0.9f, 0.9f, 1f);
        private static readonly float emptyHeight = 24f;

        private Type cachedValueType;
        private Type valueType
        {
            get
            {
                if (cachedValueType == null)
                {
                    cachedValueType = GetValueType(fieldInfo.FieldType);
                }

                return cachedValueType;
            }
        }

        private static readonly HashSet<SerializedPropertyType> shortTypes = new HashSet<SerializedPropertyType>()
        {
            SerializedPropertyType.Boolean,
            SerializedPropertyType.Color,
            SerializedPropertyType.Enum,
            SerializedPropertyType.Float,
            SerializedPropertyType.Gradient,
            SerializedPropertyType.Integer,
            SerializedPropertyType.String,
            SerializedPropertyType.LayerMask,
            SerializedPropertyType.ObjectReference,
        };

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            this.serializedProperty = property;
            this.label = label;

            if (!serializedProperty.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            if (reorderableList == null)
            {
                CreateReorderableList();
            }

            SerializedProperty keys = property.FindPropertyRelative("keys");
            SerializedProperty values = property.FindPropertyRelative("values");

            int count = keys.arraySize;

            float height = 0;
            height += EditorGUIUtility.singleLineHeight;

            height += GetHeightForAllPairs();

            height += spaceHeight;

            bool drawDropArea = typeof(UnityEngine.Object).IsAssignableFrom(valueType);
            if (drawDropArea)
            {
                height += dropAreaHeight;
            }

            int selected = reorderableList.index;
            if (selected >= 0 && selected < count)
            {
                SerializedProperty key = keys.GetArrayElementAtIndex(selected);
                SerializedProperty value = values.GetArrayElementAtIndex(selected);
                bool drawAsSingleLine = DrawAsSingleLine(key.propertyType, value.propertyType);
                if (!drawAsSingleLine)
                {
                    height += spaceHeight;
                    height += EditorGUI.GetPropertyHeight(value, GUIContent.none, value.isExpanded);
                }
            }

            return height;
        }

        private void CreateReorderableList()
        {
            this.reorderableList = new ReorderableList(this.serializedProperty.serializedObject, this.serializedProperty.FindPropertyRelative("keys"));
            this.reorderableList.drawElementCallback = DrawElementCallback;
            this.reorderableList.elementHeightCallback = ElementHeightCallback;
            this.reorderableList.onAddCallback = OnAddCallback;
            this.reorderableList.onRemoveCallback = OnRemoveCallback;
            this.reorderableList.onReorderCallbackWithDetails = OnReorderCallbackWithDetails;
            this.reorderableList.headerHeight = 1f;
            this.reorderableList.drawHeaderCallback = DrawHeaderCallback;
        }

        private float ElementHeightCallback(int index)
        {
            SerializedProperty keys = serializedProperty.FindPropertyRelative("keys");
            SerializedProperty values = serializedProperty.FindPropertyRelative("values");
            SerializedProperty key = keys.GetArrayElementAtIndex(index);
            SerializedProperty value = values.GetArrayElementAtIndex(index);

            return GetHeightForPair(key, value);
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty keys = serializedProperty.FindPropertyRelative("keys");
            SerializedProperty values = serializedProperty.FindPropertyRelative("values");
            SerializedProperty key = keys.GetArrayElementAtIndex(index);
            SerializedProperty value = values.GetArrayElementAtIndex(index);

            if (DrawAsSingleLine(key.propertyType, value.propertyType))
            {
                Rect half = rect;
                half.width = rect.width * 0.5f;
                EditorGUI.PropertyField(half, key, GUIContent.none);
                half.x += half.width;
                EditorGUI.PropertyField(half, value, GUIContent.none);
            }
            else
            {
                float heightForKey = EditorGUI.GetPropertyHeight(key);

                rect.height = heightForKey;
                EditorGUI.PropertyField(rect, key, GUIContent.none);

                rect.y += heightForKey;
                rect.height = heightForKey;
            }
        }

        private void OnAddCallback(ReorderableList list)
        {
            SerializedProperty keys = serializedProperty.FindPropertyRelative("keys");
            SerializedProperty values = serializedProperty.FindPropertyRelative("values");
            int count = keys.arraySize;
            keys.InsertArrayElementAtIndex(count);
            values.InsertArrayElementAtIndex(count);
        }

        private void OnRemoveCallback(ReorderableList list)
        {
            SerializedProperty keys = serializedProperty.FindPropertyRelative("keys");
            SerializedProperty values = serializedProperty.FindPropertyRelative("values");
            int index = list.index;
            int last = keys.arraySize - 1;

            if (index != last)
            {
                keys.MoveArrayElement(index, last);
                values.MoveArrayElement(index, last);
            }
            else
            {
                list.index = last - 1;
            }

            values.DeleteArrayElementAtIndex(last);
            keys.DeleteArrayElementAtIndex(last);
        }

        private void OnReorderCallbackWithDetails(ReorderableList list, int oldIndex, int newIndex)
        {
            SerializedProperty values = serializedProperty.FindPropertyRelative("values");
            values.MoveArrayElement(oldIndex, newIndex);
        }

        private void DrawHeaderCallback(Rect rect)
        {
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.serializedProperty = property;
            this.label = label;

            GUI.Box(position, GUIContent.none);

            SerializedProperty keys = property.FindPropertyRelative("keys");
            SerializedProperty values = property.FindPropertyRelative("values");

            int count = keys.arraySize;

            Rect rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;

            serializedProperty.isExpanded = GUI.Toggle(rect, serializedProperty.isExpanded, $"{label.text} ({count})", "BoldLabel");
            if (!serializedProperty.isExpanded)
            {
                return;
            }

            if (reorderableList == null)
            {
                CreateReorderableList();
            }

            rect.y += EditorGUIUtility.singleLineHeight;

            reorderableList.DoList(rect);

            rect.y += GetHeightForAllPairs();

            bool drawDropArea = typeof(UnityEngine.Object).IsAssignableFrom(valueType);

            if (property.FindPropertyRelative("invalidFlag").boolValue)
            {
                //rect.height = EditorGUIUtility.singleLineHeight;
                //EditorGUI.HelpBox(rect, "Duplicate keys exist", MessageType.Error);
            }

            if (drawDropArea)
            {
                rect.y += spaceHeight;
                rect.height = dropAreaHeight;
                DrawDropArea(rect, property);
                rect.y += dropAreaHeight;
            }

            int selected = reorderableList.index;
            if (selected >= 0 && selected < count)
            {
                SerializedProperty key = keys.GetArrayElementAtIndex(selected);
                SerializedProperty value = values.GetArrayElementAtIndex(selected);
                bool drawAsSingleLine = DrawAsSingleLine(key.propertyType, value.propertyType);
                if (!drawAsSingleLine)
                {
                    rect.y += spaceHeight;
                    rect.height = EditorGUI.GetPropertyHeight(value, GUIContent.none, value.isExpanded);
                    EditorGUI.PropertyField(rect, value, GUIContent.none, value.isExpanded);
                }
            }
        }

        private float GetHeightForPair(SerializedProperty key, SerializedProperty value)
        {
            if (DrawAsSingleLine(key.propertyType, value.propertyType))
            {
                return EditorGUIUtility.singleLineHeight;
            }
            else
            {
                return EditorGUI.GetPropertyHeight(key);
            }
        }

        private float GetHeightForAllPairs()
        {
            float height = 0f;
            SerializedProperty keys = serializedProperty.FindPropertyRelative("keys");
            SerializedProperty values = serializedProperty.FindPropertyRelative("values");

            height += reorderableList.headerHeight;

            int count = keys.arraySize;

            if (count != 0)
            {
                for (int i = 0; i < count; ++i)
                {
                    SerializedProperty key = keys.GetArrayElementAtIndex(i);
                    SerializedProperty value = values.GetArrayElementAtIndex(i);
                    height += GetHeightForPair(key, value);
                }
            }
            else
            {
                height += emptyHeight;
            }

            height += reorderableList.footerHeight;
            return height;
        }

        protected void DrawDropArea(Rect position, SerializedProperty property)
        {
            Event evt = Event.current;
            Rect dropArea = position;
            Color guiColor = GUI.color;
            GUI.color = Color.yellow;
            GUI.Box(dropArea, "Drop objects hero to add new entries", new GUIStyle("box") { alignment = TextAnchor.MiddleCenter });
            GUI.color = guiColor;

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (dropArea.Contains(evt.mousePosition) &&
                        DragAndDrop.objectReferences.Any(o => valueType.IsInstanceOfType(o)))
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                        if (evt.type == EventType.DragPerform)
                        {
                            DragAndDrop.AcceptDrag();

                            HashSet<UnityEngine.Object> newObjects = new HashSet<UnityEngine.Object>(DragAndDrop.objectReferences);
                            SerializedProperty values = property.FindPropertyRelative("values");
                            for (int i = 0, size = values.arraySize; i < size; ++i)
                            {
                                newObjects.Remove(values.GetArrayElementAtIndex(i).objectReferenceValue);
                            }

                            int newObjectCount = newObjects.Count;
                            if (newObjectCount > 0)
                            {
                                SerializedProperty keys = property.FindPropertyRelative("keys");
                                int oldSize = keys.arraySize;
                                keys.arraySize += newObjectCount;
                                values.arraySize += newObjectCount;
                                foreach (var newObj in newObjects)
                                {
                                    keys.GetArrayElementAtIndex(oldSize).stringValue = newObj.name;
                                    values.GetArrayElementAtIndex(oldSize).objectReferenceValue = newObj;
                                    ++oldSize;
                                }
                            }
                        }
                    }

                    break;
            }
        }

        private static Type GetValueType(Type propertyType)
        {
            while (propertyType != null)
            {
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(SerializableDictionary<,>))
                {
                    Type[] genericArgs = propertyType.GenericTypeArguments;
                    if (genericArgs != null && genericArgs.Length == 2)
                    {
                        Type valueType = genericArgs[1];
                        return valueType;
                    }
                }
                else
                {
                    propertyType = propertyType.BaseType;
                }
            }

            return null;
        }

        private static bool DrawAsSingleLine(SerializedPropertyType keyType, SerializedPropertyType valueType)
        {
            return shortTypes.Contains(keyType) && shortTypes.Contains(valueType);
        }
    }
}
