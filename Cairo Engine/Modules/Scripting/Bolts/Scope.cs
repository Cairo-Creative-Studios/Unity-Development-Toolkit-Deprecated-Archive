using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Scripting
{
    public class Scope
    {
        /// <summary>
        /// Objects grouped with the Properties that are being used in Scripts. Properties are not updated unless Scripts are using them.
        /// </summary>
        public SDictionary<UnityEngine.Object, SDictionary<string, object>> objects = new SDictionary<UnityEngine.Object, SDictionary<string, object>>();

        /// <summary>
        /// Initialize the Scope
        /// </summary>
        public void Init()
        {
            Transform[] children = LevelModule.GetTransform().GetChildren();
        }

        /// <summary>
        /// Updates the Properties in the Scope
        /// </summary>
        public void Update()
        {
            foreach(UnityEngine.Object trackedObject in objects.Keys)
            {
                foreach(string property in objects[trackedObject].Keys)
                {
                    objects[trackedObject][property] = 0;
                }
            }
        }

        /// <summary>
        /// Adds a Tracked Property to the Objects Dictionary
        /// </summary>
        /// <param name="">.</param>
        /// <param name="property">Property.</param>
        public void Track(UnityEngine.Object trackedObject, string property)
        {
            objects[trackedObject].Add(property, null);
        }
    }
}
