using System;
using System.Collections;
using UnityEngine;
using UDT.Reflection;
using System.Collections.Generic;

namespace UDT.AssetScripting
{
    [CreateAssetMenu(menuName = "Asset Scripting/Method")]
    public class AssetMethod : ScriptableObject
    {
        /// <summary>
        /// All Objects that have been subscribed to this Method
        /// </summary>
        public List<object> subscribers = new List<object>();
        /// <summary>
        /// The Name of the Method to Call the Method on.
        /// </summary>
        [Tooltip("The Name of the Method to Call the Method on.")]
        public string method = "";
        /// <summary>
        /// The Child nested in the Instance to call the Method on.
        /// </summary>
        [Tooltip("The Child nested in the Instance to call the Method on.")]
        public string child = "";
        /// <summary>
        /// The Last Value 
        /// </summary>
        [Tooltip("The Last Value returned from this Asset Method")]
        public object lastValue = null;

        /// <summary>
        /// Adds an object as a Subscriber to this Method. When the Call Method is Called, it will be called on all of the Method's Subscribers
        /// </summary>
        /// <param name="instance">Instance.</param>
        public void Subscribe(object instance)
        {
            subscribers.Add(instance);
        }

        /// <summary>
        /// Call the Method on the Subscibing Instances
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public object Call(object[] parameters = null)
        {
            foreach (object instance in subscribers)
            {
                return instance.CallMethod(method, parameters);
            }
            return null;
        }
    }
}
