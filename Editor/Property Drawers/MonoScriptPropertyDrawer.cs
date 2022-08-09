/*****
 * This is a simple PropertyDrawer for string variables to allow drag and drop
 * of MonoScripts in the inspector of the Unity3d editor.
 * 
 * NOTE: This is an editor script and need to be placed in a folder named "editor".
 *       It also requires another runtime file named "MonoScriptAttribute.cs"
 * 
 * Copyright (c) 2016 Bunny83
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to
 * deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 * 
 *****/
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using B83.Unity.Attributes;
using System;

namespace B83.Unity.Editor.PropertyDrawers
{

    [CustomPropertyDrawer(typeof(MonoScriptAttribute), false)]
    public class MonoScriptPropertyDrawer : PropertyDrawer
    {
        static Dictionary<string, MonoScript> m_ScriptCache;
        static MonoScriptPropertyDrawer()
        {
            m_ScriptCache = new Dictionary<string, MonoScript>();
            var scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            for (int i = 0; i < scripts.Length; i++)
            {
                var type = scripts[i].GetClass();
                if (type != null && !m_ScriptCache.ContainsKey(type.FullName))
                {
                    m_ScriptCache.Add(type.FullName, scripts[i]);
                }
            }
        }
        bool m_ViewString = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                Rect r = EditorGUI.PrefixLabel(position, label);
                Rect labelRect = position;
                labelRect.xMax = r.xMin;
                position = r;
                m_ViewString = GUI.Toggle(labelRect, m_ViewString, "", "label");
                if (m_ViewString)
                {
                    property.stringValue = EditorGUI.TextField(position, property.stringValue);
                    return;
                }
                MonoScript script = null;
                string typeName = property.stringValue;
                if (!string.IsNullOrEmpty(typeName))
                {
                    m_ScriptCache.TryGetValue(typeName, out script);
                    if (script == null)
                        GUI.color = Color.red;
                }

                script = (MonoScript)EditorGUI.ObjectField(position, script, typeof(MonoScript), false);
                if (GUI.changed)
                {
                    if (script != null)
                    {
                        var type = script.GetClass();
                        MonoScriptAttribute attr = (MonoScriptAttribute) attribute;
                        if (attr.type != null && !attr.type.IsAssignableFrom(type))
                            type = null;
                        if(type!=null)
                            property.stringValue = script.GetClass().FullName;
                    }
                    else
                    {
                        property.stringValue = "";
                        Debug.Log("Script was Null");
                    }
                        
                }
            }
            else
            {
                GUI.Label(position, "The MonoScript attribute can only be used on string variables");
            }
        }
    }
}