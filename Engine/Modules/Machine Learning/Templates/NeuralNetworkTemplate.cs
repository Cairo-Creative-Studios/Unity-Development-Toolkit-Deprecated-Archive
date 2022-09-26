//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.MachineLearning;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Machine Learning/Neural Network")]
    public class NeuralNetworkTemplate : ScriptableObject
    {
        public string ID = "Default Network";
        /// <summary>
        /// Allows you to see statistics in the Inspector on values related to the running Neural Network. 
        /// This slows the game down a lot, so it is suggested that you keep it disabled until you're ready to use it.
        /// </summary>
        public bool enableStats = false;
        public GameObject prefab;
        /// <summary>
        /// The amount of Neurons in each Layer of the Network
        /// </summary>
        public List<int> neurons = new List<int>(){14,7};
        /// <summary>
        /// The Sensers to use for the Network
        /// </summary>
        public List<SenserTemplate> sensers = new List<SenserTemplate>();
        /// <summary>
        /// Inputs that are to be controlled through user generated Code.
        /// </summary>
        public List<string> variableInputs = new List<string>();
        /// <summary>
        /// A Temporary List of Inputs created at Runtime, generated from the Network and it's Sensers.
        /// </summary>
        public List<string> tempInputs = new List<string>();
        /// <summary>
        /// Outputs that are to be controlled through user generated code.
        /// </summary>
        public List<string> variableOutputs = new List<string>();
        public int maxLife = 100;
    }
}
