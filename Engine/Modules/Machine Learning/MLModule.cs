/*! \addtogroup machinelearningmodule Machine Learning Module
 *  Additional documentation for group 'Machine Learning Module'
 *  @{
 */

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using CairoEngine.MachineLearning;
using CairoEngine.MachineLearning.Sensers;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// The Machine Learning Module simplifies the creation and manipulation of Machine Learning Agents, allowing them to be used with very little input from the user by modularizing Training Data and mapping it to the rest of the Toolkit. Despite it's surface level simiplicity, it comes with a ton of features, and can be used for any application.
    /// </summary>
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
            List<NeuralNetworkTemplate> neuralNetworkInfosList = new List<NeuralNetworkTemplate>();
            neuralNetworkInfosList.AddRange(Resources.LoadAll<NeuralNetworkTemplate>(""));

            foreach(NeuralNetworkTemplate info in neuralNetworkInfosList)
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
                    //Update the Network and increase it's Lifespan
                    network.Update();
                    network.lifespan++;

                    //If Stats are enabled, Update the Input/Output Dictionaries of the Neural Network Objects.
                    if (network != null && network.info.enableStats)
                    {
                        //Get the Neural Network Behaviour from the Game Object
                        NeuralNetworkBehaviour objectBehaviour = network.gameObject.GetComponent<NeuralNetworkBehaviour>();

                        //Set the Generation Count
                        objectBehaviour.generation = network.generation;

                        //Clear the Inputs and Outputs of the Network Behaviour
                        objectBehaviour.inputs.Clear();
                        objectBehaviour.outputs.Clear();

                        //Get the Current Inputs and Outputs of the Network
                        double[][] inputs = network.GetInput();
                        double[][] outputs = network.GetOutput();

                        //Loop through the Info's Generated List of Inputs and Add them to the Neural Network Behaviour's List of Inputs
                        int j = 0;
                        foreach(string key in pool.info.tempInputs)
                        {
                            //Set the value of the selected Input in the Dictionary to the Network's Input value
                            objectBehaviour.inputs.Add(key, inputs[j][0]);
                            if(j < pool.info.tempInputs.Count-2)
                                j++;
                        }
                        j = 0;
                        foreach (string key in pool.info.variableOutputs)
                        {
                            //Set the value of the selected output in the Dictionary to the Network's Output value
                            objectBehaviour.outputs.Add(key, outputs[j][0]);
                            if (j < pool.info.tempInputs.Count - 2)
                                j++;
                        }
                    }

                    if (i < pool.networks.Count)
                        i++;
                }
            }
        }

        /// <summary>
        /// Creates a Neural Network from the Network Info with the given ID, and returns the created Network. There must be a Network Info Scriptable Object for the given NetworkID, or this method will do nothing.
        /// </summary>
        /// <param name="NetworkID">The name of the Network Info Object to use as the Starting Point of the Network Group</param>
        public static NeuralNetwork CreateNetwork(string NetworkID)
        {
            //Get the Network Info
            NeuralNetworkTemplate info = genePools[NetworkID].info;

            //Optional Object Parameters
            GameObject networkObject = null;
            NeuralNetworkBehaviour behaviour = null;

            //If the Network has a Prefab, Instantiate it
            if (info.prefab != null)
            {
                networkObject = InstantiatePrefab(info);
                behaviour = networkObject.GetComponent<NeuralNetworkBehaviour>();
                info = behaviour.template;
            }

            //Create the Neural Network
            GenePool genePool = genePools[NetworkID];
            NeuralNetwork network = new NeuralNetwork(info);

            if (genePool.parents.Count > 1)
            {
                //Find Mates for the Generation of this Network
                NeuralNetwork firstParent = genePool.FindMate(network);
                NeuralNetwork secondParent = genePool.FindMate(firstParent);

                //Remove the Parents from the Crossover Canadidates List
                genePool.crossoverCandidates.Remove(firstParent);
                genePool.crossoverCandidates.Remove(secondParent);

                //Set the network from the Two Parents
                network = new NeuralNetwork(firstParent, secondParent);
            }

            //Add the new Network to the Gene Pool
            genePool.networks.Add(network);

            //If the Network has a Prefab, pair the Network and Behaviour
            if (networkObject != null)
                SetupNetworkObject(info, network, networkObject, behaviour);

            //Return the Created Network
            return genePools[NetworkID].networks[genePools[NetworkID].networks.Count-1];
        }

        /// <summary>
        /// Instantiates the Prefab for a Neural Network Template
        /// </summary>
        /// <returns>The prefab.</returns>
        /// <param name="info">Info.</param>
        private static GameObject InstantiatePrefab(NeuralNetworkTemplate info)
        {
            //Network Object Properties
            GameObject networkObject = Engine.CreatePrefabInstance(info.prefab);
            NeuralNetworkBehaviour behaviour = networkObject.AddComponent<NeuralNetworkBehaviour>();

            //If the Object has sensers
            if (info.sensers.Count > 0)
            {
                //Clear the Temporary Inputs and Add the user defined Variable Inputs
                info.tempInputs.Clear();
                info.tempInputs.AddRange(info.variableInputs.GetRange(0,info.variableInputs.Count));

                //Loop through all the Senser Templates to add them to the Object
                foreach (SenserTemplate senserInfo in info.sensers)
                {
                    //Add the Senser Behaviour
                    Senser senserBehaviour = (Senser)networkObject.AddComponent(Type.GetType(senserInfo.MonoBehaviour));
                    GameObject behaviourObject = GameObject.Instantiate(senserInfo.prefab);

                    //Set up Senser props
                    senserBehaviour.template = senserInfo;
                    senserBehaviour.senserObject = behaviourObject;
                    senserBehaviour.agentObject = networkObject.gameObject;

                    //Add the Senser Object to the Network object
                    senserBehaviour.senserObject.transform.parent = networkObject.transform;

                    //Initialize the Senser Behaviour
                    senserBehaviour.Init();

                    //Recieve Inputs from the Senser Info
                    List<string> recievedInputs = senserInfo.GetInputs(info.tempInputs);

                    //Loop through all the Inputs Recieved from the Senser Template, and add them to the Inputs Lists
                    for (int i = 0; i < recievedInputs.Count; i++)
                    {
                        //Add the Recieved Input to the Network Info's Temp Inputs List
                        info.tempInputs.Add(recievedInputs[i]);
                        //Add the Recieved Input to the Senser's Inputs List with the Index in the Network Info's Temp Input List and the default value of 0.0
                        senserBehaviour.inputs.Add(info.tempInputs.Count, 0.0);
                        //Add the Senser Behaviour to the Senser's Input Names List with it's index in the Network Info's Temp Inputs List 
                        senserBehaviour.inputNames.Add(recievedInputs[i], info.tempInputs.Count);
                    
                        //FINAL NOTE: This system allows Named Inputs to be added to the Network and Senser independently to set Input values by name.
                    }
                }
            }

            behaviour.template = info;

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
        private static void SetupNetworkObject(NeuralNetworkTemplate info, NeuralNetwork network, GameObject networkObject, NeuralNetworkBehaviour behaviour)
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
            behaviour.template = info;
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

            foreach(SenserTemplate senserInfo in network.info.sensers)
            {

            }

            return inputList.ToArray();
        }

        /// <summary>
        /// Kills an AI
        /// </summary>
        /// <param name="network">Network.</param>
        public static void Kill(NeuralNetwork network)
        {
            genePools[network.info.ID].networks.Remove(network);
            if(network.gameObject != null)
            {
                GameObject.Destroy(network.gameObject);
            }
        }

        /// <summary>
        /// Kills an AI that is attached to the specified Game Object.
        /// </summary>
        /// <param name="networkObject">Network object.</param>
        public static void Kill(GameObject networkObject)
        {
            NeuralNetworkBehaviour networkBehaviour = networkObject.GetComponent<NeuralNetworkBehaviour>();
            genePools[networkBehaviour.template.ID].networks.Remove(networkBehaviour.network);
            GameObject.Destroy(networkObject);
        }

        /// <summary>
        /// Adds the Network to the List of Parents that are ready to Mate.
        /// </summary>
        /// <param name="network">Network.</param>
        public static void Prime(NeuralNetwork network)
        {
            genePools[network.info.ID].crossoverCandidates.Add(network);
        }

        /// <summary>
        /// Adds the Network attached to the specified Game Object to the List of Parents that are ready to mate.
        /// </summary>
        /// <param name="networkObject">Network object.</param>
        public static void Prime(GameObject networkObject)
        {
            NeuralNetworkBehaviour networkBehaviour = networkObject.GetComponent<NeuralNetworkBehaviour>();
            genePools[networkBehaviour.network.info.ID].crossoverCandidates.Add(networkBehaviour.network);
        }

        /// <summary>
        /// A Container of Neural Networks
        /// </summary>
        private class GenePool
        {
            public NeuralNetworkTemplate info;
            public List<NeuralNetwork> networks = new List<NeuralNetwork>();
            public List<NeuralNetwork> crossoverCandidates = new List<NeuralNetwork>();
            public List<NeuralNetwork> parents = new List<NeuralNetwork>();

            public GenePool(NeuralNetworkTemplate info)
            {
                this.info = info;
            }

            /// <summary>
            /// Call the Crossover Method for all Crossover Candidates
            /// </summary>
            //public void Breed()
            //{
            //    while (crossoverCandidates.Count > 0)
            //    {
            //        Crossover(crossoverCandidates.Dequeue());
            //    }
            //}

            /// <summary>
            /// Crossover genes from two Parents
            /// </summary>
            /// <param name="firstParent">First parent.</param>
            //public void Crossover(NeuralNetwork firstParent)
            //{
            //    NeuralNetwork secondParent = FindMate(firstParent);
            //    MLModule.CreateNetwork(firstParent, secondParent);
            //}

            /// <summary>
            /// Finds a good mate for the Parent
            /// </summary>
            /// <returns>The mate.</returns>
            /// <param name="firstParent">First parent.</param>
            public NeuralNetwork FindMate(NeuralNetwork firstParent)
            {
                double fitness = firstParent.GetFitness();

                if (crossoverCandidates.Count > 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (NeuralNetwork network in crossoverCandidates)
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
