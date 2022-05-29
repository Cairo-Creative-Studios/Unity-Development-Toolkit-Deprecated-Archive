using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.MachineLearning
{
    /// <summary>
    /// The Behaviour of the Object's Senser
    /// </summary>
    public class Senser
    {
        /// <summary>
        /// The Information of the Senser
        /// </summary>
        public SenserInfo info;
        /// <summary>
        /// The index/value of the Inputs within the Neural Network
        /// </summary>
        public List<double[]> inputs = new List<double[]>();

    }
}
