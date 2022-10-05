using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace FasterGames.EditorTools.Editor.Tests
{
    public class SerializedObjectUtilTests
    {
        public class SimpleObjectFixture : ScriptableObject
        {
            public string value;
        }
        
        [Test]
        public void SimpleObject()
        {
            var so = new SerializedObject(ScriptableObject.CreateInstance<SimpleObjectFixture>());
            var prop = so.FindProperty(nameof(SimpleObjectFixture.value));
            var type = SerializedObjectUtils.FindPropertyType(so, prop.propertyPath);
            
            Assert.AreEqual(typeof(string), type);
        }

        [Serializable]
        public class ArrayObjectEntry
        {
            public int value;
        }

        public class ArrayObjectFixture : ScriptableObject
        {
            // note: SO doesn't serialize if there's no data, so using it as a reflection source
            // does require that the array is non-null
            public ArrayObjectEntry[] children = new[] {new ArrayObjectEntry() {value = 10}};
        }
        
        [Test]
        public void ArrayObject()
        {
            var so = new SerializedObject(ScriptableObject.CreateInstance<ArrayObjectFixture>());
            var prop = so.FindProperty(nameof(ArrayObjectFixture.children));
            prop = prop.GetArrayElementAtIndex(0);
            prop = prop.FindPropertyRelative(nameof(ArrayObjectEntry.value));
                
            var type = SerializedObjectUtils.FindPropertyType(so, prop.propertyPath);
            
            Assert.AreEqual(typeof(int), type);
        }

        [Serializable]
        public class ListObjectEntry
        {
            public ListObjectEntry(float v)
            {
                test = v;
            }
            
            [SerializeField]
            protected float test;
        }

        public class ListObjectFixture : ScriptableObject
        {
            public List<ListObjectEntry> items = new List<ListObjectEntry>()
            {
                new ListObjectEntry(10f)
            };
        }
        
        [Test]
        public void ListObject()
        {
            var so = new SerializedObject(ScriptableObject.CreateInstance<ListObjectFixture>());
            var prop = so.FindProperty(nameof(ListObjectFixture.items));
            prop = prop.GetArrayElementAtIndex(0);
            prop = prop.FindPropertyRelative("test");

            var type = SerializedObjectUtils.FindPropertyType(prop);
            
            Assert.AreEqual(typeof(float), type);
        }
    }
}