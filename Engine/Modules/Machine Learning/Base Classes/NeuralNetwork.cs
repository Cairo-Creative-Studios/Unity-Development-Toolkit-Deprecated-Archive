using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.MachineLearning;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System.Linq;
using NMatrix;

namespace CairoEngine.MachineLearning
{
    public class NeuralNetwork
    {
        public NeuralNetworkInfo info;
        public GameObject gameObject;
        public int lifespan = 0;

        ActivationNetwork _neuralNetwork;
        ParallelResilientBackpropagationLearning _neuralTeacher;

        Matrix inputs;
        Matrix outputs;
        Matrix trainingInputs;
        Matrix trainingOutputs;
        double fitness = 0f;
        bool trained = false;

        //double initialStep = 0.0125;
        double sigmoidAlphaValue = 2.0;

        //Stats
        public double error;
        public int gatheredDataSets;
        public int generation;

        /// <summary>
        /// Initializes an original Instance of the Neural Network
        /// </summary>
        public NeuralNetwork(NeuralNetworkInfo info)
        {
            //Apply the Info for this Network
            this.info = info;

            if (info.tempInputs.Count == 0)
            {
                Debug.LogWarning("The input List for "+info.ID+" is empty. A Neural Network can't be Created with an empty Input List.");
            }

            if (info.tempInputs.Count == 0)
            {
                Debug.LogWarning("The output List for " + info.ID + " is empty. A Neural Network can't be Created with an empty output List.");
            }

            //Set the Size of the Input and Output Matrices to the Size of the Info's I/O Lists
            inputs = new Matrix((int)Mathf.Clamp(info.tempInputs.Count, 1, Mathf.Infinity), 1);
            outputs = new Matrix((int)Mathf.Clamp(info.tempInputs.Count, 1, Mathf.Infinity), 1);
            trainingInputs = inputs;
            trainingOutputs = outputs;

            //Initialize the Network and it's Teacher
            _neuralNetwork = new ActivationNetwork(new SigmoidFunction(sigmoidAlphaValue), inputs.Rows, info.neurons.ToArray());
            _neuralTeacher = new ParallelResilientBackpropagationLearning(_neuralNetwork);

        }

        /// <summary>
        /// Initializes a child Instance of the Neural Network
        /// </summary>
        /// <param name="firstParent">The Layout of the Network inherited from the Network's First Parent.</param>
        /// <param name="secondParent">The Layout of the Network inherited from the Network's Second Parent.</param>
        public NeuralNetwork(NeuralNetwork firstParent, NeuralNetwork secondParent)
        {
            lifespan = 0;
            //Apply the Info for this Network
            this.info = firstParent.info;
            //Keep track of the Network's Generation
            this.generation = firstParent.generation+1;
            Debug.Log("New Child " + generation);

            //Set the Size of the Input and Output Matrices to the Size of the Info's I/O Lists
            inputs = new Matrix((int)Mathf.Clamp(info.tempInputs.Count, 1, Mathf.Infinity), 1);
            outputs = new Matrix((int)Mathf.Clamp(info.tempInputs.Count, 1, Mathf.Infinity), 1);
            trainingInputs = inputs;
            trainingOutputs = outputs;

            //Initialize the Network and it's Teacher
            _neuralNetwork = new ActivationNetwork(new SigmoidFunction(sigmoidAlphaValue), inputs.Rows, info.neurons.ToArray());
            _neuralTeacher = new ParallelResilientBackpropagationLearning(_neuralNetwork);

            //Pass on Neurons randomly from the two Parents
            for (int i = 0; i < _neuralNetwork.Layers.Length; i++)
            {
                for(int j = 0; j < _neuralNetwork.Layers[i].Neurons.Length; j++)
                {
                    if (UnityEngine.Random.Range(0, 1) > 0.5)
                        _neuralNetwork.Layers[i].Neurons[j] = firstParent._neuralNetwork.Layers[i].Neurons[j];
                    else
                        _neuralNetwork.Layers[i].Neurons[j] = secondParent._neuralNetwork.Layers[i].Neurons[j];
                }
            }
        }

        /// <summary>
        /// Train the Network with the specified Inputs and Outputs.
        /// </summary>
        /// <param name="trainingInputs">Training inputs.</param>
        /// <param name="trainingOutputs">Training outputs.</param>
        public void Train(double[] trainingInputs, double[] trainingOutputs)
        {
            this.trainingInputs.Add(trainingInputs);
            this.trainingOutputs.Add(trainingOutputs);

            gatheredDataSets++;
        }

        /// <summary>
        /// Compute the specified Inputs in the Network and get the Outputs
        /// </summary>
        /// <param name="givenInputs">Given inputs.</param>
        public double[] Compute(double[] givenInputs)
        {
            return _neuralNetwork.Compute(givenInputs);
            //Set Error Stat
            //error = _neuralTeacher.RunEpoch(inputs.ToArray(), outputs.ToArray());
        }

        /// <summary>
        /// Update the Neural Network
        /// </summary>
        public void Update()
        {
            //Compute the current Inputs of the Network
            Compute(inputs.GetColumnArray(0));

            if(trained)
                _neuralTeacher.RunEpoch(trainingInputs.ToArray(), trainingOutputs.ToArray());
        }

        public void AddFitness(double amount)
        {
            fitness += amount;
        }

        public double GetFitness()
        {
            return fitness;
        }

        public double[][] GetInput()
        {
            return inputs.ToArray();
        }

        public double[][] GetOutput()
        {
            return outputs.ToArray();
        }

        /// <summary>
        /// Sets the Inputs of the Neural Network
        /// </summary>
        /// <param name="inputs">Inputs.</param>
        public void SetInputs(double[][] inputs)
        {
            //Loop through the passed Multidimensional Double Array and set the Input Matrix Values
            for (int i = 0; i < inputs.Length; i++)
            {
                this.inputs[i, 0] = inputs[i][0];
            }
        }
    }
}
