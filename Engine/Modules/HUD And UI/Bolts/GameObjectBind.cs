using System;
using System.Collections.Generic;
using UnityEngine;
using B83.Unity.Attributes;

namespace CairoEngine.UI
{
    /// <summary>
    /// Binds a field within a Game Object to a Property of a UI Element.
    /// </summary>
    [System.Serializable]
    public class GameObjectBind
    {

        public enum PropertyType
        {
            Text,
            Width,
            Height,
            Color
        }

        /// <summary>
        /// The linked Object
        /// </summary>
        [Tooltip("The Game Object to get the Field value from for binding.")]
        public GameObject boundObject;

        [Tooltip("The Bound Monobehaviour")]
        [MonoScript] public string boundBehaviour;

        /// <summary>
        /// The Name of this Value/Object Link
        /// </summary>
        [Tooltip("The Field to search for")]
        public string fieldName;

        /// <summary>
        /// The UI Element that the Game Object's Field is bound to.
        /// </summary>
        [Tooltip("The UI Element that the Game Object's Field is bound to.")]
        public string element = "";

        /// <summary>
        /// Which of the Element's Properties is bound to this Field?
        /// </summary>
        [Tooltip("Which of the Element's Properties is bound to this Field?")]
        public PropertyType property;
    }
}
