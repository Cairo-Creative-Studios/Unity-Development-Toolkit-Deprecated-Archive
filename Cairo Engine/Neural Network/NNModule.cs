using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.MachineLearning;
using System.Linq;

namespace CairoEngine
{
    /// <summary>
    /// The Neuro Network Module allows you to add create Neural Networks, and attach them to objects. 
    /// </summary>
    public class NNModule
    {
        private static Dictionary<string, Dictionary<GameObject, NeuralNetwork>> networks = new Dictionary<string, Dictionary<GameObject, NeuralNetwork>>();
        private static List<NeuralNetworkTemplate> neuralNetworkTemplates = new List<NeuralNetworkTemplate>();

        /// <summary>
        /// Initialize the Neural Network Module.
        /// </summary>
        public static void Init()
        {
            neuralNetworkTemplates.AddRange(Resources.LoadAll<NeuralNetworkTemplate>(""));
        }

        /// <summary>
        /// Update the Neural Network Module.
        /// </summary>
        public static void Update()
        {
            //Update all the Specimens in the Module
            foreach(string pool in networks.Keys)
                foreach(GameObject child in networks[pool].Keys)
                {
                    //Add to their Life Spans
                    networks[pool][child].lifespan++;

                    //If the Life Span of the Specimen has exceeded the Max Lifespan set in the Template, Reproduce
                    if(networks[pool][child].lifespan> networks[pool][child].template.maxLifespan&&networks[pool][child].template.maxLifespan>0)
                    {
                        Reproduce(networks[pool][child].gameObject);
                    }

                    //Update the Specimens, but calling the Train method
                    networks[pool][child].Train();
                }
        }

        /// <summary>
        /// Creates a Specimen of the Specified Neural Network
        /// </summary>
        /// <param name="networkID">The name of the Neural Network</param>
        /// <param name="structure">The Sizes of each Neural Network Layer</param>
        /// <param name="gameObject">Optional Game Object that can be used in place of the default object in the Network Template.</param>
        public static void CreateSpecimen(string networkID, int[] structure, GameObject gameObject = null)
        {
            //Create a Network Structure list from the passed Structure argument
            List<int> networkStructure = new List<int>();
            networkStructure.AddRange(structure);

            //If the Game Object is a Controller, add the Inputs of the Controller the Inputs Layer.
            if (gameObject.GetComponent<Controller>() != null)
            {
                Controller controller = gameObject.GetComponent<Controller>();
                //Get all the Inputs and add to the Size of the Input Layer
                foreach(string input in controller.inputs.Keys)
                {
                    networkStructure[0] += 1;
                }
            }

            //If the List in the Networks Dictionary doesn't already exist, then Create it
            if (!networks.ContainsKey(networkID))
            {
                networks.Add(networkID, new Dictionary<GameObject, NeuralNetwork>());
            }

            //Get the Template for the Network
            NeuralNetworkTemplate template = GetNeuralNetworkTemplate(networkID);
            //If a Template doesn't exist, create a new one
            if(template == null)
            {
                template = ScriptableObject.CreateInstance<NeuralNetworkTemplate>();
            }
            //If the user passed a Game Object to use, pass that as the Game Object paramater
            if(gameObject != null)
                //Add the Network to the group of Networks that the Module should Control
                networks[networkID].Add(gameObject, new NeuralNetwork(networkStructure.ToArray(), gameObject, networkID));
            //Otherwise, attempt to pass the Template's Prefab
            else
            {
                GameObject thisObject;

                //If the Object Prefab hasn't been set, create an empty Game Object, otherwise Instantiate the Prefab
                if (template.objectPrefab == null)
                    thisObject = new GameObject();
                else
                    thisObject = Object.Instantiate(template.objectPrefab);


                //Add the Network to the group of Networks that the Module should Control, with the new Game Object.
                networks[networkID].Add(thisObject, new NeuralNetwork(networkStructure.ToArray(), thisObject, networkID));
            }


        }

