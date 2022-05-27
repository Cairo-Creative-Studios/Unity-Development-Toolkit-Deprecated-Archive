using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;
using System.Linq;

namespace CairoEngine.MachineLearning
{
    /// <summary>
    /// A Backpropogated Neural Network developed with the assistance of: <see cref="https://www.youtube.com/watch?v=-WjKICvAOsY"/> 
    /// </summary>
    [Serializable]
    public class NeuralNetwork
    {
        public string networkID;
        /// <summary>
        /// The Game Object that this Neural Network is Controlling.
        /// </summary>
        public GameObject gameObject;
        /// <summary>
        /// The Template that this Network should follow.
        /// </summary>
        public NeuralNetworkTemplate template;
        /// <summary>
        /// The current Inputs of the Network
        /// </summary>
        public float[] inputs;
        /// <summary>
        /// The current Outputs of the Network
        /// </summary>
        public float[] outputs;
        /// <summary>
        /// The fitness of this Specimen.
        /// </summary>
        public float fitness = 0f;
        /// <summary>
        /// The current lifespan of this Specimen.
        /// </summary>
        public float lifespan = 0;
        /// <summary>
        /// The Structure of the Network
        /// </summary>
        public int[] structure;

        private float[][] values; 
        private float[][] biases; 
        public float[][][] weights;

        private float[][] desiredValues;
        private float[][] biasesSmudge;
        private float[][][] weightsSmudge; 

        private float weightDecay = 0.001f;
        private float learningRate = 1f;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CairoEngine.MachineLearning.BackpropogatedNeuralNetwork"/> class.
        /// </summary>
        /// <param name="structure">Structure.</param>
        public NeuralNetwork(int[] structure, GameObject gameObject, string networkID)
        {
            this.networkID = networkID;

            //Set the Neural Networks Controlling Object
            this.gameObject = gameObject;
            this.structure = structure;

            //Set the length of the arrays based on the Structure Length
            values = new float[structure.Length][];
            desiredValues = new float[structure.Length][];
            biases = new float[structure.Length][];
            biasesSmudge = new float[structure.Length][];
            weights = new float[structure.Length - 1][][];
            weightsSmudge = new float[structure.Length - 1][][];

            //Set the size of the Second dimensions of Values & Biases based on the value in the Structure
            for(int i = 0; i < structure.Length; i++)
            {
                values[i] = new float[structure[i]];
                desiredValues[i] = new float[structure[i]];
                biases[i] = new float[structure[i]];
                biasesSmudge[i] = new float[structure[i]];
            }

            //Set the size of the Weights second Dimension in weights based on the length of Values next 2nd Dimension
            for (int i = 0; i < structure.Length - 1; i++)
            {
                weights[i] = new float[values[i + 1].Length][];
                weightsSmudge[i] = new float[values[i + 1].Length][];
                //Set the size of the Weights third dimension based on the length of the values current second dimension
                for(int j = 0; i < weights[i].Length; j++)
                {
                    weights[i][j] = new float[values[i].Length];
                    weightsSmudge[i][j] = new float[values[i].Length];
                    //Initialize Weights with Random values
                    for (int k = 0; k < weights[i][j].Length; k++) weights[i][j][k] = UnityEngine.Random.Range(0f, 1f) * Mathf.Sqrt(2f/weights[i][j].Length);
                }
            }
        }

        /// <summary>
        /// Creates a clone of a Neural Network.
        /// </summary>
        /// <param name="network">Network.</param>
        public NeuralNetwork(NeuralNetwork network, float[][][] combinedWeights)
        {
            //Set the Neural Networks Controlling Object
            this.gameObject = network.gameObject;

            //Set the length of the arrays based on the Structure Length
            values = new float[network.structure.Length][];
            desiredValues = new float[network.structure.Length][];
            biases = new float[network.structure.Length][];
            biasesSmudge = new float[network.structure.Length][];
            weights = new float[network.structure.Length - 1][][];
            weightsSmudge = new float[network.structure.Length - 1][][];

            //Set the size of the Second dimensions of Values & Biases based on the value in the Structure
            for (int i = 0; i < network.structure.Length; i++)
            {
                values[i] = new float[network.structure[i]];
                desiredValues[i] = new float[network.structure[i]];
                biases[i] = new float[network.structure[i]];
                biasesSmudge[i] = new float[network.structure[i]];
            }

            //Set the size of the Weights second Dimension in weights based on the length of Values next 2nd Dimension
            for (int i = 0; i < network.structure.Length - 1; i++)
            {
                weights[i] = new float[values[i + 1].Length][];
                weightsSmudge[i] = new float[values[i + 1].Length][];
                //Set the size of the Weights third dimension based on the length of the values current second dimension
                for (int j = 0; i < weights[i].Length; j++)
                {
                    weights[i][j] = new float[values[i].Length];
                    weightsSmudge[i][j] = new float[values[i].Length];
                    //Initialize Weights with the passed Genes with some Mutation
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        //Set the Weights of the Child to the chosen Parent's passed Genes, with some Mutation.
                        weights[i][j][k] = combinedWeights[i][j][k] + (UnityEngine.Random.Range(0f, 1f) * Mathf.Sqrt(2f / weights[i][j].Length)) * template.mutationRate;
                    }
                }
            }
        }

