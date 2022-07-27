//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using CairoEngine.Behaviour;
using CairoEngine.Reflection;

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
        private static GameObject behaviourObjectPool;

        /// <summary>
        /// All the Game Objects with their Behaviours managed by the Behaviour Module
        /// </summary>
        public static Dictionary<GameObject, List<BehaviourType<object>>> behaviours = new Dictionary<GameObject, List<BehaviourType<object>>>();
    
        public static void Init()
        {
            behaviourTemplates.AddRange(Resources.LoadAll<BehaviourTypeTemplate>(""));
            behaviourObjectPool = new GameObject();
            Object.DontDestroyOnLoad(behaviourObjectPool);
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
        public static void AddBehaviour(GameObject gameObject, Type Template)
        {
            //Get the Behavour Template and Behaviour
            BehaviourTypeTemplate template = (BehaviourTypeTemplate)(object)Template;
            BehaviourType<object> behaviourType = (BehaviourType<object>)Activator.CreateInstance(Type.GetType(template.behaviourClass));
            behaviourType.template = template;

            //Root Object
            Object root;

            //Get the Root Object Behaviour attached to the Game Object 
            if (!behaviours.ContainsKey(gameObject))
            {
                //Create one if it doesn't exist, so the Behaviour Module can interface with it, and add it to the Behaviours List
                behaviours.Add(gameObject, new List<BehaviourType<object>>());

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

            //Get the Root Transform and Animator using the Paths in the Template, to child objects instanced with the Prefab
            if (root.template.rootPath != "")
                behaviourType.rootTransform = root.transform.Find(root.template.rootPath);
            else
                behaviourType.rootTransform = root.transform;
            if (root.template.animatorPath != "")
                behaviourType.animator = root.transform.Find(root.template.animatorPath).gameObject.GetComponent<Animator>();
            else
                behaviourType.animator = root.GetComponent<Animator>();

            //Add the Behaviour to Object in the Module's Behaviour Collection
            behaviours[gameObject].Add(behaviourType);

            //Add all the Inputs to the Behaviour from the Input Map
            foreach (string inputName in template.inputMap.Keys)
            {
                behaviourType.inputMap.Add(template.inputMap[inputName], inputName);
                behaviourType.inputs.Add(inputName, 0.0f);
            }

            //Call the Init Message on the Behaviour 
            Message(gameObject, "Init", null);
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
                if (behaviour == "")
                {
                    foreach (BehaviourType<object> behaviourType in behaviours[gameObject])
                    {
                        behaviourType.CallMethod(message, parameters);
                    }
                }
                else
                {
                    foreach(BehaviourType<object> behaviourType in behaviours[gameObject])
                    {
                        if(((BehaviourTypeTemplate)(object)behaviourType.template).ID == behaviour)
                        {
                            behaviourType.CallMethod(message, parameters);
                        }
                    }
                }
            }
            else
                Debug.LogWarning("Can't send Message to Object, " + gameObject.name + ", as no Behaviours have been added to it.");
        }

        /// <summary>
        /// Adds a Behaviour Specific Object to the Behaviour Object Pool 
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        public static void AddBehaviourObject(GameObject gameObject)
        {
            gameObject.transform.parent = behaviourObjectPool.transform;
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
