using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// The Object Manager lets you create and interact with Objects easily in the Game.
    /// </summary>
    public class ObjectManager
    {

        /// <summary>
        /// The objects that the Engine is controlling.
        /// </summary>
        private static List<Object> objects = new List<Object>();

        public static void Init()
        {

        }

        public static void Update()
        {

        }

        #region Objects
        ///<summary>
        /// Adds an Object to the Object list
        ///</summary>
        public static void AddObject(Object addedObject)
        {
            objects.Add(addedObject);
        }

        /// <summary>
        /// Creates a new Object and Returns it.
        /// </summary>
        /// <returns>The object.</returns>
        public static Object CreateObject(string name)
        {
            GameObject spekObject = new GameObject();
            Object spekBehaviour = spekObject.AddComponent<Object>();
            objects.Add(spekBehaviour);
            spekBehaviour.OID = objects.Count - 1;
            return spekBehaviour;
        }
        #endregion

    }
}
