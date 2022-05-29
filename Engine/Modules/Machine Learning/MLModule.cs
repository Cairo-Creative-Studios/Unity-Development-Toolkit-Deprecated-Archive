using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.MachineLearning;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System.Linq;

namespace CairoEngine
{
    public class MLModule
    {
        //Specimen information
        private static Dictionary<string, GenePool> genePools = new Dictionary<string, GenePool>();
        private static Dictionary<GameObject, int> networkObjects = new Dictionary<GameObject, int>();

        public static bool enableStats = true;

        /// <summary>
        /// Initialize the Machine Learning Module.
        /// </summary>
        public static void Init()
        {
            List<NeuralNetworkInfo> neuralNetworkInfosList = new List<NeuralNetworkInfo>();
            neuralNetworkInfosList.AddRange(Resources.LoadAll<NeuralNetworkInfo>(""));

            foreach(NeuralNetworkInfo info in neuralNetworkInfosList)
            {
                genePools.Add(info.ID, new GenePool(info));
            }
        }

        /// <summary>
        /// Update the Machine Learning Module
        /// </summary>
        public static void Update()
        {
            //Loop through all the Neural Networks and Update them.
            foreach(GenePool pool in genePools.Values)
            {
                int i = 0;
                foreach(NeuralNetwork network in pool.networks)
                {
                    network.Update();
                    network.lifespan++;

                    if(network.lifespan > pool.info.maxLife)
                    {
                        pool.crossoverCandidates.Enqueue(network);
                    }

                    //If Stats are enabled, Update the Input/Output Dictionaries of the Neural Network Objects.
                    if (network != null && enableStats)
                    {
                        NeuralNetworkBehaviour objectBehaviour = network.gameObject.GetComponent<NeuralNetworkBehaviour>();

                        objectBehaviour.generation = network.generation;

                        objectBehaviour.inputs.Clear();
                        objectBehaviour.outputs.Clear();

                        int j = 0;
                        foreach(string key in pool.info.variableInputs)
                        {
                            objectBehaviour.inputs.Add(key, pool.networks[i].GetInput()[0][j]);
                            j++;
                        }
                        j = 0;
                        foreach(string key in pool.info.variableOutputs)
                        {
                            objectBehaviour.outputs.Add(key, pool.networks[i].GetOutput()[0][j]);
                            j++;
                        }
                    }

                    i++;
                }
                pool.Breed();
            }
        }

        /// <summary>
        /// Creates a Neural Network from the Network Info with the given ID, and returns the created Network. There must be a Network Info Scriptable Object for the given NetworkID, or this method will do nothing.
        /// </summary>
        /// <param name="NetworkID">The name of the Network Info Object to use as the Starting Point of the Network Group</param>
        public static NeuralNetwork CreateNetwork(string NetworkID)
        {
            //Create the Neural Network
            NeuralNetworkInfo info = genePools[NetworkID].info;
            GenePool genePool = genePools[NetworkID];
            NeuralNetwork network = new NeuralNetwork(info);

            //Add the new Network to the Gene Pool
            genePool.networks.Add(network);

            //If the Network has a Prefab, Instantiate it and set up it's propeties
            if (info.prefab != null)
            {
                GameObject networkObject = Object.Instantiate(info.prefab);
                network.gameObject = networkObject;
                NeuralNetworkBehaviour behaviour = networkObject.AddComponent<NeuralNetworkBehaviour>();
                behaviour.network = network;
                behaviour.info = info;
            }

            //Return the Created Network
            return genePools[NetworkID].networks[genePools[NetworkID].networks.Count-1];
        }

        /// <summary>
        /// Creates a child agent from two Parents
        /// </summary>
        /// <returns>The child.</returns>
        /// <param name="firstParent">First parent.</param>
        /// <param name="secondParent">Second parent.</param>
        public static NeuralNetwork CreateNetwork(NeuralNetwork firstParent, NeuralNetwork secondParent)
        {
            //Create the Neural Network
            NeuralNetworkInfo info = firstParent.info;
            GenePool genePool = genePools[firstParent.info.ID];
            NeuralNetwork network = new NeuralNetwork(firstParent, secondParent);

            //Add the new Network to the Gene Pool
            genePool.networks.Add(network);

            //If the Network has a Prefab, Instantiate it and set up it's propeties
            if (info.prefab != null)
            {
                GameObject networkObject = Object.Instantiate(info.prefab);
                network.gameObject = networkObject;
                NeuralNetworkBehaviour behaviour = networkObject.AddComponent<NeuralNetworkBehaviour>();
                behaviour.network = network;
                behaviour.info = info;
            }

            GameObject.Destroy(firstParent.gameObject);
            genePool.networks.Remove(firstParent);

            return network;
        }

        /// <summary>
        /// Adds the Network's Sensers to the Game Object, and returns the compiled Array of Inputs
        /// </summary>
        /// <returns>The sensers.</returns>
        /// <param name="network">Network.</param>
        public static double[] AddSensers(NeuralNetwork network, double[] currentInputs)
        {
            List<double> inputList = new List<double>();
            inputList.AddRange(currentInputs);

            foreach(SenserInfo senserInfo in network.info.sensers)
            {

            }

            return inputList.ToArray();
        }

        /// <summary>
        /// A Container of Neural Networks
        /// </summary>
        private class GenePool
        {
            public NeuralNetworkInfo info;
            public List<NeuralNetwork> networks = new List<NeuralNetwork>();
            public Queue<NeuralNetwork> crossoverCandidates = new Queue<NeuralNetwork>();

            public GenePool(NeuralNetworkInfo info)
            {
                this.info = info;
            }

            /// <summary>
            /// Call the Crossover Method for all Crossover Candidates
            /// </summary>
            public void Breed()
            {
                while (crossoverCandidates.Count > 0)
                {
                    Crossover(crossoverCandidates.Dequeue());
                }
            }

            /// <summary>
            /// Crossover genes from two Parents
            /// </summary>
            /// <param name="firstParent">First parent.</param>
            public void Crossover(NeuralNetwork firstParent)
            {
                NeuralNetwork secondParent = FindMate(firstParent);
                MLModule.CreateNetwork(firstParent, secondParent);
            }

            /// <summary>
            /// Finds a good mate for the Parent
            /// </summary>
            /// <returns>The mate.</returns>
            /// <param name="firstParent">First parent.</param>
            private NeuralNetwork FindMate(NeuralNetwork firstParent)
            {
                double fitness = firstParent.GetFitness();

                if (networks.Count > 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (NeuralNetwork network in networks)
                        {
                            if (network.GetFitness() > fitness && UnityEngine.Random.Range(0, 1) > 0.57 && network != firstParent)
                            {
                                return network;
                            }
                            else if (UnityEngine.Random.Range(0, 1) < 0.117 && network != firstParent)
                            {
                                return network;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Cannot create child agent of type " + firstParent.info.ID + ", because there are no other Agents to Crossover with candidates");
                }
                return firstParent;
            }

        }
    }
}
