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
        private static List<BehaviourTemplate> behaviourTemplates = new List<BehaviourTemplate>();
        private static GameObject behaviourObjectPool;

        /// <summary>
        /// All the Game Objects with their Behaviours managed by the Behaviour Module
        /// </summary>
        public static Dictionary<GameObject, List<object>> behaviours = new Dictionary<GameObject, List<object>>();
        /// <summary>
        /// The Default Audio Library
        /// </summary>
        public static Dictionary<string, Dictionary<string, List<AudioClip>>> defaultAudio = new Dictionary<string, Dictionary<string, List<AudioClip>>>();

        public static void Init()
        {
            behaviourTemplates.AddRange(Resources.LoadAll<BehaviourTemplate>(""));

            //behaviourObjectPool = new GameObject();
            //GameObject.DontDestroyOnLoad(behaviourObjectPool);
        }

        public static void Update()
        {
            List<GameObject> behaviourCopy = new List<GameObject>();
            behaviourCopy.AddRange(behaviours.Keys);

            foreach (GameObject gameObject in behaviourCopy)
            {
                //Update the Core of the Behaviour
                Message(gameObject, "CoreUpdate");
                //Update the Behaviour
                Message(gameObject, "Update");
            }
        }

        public static void FixedUpdate()
        {
            List<GameObject> behaviourCopy = new List<GameObject>();
            behaviourCopy.AddRange(behaviours.Keys);

            foreach (GameObject gameObject in behaviourCopy)
            {
                Message(gameObject, "FixedUpdate");
            }
        }

        /// <summary>
        /// Adds the Behaviour and passes the given Behaviour Type Info to it.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="BehaviourName">The name of the Behaviour Template</param>
        public static void AddBehaviour(GameObject gameObject, object Template)
        {
            //Get the Behavour Template and Behaviour
            BehaviourTemplate template = (BehaviourTemplate)Template;
            object behaviourType = (object)Activator.CreateInstance(Type.GetType(template.behaviourClass));
            behaviourType.SetField("template",template);

            //Root Object
            CObject root;

            //Get the Root Object Behaviour attached to the Game Object 
            if (!behaviours.ContainsKey(gameObject))
            {
                //Create one if it doesn't exist, so the Behaviour Module can interface with it, and add it to the Behaviours List
                behaviours.Add(gameObject, new List<object>());
            }

            root = gameObject.GetComponent<CObject>();

            if (root == null)
                root = gameObject.AddComponent<CObject>();

            behaviourType.SetField("root", root);

            //Add the Behaviour to the Object's Behaviour List
            root.behaviours.Add(behaviourType);

            //Set Behaviour Properties
            behaviourType.SetField("gameObject", gameObject);
            behaviourType.SetField("transform", gameObject.transform);

            //Get the Root Transform and Animator using the Paths in the Template, to child objects instanced with the Prefab
            if (root.template.rootPath != "")
                behaviourType.SetField("rootTransform", root.transform.Find(root.template.rootPath));
            else
                behaviourType.SetField("rootTransform", root.transform);
            if (root.template.animatorPath != "")
                behaviourType.SetField("animator", root.transform.Find(root.template.animatorPath).gameObject.GetComponent<Animator>());
            else
                behaviourType.SetField("animator", root.GetComponent<Animator>());

            //Add the Behaviour to Object in the Module's Behaviour Collection
            behaviours[gameObject].Add(behaviourType);

            //Add all the Inputs to the Behaviour from the Input Map
            foreach (string inputName in template.inputMap.Keys)
            {
                behaviourType.CallMethod("AddInput", new object[] { template.inputMap[inputName], inputName });

            }

            behaviourType.CallMethod("Init");

            //Call the Init Message on the Behaviour 
            //Message(gameObject, "Init", null);
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
                    foreach (object behaviourType in behaviours[gameObject])
                    {
                        behaviourType.CallMethod(message, parameters);
                    }
                }
                else
                {
                    foreach(object behaviourType in behaviours[gameObject])
                    {
                        if(((BehaviourTemplate)behaviourType.GetField("template")).ID == behaviour)
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
        /// Sends a Message to a Behaviour in the Specified Cairo Object
        /// </summary>
        /// <returns>The message.</returns>
        /// <param name="cobject">Cobject.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static object Message<T>(CObject cobject, string message, object[] parameters)
        {
            GameObject selectedObject = cobject.gameObject;

            foreach(object behaviour in behaviours[selectedObject])
            {
                if (behaviour.GetType() == typeof(T))
                    return behaviour.CallMethod(message, parameters);
            }

            return null;
        }

        /// <summary>
        /// Adds a Behaviour Specific Object to the Behaviour Object Pool 
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        public static void AddBehaviourObject(GameObject gameObject)
        {
            gameObject.transform.parent = behaviourObjectPool.transform;
        }

        public static void RemoveBehaviourObject(GameObject gameObject)
        {
            if (behaviours.ContainsKey(gameObject))
            {
                behaviours.Remove(gameObject);
            }
        }


        /// <summary>
        /// Attempts to get Audio from the Defaults Library
        /// </summary>
        public static List<AudioClip> GetAudio(string audioPool, string clipName)
        {
            if (defaultAudio.ContainsKey(audioPool))
                if (defaultAudio[audioPool].ContainsKey(clipName))
                    return defaultAudio[audioPool][clipName];
            return null;
        }

        /// <summary>
        /// Gets the specified template.
        /// </summary>
        /// <returns>The template.</returns>
        /// <param name="ID">Identifier.</param>
        private static BehaviourTemplate GetTemplate(string ID)
        {
            foreach(BehaviourTemplate template in behaviourTemplates)
            {
                if (template.ID == ID)
                    return template;
            }
            return null;
        }
    }
}
