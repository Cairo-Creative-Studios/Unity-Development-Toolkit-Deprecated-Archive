using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.MachineLearning;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "CairoGame/Machine Learning/Neural Network")]
    public class NeuralNetworkInfo : ScriptableObject
    {
        public string ID = "DefaultNetwork";
        public GameObject prefab;
        /// <summary>
        /// The amount of Neurons in each Layer of the Network
        /// </summary>
        public List<int> neurons = new List<int>(){14,7};
        /// <summary>
        /// The Sensers to use for the Network
        /// </summary>
        public List<SenserInfo> sensers = new List<SenserInfo>();
        /// <summary>
        /// Inputs that are to be controlled through user generated Code.
        /// </summary>
        public List<string> variableInputs = new List<string>();
        /// <summary>
        /// Outputs that are to be controlled through user generated code.
        /// </summary>
        public List<string> variableOutputs = new List<string>();
        public int maxLife = 100;
    }
}
