using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [System.Serializable]
    public class UIValueLink
    {
        public enum ValueType
        {
            String,
            Float,
            Int,
            Bool
        }
        /// <summary>
        /// The Name of this Value/Object Link
        /// </summary>
        public string ID;
        /// <summary>
        /// The linked Object
        /// </summary>
        public GameObject linkedObject;
        /// <summary>
        /// The linked Value
        /// </summary>
        public string value;
        /// <summary>
        /// The Type of the Linked Value
        /// </summary>
        public ValueType valueType;
        /// <summary>
        /// A List of Properties that make changes to the default Rendering of the Value Linked Object.
        /// </summary>
        public List<string> properties = new List<string>();
    }
}
