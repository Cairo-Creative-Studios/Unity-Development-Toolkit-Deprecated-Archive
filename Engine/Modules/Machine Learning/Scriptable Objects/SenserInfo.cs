using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;
using CairoEngine.MachineLearning;
using CairoEngine.MachineLearning.Sensers;
using B83.Unity.Attributes;

namespace CairoEngine.MachineLearning
{
    /// <summary>
    /// A base Senser class to use for 
    /// </summary>
    [CreateAssetMenu(menuName = "CairoGame/Machine Learning/Senser")]
    public class SenserInfo : ScriptableObject
    {
        /// <summary>
        /// The ID of this Senser
        /// </summary>
        public string ID = "TestSenser";
        /// <summary>
        /// The class of the MonoBehaviour to use for this Senser 
        /// </summary>
        [MonoScript(order = 0, type = typeof(Senser))] public string MonoBehaviour = "Senser";
        /// <summary>
        /// The Prefab of this Senser (for Sensers that need to use child Objects, like Colliders)
        /// </summary>
        public GameObject prefab;
        /// <summary>
        /// Properties to pass to the Senser Script
        /// </summary>
        public List<SerializableObject> properties = new List<SerializableObject>();
        /// <summary>
        /// The Inputs used for the Senser
        /// </summary>
        public List<string> inputs = new List<string>();
    }
}
