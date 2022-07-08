//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using CairoEngine.Behaviour;

namespace CairoEngine
{
    /// <summary>
    /// Manages Cairo Engine Object Behaviours. Allows you to Add, Interact with and Change Behaviour values on any Game Object.
    /// You can do this programmatically for runtime Customization, or Create Objects/Prefabs that generate them using the BehaviourTypeInfo lists.
    /// </summary>
    public class BehaviourModule
    {
        /// <summary>
        /// All the loaded Behaviour Templates
        /// </summary>
        private static List<BehaviourTypeTemplate> behaviourTemplates = new List<BehaviourTypeTemplate>();
        /// <summary>
        /// All the Game Objects with their Behaviours managed by the Behaviour Module
        /// </summary>
        public static Dictionary<GameObject, List<BehaviourType>> behaviours = new Dictionary<GameObject, List<BehaviourType>>();
    
        public static void Init()
        {
            behaviourTemplates.AddRange(Resources.LoadAll<BehaviourTypeTemplate>(""));
        }

        public static void Update()
        {
            foreach(GameObject gameObject in behaviours.Keys)
            {
                //Update the Core of the Behaviour
                Message(gameObject, "CoreUpdate");
                //Update the Behaviour
                Message(gameObject, "Update");
            }
        }

        public static void FixedUpdate()
        {
            foreach (GameObject gameObject in behaviours.Keys)
            {
                Message(gameObject, "FixedUpdate");
            }
        }

        /// <summary>
        /// Adds the Behaviour and passes the given Behaviour Type Info to it.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="BehaviourName">The name of the Behaviour Template</param>
        public static void AddBehaviour(GameObject gameObject, string BehaviourName)
        {
            //Get the Behavour Template and Behaviour
            BehaviourTypeTemplate template = GetTemplate(BehaviourName);
            BehaviourType behaviourType = (BehaviourType)Activator.CreateInstance(Type.GetType(template.behaviourClass));
            behaviourType.template = template;

            //Root Object
            Object root;

            //Get the Root Object Behaviour attached to the Game Object 
            if (!behaviours.ContainsKey(gameObject))
            {
                //Create one if it doesn't exist, so the Behaviour Module can interface with it, and add it to the Behaviours List
                behaviours.Add(gameObject, new List<BehaviourType>());

                root = gameObject.GetComponent<Object>();

                if (root==null)
                    root = gameObject.AddComponent<Object>();

                behaviourType.root = root;
            }
            else
                root = gameObject.GetComponent<Object>();

            //Add the Behaviour to the Object's Behaviour List
            root.behaviours.Add(behaviourType);

            //Set Behaviour Properties
            behaviourType.gameObject = gameObject;
            behaviourType.transform = gameObject.transform;

            //Add the Behaviour to Object in the Module's Behaviour Collection
            behaviours[gameObject].Add(behaviourType);

            //Add all the Inputs to the Behaviour from the Input Map
            foreach (string inputName in template.inputMap.Keys)
            {
                behaviourType.inputMap.Add(template.inputMap[inputName], inputName);
                behaviourType.inputs.Add(inputName, 0.0f);
            }

            //Call the Init Message on the Behaviour 
            Message(gameObject, "Init", null, BehaviourName);
        }

        /// <summary>
        /// Sends a Message to Behaviours in a Game Object. If <paramref name="behaviour"/> is set, it will only send the Message to the Behaviour with that Name.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="message">Message.</param>
        /// <param name="parameters">Parameters.</param>
        /// <param name="behaviour">Behaviour.</param>
        public static void Message(GameObject gameObject, string message,object[] parameters = null,string behaviour = "")
        {
            if (behaviours.ContainsKey(gameObject))
            {
                bool methodFound = false;
                if (behaviour == "")
                {
                    foreach (BehaviourType behaviourType in behaviours[gameObject])
                    {
                        MethodInfo method = behaviourType.GetType().GetMethod(message);
                        if(method != null)
                        {
                            if (behaviourType.Enabled)
                            {
                                methodFound = true;
                                method.Invoke(behaviourType, parameters);
                            }
                        }
                    }
                    //if (!methodFound)
                    //    Debug.LogWarning("Cannot declare non-existent Method for sent message to " + gameObject);
                }
                else
                {
                    bool found = false;
                    foreach(BehaviourType behaviourType in behaviours[gameObject])
                    {
                        if(behaviourType.template.ID == behaviour)
                        {
                            found = true;

                            MethodInfo method = behaviourType.GetType().GetMethod(message);
                            if (method != null)
                            {
                                if (behaviourType.Enabled)
                                {
                                    methodFound = true;
                                    method.Invoke(behaviourType, parameters);
                                }
                            }
                        }
                    }
                    if (!found)
                        Debug.LogWarning("Can't send Message to non-existent Behaviour of " + gameObject.name);
                    else if (!methodFound)
                        Debug.LogWarning("Cannot declare non-existent Method for sent message to Behaviour " + behaviour + " in " + gameObject);
                }
            }
            else
                Debug.LogWarning("Can't send Message to Object, " + gameObject.name + ", as no Behaviours have been added to it.");
        }

        /// <summary>
        /// Gets the specified template.
        /// </summary>
        /// <returns>The template.</returns>
        /// <param name="ID">Identifier.</param>
        private static BehaviourTypeTemplate GetTemplate(string ID)
        {
            foreach(BehaviourTypeTemplate template in behaviourTemplates)
            {
                if (template.ID == ID)
                    return template;
            }
            return null;
        }
    }
}
