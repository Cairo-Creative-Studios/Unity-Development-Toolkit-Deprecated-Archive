using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.MachineLearning;
using CairoEngine.MachineLearning.Sensers;
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

        //Default Properties

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
                        Debug.Log(network.gameObject);
                        NeuralNetworkBehaviour objectBehaviour = network.gameObject.GetComponent<NeuralNetworkBehaviour>();

                        objectBehaviour.generation = network.generation;

                        objectBehaviour.inputs.Clear();
                        objectBehaviour.outputs.Clear();

                        int j = 0;
                        foreach(string key in pool.info.variableInputs)
                        {
                            objectBehaviour.inputs.Add(key, pool.networks[i].GetInput()[0][j]);
                            if(j < pool.info.variableInputs.Count-2)
                                j++;
                        }
                        j = 0;
                        foreach(string key in pool.info.variableOutputs)
                        {
                            objectBehaviour.outputs.Add(key, pool.networks[i].GetOutput()[0][j]);
                            if (j < pool.info.variableInputs.Count-2)
                                j++;
                        }
                    }

                    if (i < pool.networks.Count)
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
            //Get the Network Info
            NeuralNetworkInfo info = genePools[NetworkID].info;

            //Optional Object Parameters
            GameObject networkObject = null;
            NeuralNetworkBehaviour behaviour = null;

            //If the Network has a Prefab, Instantiate it
            if (info.prefab != null)
            {
                networkObject = InstantiatePrefab(info);
                behaviour = networkObject.GetComponent<NeuralNetworkBehaviour>();
                info = behaviour.info;
            }

            //Create the Neural Network
            GenePool genePool = genePools[NetworkID];
            NeuralNetwork network = new NeuralNetwork(info);

            //Add the new Network to the Gene Pool
            genePool.networks.Add(network);

            //If the Network has a Prefab, pair the Network and Behaviour
            if (networkObject != null)
                SetupNetworkObject(info, network, networkObject, behaviour);

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

            //Optional Object Parameters
            GameObject networkObject = null;
            NeuralNetworkBehaviour behaviour = null;

            //If the Network has a Prefab, Instantiate it
            if (info.prefab != null)
            {
                networkObject = InstantiatePrefab(info);
                behaviour = networkObject.GetComponent<NeuralNetworkBehaviour>();
                info = behaviour.info;
            }

            //Create the Neural Network
            GenePool genePool = genePools[info.ID];
            NeuralNetwork network = new NeuralNetwork(firstParent, secondParent);

            //Add the new Network to the Gene Pool
            genePool.networks.Add(network);

            //If the Network has a Prefab, pair the Network and Behaviour
            if (networkObject != null)
                SetupNetworkObject(info, network, networkObject, behaviour);

            //Remove the First Parent so not to Overpopulate the Gene Pool
            Object.Destroy(firstParent.gameObject);
            genePool.networks.Remove(firstParent);

            //Return the created Network
            return network;
        }

        /// <summary>
        /// Instantiates the Prefab for a Neural Network Template
        /// </summary>
        /// <returns>The prefab.</returns>
        /// <param name="info">Info.</param>
        private static GameObject InstantiatePrefab(NeuralNetworkInfo info)
        {
            //Network Object Properties
            GameObject networkObject = Object.Instantiate(info.prefab);
            NeuralNetworkBehaviour behaviour = networkObject.AddComponent<NeuralNetworkBehaviour>();

            //If the Object has sensers
            if (info.sensers.Count > 0)
            {
                //Loop through all the Senser Templates to add them to the Object
                foreach (SenserInfo senserInfo in info.sensers)
                {
                    //Add the Senser Behaviour
                    Senser senserBehaviour = (Senser)networkObject.AddComponent(Type.GetType(senserInfo.MonoBehaviour));
                    GameObject behaviourObject = Object.Instantiate(senserInfo.prefab);

                    //Set up Senser props
                    senserBehaviour.info = senserInfo;
                    senserBehaviour.senserObject = behaviourObject;
                    senserBehaviour.agentObject = networkObject.gameObject;

                    //Add the Senser Object to the Network object
                    senserBehaviour.senserObject.transform.parent = networkObject.transform;

                    //Initialize the Senser Behaviour
                    senserBehaviour.Init();

                    //Recieve Inputs from the Senser Info
                    Dictionary<int,List<string>> recievedInputs = senserInfo.GetInputs(info.variableInputs.Count-1);

                    //Loop through all the Inputs Recieved from the Senser Template, and add them to the Inputs Lists
                    for (int i = 0; i < recievedInputs.Count; i++)
                    {
                        senserBehaviour.inputs.Add(i, 0.0);
                        senserBehaviour.inputNames.Add(recievedInputs[info.variableInputs.Count - 1][i], i);
                        info.variableInputs.AddRange(recievedInputs[info.variableInputs.Count - 1]);
                    }
                }
            }

            behaviour.info = info;

            //Return the created Network Object
            return networkObject;
        }

        /// <summary>
        /// Set up the newly created Neural Network Object
        /// </summary>
        /// <param name="info">Info.</param>
        /// <param name="network">Network.</param>
        /// <param name="networkObject">Network object.</param>
        /// <param name="behaviour">Behaviour.</param>
        private static void SetupNetworkObject(NeuralNetworkInfo info, NeuralNetwork network, GameObject networkObject, NeuralNetworkBehaviour behaviour)
        {
            //Set the Neural Network's Game Object to the newly created one
            network.gameObject = networkObject;

            //Get all the Sensers, loop through them and set their Network to new Neural Network
            Senser[] sensers = networkObject.GetComponents<Senser>();
            foreach (Senser senser in sensers)
            {
                senser.agentNetwork = network;
            }

            //Set the Neural Network Behaviours Network and Info Template 
            behaviour.network = network;
            behaviour.info = info;
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
