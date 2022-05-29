using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

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
        public string MonoBehaviour = "Senser";
    }
}
