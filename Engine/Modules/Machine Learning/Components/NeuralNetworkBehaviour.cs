using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using CairoEngine.MachineLearning;
using CairoEngine.MachineLearning.Sensers;

namespace CairoEngine
{
    public class NeuralNetworkBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The Neural Network Info of this Agent
        /// </summary>
        [ReadOnly] public NeuralNetworkTemplate template;
        /// <summary>
        /// The Neural Network of this Agent
        /// </summary>
        [ReadOnly] public NeuralNetwork network;
        /// <summary>
        /// The Generation of this Agent
        /// </summary>
        [ReadOnly] public int generation;
        /// <summary>
        /// The Sensers Attacked to the Object
        /// </summary>
        [ReadOnly] public List<Senser> sensers = new List<Senser>();


        /// <summary>
        /// Names and Values of the Inputs
        /// </summary>
        [ReadOnly] public SerializableDictionary<string, double> inputs = new SerializableDictionary<string, double>();
        /// <summary>
        /// Names and Values of the Outputs
        /// </summary>
        [ReadOnly] public SerializableDictionary<string, double> outputs = new SerializableDictionary<string, double>();
    }
}
