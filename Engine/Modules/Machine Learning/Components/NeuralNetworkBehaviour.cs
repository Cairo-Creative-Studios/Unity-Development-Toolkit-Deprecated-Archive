//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UDT.MachineLearning;
using UDT.MachineLearning.Sensers;

namespace UDT
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
        [ReadOnly] public SDictionary<string, double> inputs = new SDictionary<string, double>();
        /// <summary>
        /// Names and Values of the Outputs
        /// </summary>
        [ReadOnly] public SDictionary<string, double> outputs = new SDictionary<string, double>();
    }
}
