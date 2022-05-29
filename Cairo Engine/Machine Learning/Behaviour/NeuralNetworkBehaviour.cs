using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using CairoEngine.MachineLearning;

namespace CairoEngine
{
    public class NeuralNetworkBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The Neural Network Info of this Agent
        /// </summary>
        [ReadOnly(true)] public NeuralNetworkInfo info;
        /// <summary>
        /// The Neural Network of this Agent
        /// </summary>
        [ReadOnly(true)] public NeuralNetwork network;
        /// <summary>
        /// The Generation of this Agent
        /// </summary>
        [ReadOnly(true)] public int generation;
        /// <summary>
        /// The Sensers Attacked to the Object
        /// </summary>
        [ReadOnly(true)] public List<Senser> sensers = new List<Senser>();
        /// <summary>
        /// Names and Values of the Inputs
        /// </summary>
        [ReadOnly(true)] public SerializableDictionary<string, double> inputs = new SerializableDictionary<string, double>();
        /// <summary>
        /// Names and Values of the Outputs
        /// </summary>
        [ReadOnly(true)] public SerializableDictionary<string, double> outputs = new SerializableDictionary<string, double>();
    }
}
