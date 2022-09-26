//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

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
    [CreateAssetMenu(menuName = "Cairo Game/Machine Learning/Senser")]
    public class SenserTemplate : ScriptableObject
    {
        public string ID = "Default Senser";
        /// <summary>
        /// Delays the rate at which the Senser updates, in attempt to improve performance
        /// </summary>
        public int updateDelay = 2;
        /// <summary>
        /// Offsets the Update Delay (Use when you want to update each senser one after another)
        /// </summary>
        public int updateOffset = 0;
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
        public Dictionary<string,string> properties = new Dictionary<string, string>();
        /// <summary>
        /// The Inputs used for the Senser
        /// </summary>
        public List<string> inputs = new List<string>();

        /// <summary>
        /// Generates a new Senser List based on this Type of Senser.
        /// </summary>
        /// <returns>The inputs.</returns>
        public virtual List<string> GetInputs(List<string> curInputs)
        {
            List<string> result = new List<string>();
            result.AddRange(curInputs);
            return result;
        }
    }
}