        /// <summary>
        /// Reproduces an AI Specimen
        /// </summary>
        /// <param name="specimen">Specimen.</param>
        public static void Reproduce(GameObject specimen)
        {
            List<List<List<float>>> combinedWeightsList = new List<List<List<float>>>();

            NeuralNetwork originalNetwork = GetGameObjectsNetwork(specimen);
            NeuralNetwork otherParent = GetRandomNetworkOfSameType(specimen);

            for(int i = 0; i < originalNetwork.weights.Length; i++)
            {
                for(int j = 0; j < originalNetwork.weights[i].Length; j++)
                {
                    for(int k = 0; k < originalNetwork.weights[i][j].Length; k++)
                    {
                        if (UnityEngine.Random.Range(0,1) > 0.55)
                            combinedWeightsList[i][j][k] = originalNetwork.weights[i][j][k];
                        else
                            combinedWeightsList[i][j][k] = otherParent.weights[i][j][k];
                    }
                }
            }

            //Convert the Combined Weight List to an Array
            float[][][] combinedWeights = combinedWeightsList.Select(a => a.Select(b => b.ToArray()).ToArray()).ToArray();

            //Replace the previous Specimen.
            NeuralNetwork newNetwork = new NeuralNetwork(originalNetwork, combinedWeights);
            networks[newNetwork.networkID][newNetwork.gameObject] = newNetwork;
        }

        //TODO: Test the performance of Getting the Neural Network by Game Object, so that the user doesn't have to specify what Network they're a part of.
        /// <summary>
        /// Set one of the Inputs for a Specimen in a Neural Network.
        /// </summary>
        /// <param name="network">The Name of the Network.</param>
        /// <param name="gameObject">The Game Object that we're Setting Inputs for.</param>
        /// <param name="input">The Index in the Input Layer belonging to this Input.</param>
        /// <param name="value">The Value to set the Input to.</param>
        public static void SetInput(string network, GameObject gameObject, int input, float value)
        {
            networks[network][gameObject].SetInput(input, value);
        }

        /// <summary>
        /// Gets the current Output value of the Neural Network
        /// </summary>
        /// <returns>The output.</returns>
        /// <param name="network">The Neural Network this Speicimen Uses.</param>
        public static float[] GetOutput(string network, GameObject networkID)
        {
            return networks[network][networkID].outputs;
        }

        /// <summary>
        /// Adds to the Specimens Fitness value
        /// </summary>
        /// <param name="specimen">The Specimen to add to</param>
        /// <param name="amount">The amount of Fitness to add</param>
        public static void AddFitness(GameObject specimen, float amount)
        {
            GetGameObjectsNetwork(specimen).fitness += amount;
        }

        /// <summary>
        /// Get the Template of a Neural Network, if it exists.
        /// </summary>
        /// <returns>The neural network template.</returns>
        /// <param name="ID">Identifier.</param>
        public static NeuralNetworkTemplate GetNeuralNetworkTemplate(string ID)
        {
            foreach(NeuralNetworkTemplate template in neuralNetworkTemplates)
            {
                if (template.ID == ID)
                {
                    return template;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the Neural Network that belongs to a Game Object.
        /// </summary>
        /// <returns>The game objects network.</returns>
        /// <param name="gameObject">Game object.</param>
        public static NeuralNetwork GetGameObjectsNetwork(GameObject gameObject)
        {
            //Check all Networks to find the Specimen
            foreach (Dictionary<GameObject, NeuralNetwork> network in networks.Values)
            {
                //If it's found, create a new Neural Network and replace the previous
                if (network.ContainsKey(gameObject))
                {
                    return network[gameObject];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a Random Network of the same Type as the Game Object
        /// </summary>
        /// <returns>The random network of same type.</returns>
        /// <param name="gameObject">Game object.</param>
        public static NeuralNetwork GetRandomNetworkOfSameType(GameObject gameObject)
        {
            //Check all Networks to find the Specimen
            foreach (Dictionary<GameObject, NeuralNetwork> network in networks.Values)
            {
                //If it's found, create a new Neural Network and replace the previous
                if (network.ContainsKey(gameObject) && network.Count>1)
                {
                    bool found = false;

                    //Keep searching until a network is Randomly picked and return it
                    while (!found)
                    {
                        foreach(NeuralNetwork randomNetwork in network.Values)
                        {
                            if(randomNetwork.gameObject != gameObject&&UnityEngine.Random.Range(0,1)>0.75)
                            {
                                return randomNetwork;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }

}