        /// <summary>
        /// Test the specified inputs and get the Outputs
        /// </summary>
        public float[] Test()
        {
            //Set the Inputs nodes in Values
            for (int i = 0; i < values[0].Length; i++) values[0][i] = inputs[i];
        
            //Calculate Outputs by Testing Inputs in the Network
            for(int i = 0; i<values.Length; i++)
            for(int j = 0; j<values[i].Length; j++)
            {
                values[i][j] = HardSigmoid(Sum(values[i - 1], weights[i - 1][j]) + biases[i][j]);
                    desiredValues[i][j] = values[i][j];
            }

            return values[values.Length - 1];
        }

        /// <summary>
        /// Set an Input in the Neural Network
        /// </summary>
        /// <param name="input">Input.</param>
        /// <param name="value">Value.</param>
        public void SetInput(int input, float value)
        {
            //Get the current Inputs of the Network as a List
            List<float> setInputs = new List<float>();
            setInputs.AddRange(inputs);
            setInputs[input] = value;
            inputs = setInputs.ToArray();
            outputs = Test();
        }

        /// <summary>
        /// Train the Neural Network
        /// </summary>
        /// <param name="inputs">Inputs.</param>
        /// <param name="outputs">Outputs.</param>
        public void Train()
        {
            //Iterate over every training Input
            for(int i = 0; i < inputs.Length; i++)
            {
                //Get Outputs from the network
                Test();

                //Set correct/desired Outputs of network
                for (int j = 0; j < desiredValues[desiredValues.Length - 1].Length; j++)
                {
                    desiredValues[desiredValues.Length - 1][j] = outputs[i][j];
                }

                //Iterate over every layer backwards (backpropagation)
                for (int j = values.Length; i >= 1; j--)
                {
                    //Iterate over every node
                    for(int k = 0; k <values[i].Length; k++)
                    {
                        //Store by how much we need to smudge the weights and biases
                        var biasSmudge = SigmoidDerivative(values[j][k]) * (desiredValues[j][k] - values[j][k]);
                        biasesSmudge[j][k] += biasSmudge;

                        for(int l = 0; l < values[i-1].Length; l++)
                        {
                            var weightSmudge = values[j - 1][l] * biasSmudge;
                            weightsSmudge[j - 1][k][l] += weightSmudge;
                            var valueSmudge = weights[j - 1][k][l] * biasSmudge;
                            desiredValues[j - 1][l] += valueSmudge;
                        }
                    }
                }
            }

            //Iterate over every layer backwards once again
            for (var i = values.Length - 1; i >= 1; i--)
            {
                for (var j = 0; j < values[i].Length; j++)
                {
                    biases[i][j] += biasesSmudge[i][j] * learningRate;
                    biases[i][j] *= 1 - weightDecay;
                    biasesSmudge[i][j] = 0;

                    //apply weights and biases changes and reset
                    for (var k = 0; k < values[i - 1].Length; k++)
                    {
                        weights[i - 1][j][k] += weightsSmudge[i - 1][j][k] * learningRate;
                        weights[i - 1][j][k] *= 1 - weightDecay;
                        weightsSmudge[i - 1][j][k] = 0;
                    }

                    desiredValues[i][j] = 0;
                }
            }
        }

        /// <summary>
        /// Iterate over every Value and multiply it by the corresponding weight, then sum it up.
        /// </summary>
        /// <returns>The sum.</returns>
        /// <param name="values">Values.</param>
        /// <param name="weights">Weights.</param>
        private static float Sum(IEnumerable<float> values, IReadOnlyList<float> weights) =>
            values.Select((v, i) => v * weights[i]).Sum();

        private static float Sigmoid(float x) => 1f / (1f + (float)Math.Exp(-x));

        /// <summary>
        /// A more efficient approximation of the Sigmoid Function.
        /// </summary>
        /// <returns>The sigmoid.</returns>
        /// <param name="x">The x coordinate.</param>
        private static float HardSigmoid(float x)
        {
            if (x < -2.5f)
                return 0;
            if (x > 2.5f)
                return 1;
            return 0.2f * x + 0.5f;
        }

        private static float SigmoidDerivative(float x) => x * (1 - x);


    }
}
