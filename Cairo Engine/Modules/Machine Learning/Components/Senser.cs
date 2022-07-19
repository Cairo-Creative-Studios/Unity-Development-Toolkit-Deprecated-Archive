//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

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
        [ReadOnly] public SenserTemplate template;
        /// <summary>
        /// The index/value of the Inputs within the Neural Network
        /// </summary>
        [ReadOnly] public SDictionary<int,double> inputs = new SDictionary<int, double>();
        /// <summary>
        /// The Inputs along with their names
        /// </summary>
        [ReadOnly] public SDictionary<string, int> inputNames = new SDictionary<string, int>();
        /// <summary>
        /// The Game Object this Senser was assigned to
        /// </summary>
        [ReadOnly] public GameObject agentObject;
        /// <summary>
        /// The Network this Senser was assigned to
        /// </summary>
        [ReadOnly] public NeuralNetwork agentNetwork;

        [ReadOnly] public GameObject senserObject;

        private int life = 0;

        void Update()
        {
            life++;
            if((life%template.updateDelay) == 0)
            {
                Sense();
                SetInputs();
            }
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

        /// <summary>
        /// Sets the Inputs connected to this Senser for the Neural Network
        /// </summary>
        private void SetInputs()
        {
            //Get the Current Inputs from the Network
            double[][] curInputs = agentNetwork.GetInput();

            //Loop through the Current Inputs and Update the Inputs belonging to this Senser
            for(int i = 0; i < curInputs.Length; i++)
            {
                //If the Input belongs to this senser, set it to the Senser's Input Value
                if (inputs.ContainsKey(i))
                {
                    curInputs[i][0] = inputs[i];
                }
            }

            //Update the Network's Inputs
            agentNetwork.SetInputs(curInputs);
        }
    }
}
