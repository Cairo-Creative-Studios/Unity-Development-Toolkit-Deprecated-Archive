using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.MachineLearning.Sensers
{
    /// <summary>
    /// The Behaviour of the Object's Senser
    /// </summary>
    public class Senser : MonoBehaviour
    {
        /// <summary>
        /// The Information of the Senser
        /// </summary>
        [ReadOnly] public SenserInfo info;
        /// <summary>
        /// The index/value of the Inputs within the Neural Network
        /// </summary>
        [ReadOnly] public SerializableDictionary<int,double> inputs = new SerializableDictionary<int, double>();
        /// <summary>
        /// The Inputs along with their names
        /// </summary>
        [ReadOnly] public SerializableDictionary<string, int> inputNames = new SerializableDictionary<string, int>();
        /// <summary>
        /// The Game Object this Senser was assigned to
        /// </summary>
        [ReadOnly] public GameObject agentObject;
        /// <summary>
        /// The Network this Senser was assigned to
        /// </summary>
        [ReadOnly] public NeuralNetwork agentNetwork;

        [ReadOnly] public GameObject senserObject;

        void Update()
        {
            Sense();
        }

        /// <summary>
        /// Initialize the Senser
        /// </summary>
        public virtual void Init()
        {

        }

        /// <summary>
        /// Activates the Senser and updates the Inputs
        /// </summary>
        public virtual void Sense()
        {

        }
    }
}
