using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.MachineLearning;

namespace CairoEngine
{
    /// <summary>
    /// Creates a new Gene Pool to work on with Machine Learning
    /// </summary>
    [CreateAssetMenu(menuName = "Cairo Game/Machine Learning/Neural Network Template")]
    public class NeuralNetworkTemplate : ScriptableObject
    {
        public string ID;

        [Header("Main Info")]
        /// <summary>
        /// The Game Object Prefab to generate for the Neural Network Objects in the Genetic Pool.
        /// </summary>
        public GameObject objectPrefab;
        /// <summary>
        /// The structure of the Network.
        /// </summary>
        public List<int> structure = new List<int>();
        /// <summary>
        /// The amount of Time that one child in the Gene Pool should live. If negative, one instance will live forever.
        /// </summary>
        public float maxLifespan = -1f;

        [Header("Properties")]
        /// <summary>
        /// The weight of Decay.
        /// </summary>
        public float weightDecay = 0.001f;
        /// <summary>
        /// The Learning Rate.
        /// </summary>
        public float learningRate = 1f;
        /// <summary>
        /// The Rate at which the Learning Rate should Decrease over time, to find more accurate results.
        /// </summary>
        public float dynamicLearningRateDecrease = 0.01f;
        /// <summary>
        /// The Minimum Learning Rate.
        /// </summary>
        public float learningRateMinimum = 0.25f;

        public float mutationRate = 0.1f;
    }
}
