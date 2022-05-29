using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.MachineLearning.Sensers
{
    /// <summary>
    /// The Behaviour of the Object's Senser
    /// </summary>
    public class Senser
    {
        /// <summary>
        /// The Information of the Senser
        /// </summary>
        [ReadOnly] public SenserInfo info;
        /// <summary>
        /// The index/value of the Inputs within the Neural Network
        /// </summary>
        [ReadOnly] public SerializableDictionary<int,double[]> inputs = new SerializableDictionary<int, double[]>();
        /// <summary>
        /// The Game Object this Senser was assigned to
        /// </summary>
        [ReadOnly] public GameObject agentObject;
        /// <summary>
        /// The Network this Senser was assigned to
        /// </summary>
        [ReadOnly] public NeuralNetwork agentNetwork;

        [ReadOnly] public GameObject senserObject;
    }
}
