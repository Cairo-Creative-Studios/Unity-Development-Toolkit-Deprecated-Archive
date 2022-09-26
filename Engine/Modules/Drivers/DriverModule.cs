/*! \addtogroup behaviourmodule Behaviour Module
 *  Additional documentation for group 'Behaviour Module'
 *  @{
 */

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Reflection;

namespace CairoEngine.Drivers
{
    /// <summary>
    /// The Driver Manager (Previously called the Behaviour Module) handles Drivers 
    /// </summary>
    public class DriverModule : MonoBehaviour
    {
		public bool initialize = true;
        private static GameObject behaviourObjectPool;

        /// <summary>
        /// All the Game Objects with their Behaviours managed by the Behaviour Module
        /// </summary>
        public static Dictionary<GameObject, DriverCore> behaviours = new Dictionary<GameObject, DriverCore>();
        /// <summary>
        /// The Default Audio Library
        /// </summary>
        public static Dictionary<string, Dictionary<string, List<AudioClip>>> defaultAudio = new Dictionary<string, Dictionary<string, List<AudioClip>>>();

        public static DriverModule singleton;

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            GameObject singletonObject = new GameObject();
            singleton = singletonObject.AddComponent<DriverModule>();
            GameObject.DontDestroyOnLoad(singletonObject);
            singletonObject.name = "Cairo Driver Module";

			if (!singleton.initialize)
				GameObject.Destroy(singletonObject);
		}

        /// <summary>
        /// Adds the Behaviour and passes the given Behaviour Type Info to it.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="Template">The Template of the Behaviour to add to the object</param>
        public static void AddDriver(GameObject gameObject, string state, object Template, DriverCore core = null)
        {
            //Get the Behavour Template and Behaviour
            DriverTemplate template = (DriverTemplate)Template;
            object behaviourType = gameObject.AddComponent(Type.GetType(template.behaviourClass));
            
            behaviourType.SetField("template",template);
			behaviourType.SetField("core", core);

            //Get the Root Object Behaviour attached to the Game Object 
            if (!behaviours.ContainsKey(gameObject))
            {
                //Create one if it doesn't exist, so the Behaviour Module can interface with it, and add it to the Behaviours List
                behaviours.Add(gameObject, gameObject.GetComponent<DriverCore>());
            }

            DriverCore driverCore = gameObject.GetComponent<DriverCore>();
            if(driverCore == null)
                driverCore = gameObject.AddComponent<DriverCore>();

            //Add the Behaviour to the Object's Behaviour List
            if (!driverCore.states.ContainsKey(state))
                driverCore.states.Add(state, new List<object>());
            driverCore.states[state].Add(behaviourType);

            //Set Behaviour Properties
            behaviourType.SetField("gameObject", gameObject);
            behaviourType.SetField("transform", gameObject.transform);

            //Get the Root Transform and Animator using the Paths in the Template, to child objects instanced with the Prefab
            if (driverCore.template != null)
            {
                if (driverCore.template.rootPath != "")
                    behaviourType.SetField("rootTransform", driverCore.transform.Find(driverCore.template.rootPath));
                else
                    behaviourType.SetField("rootTransform", driverCore.transform);
                if (driverCore.template.animatorPath != "")
                    behaviourType.SetField("animator", driverCore.transform.Find(driverCore.template.animatorPath).gameObject.GetComponent<Animator>());
                else
                    behaviourType.SetField("animator", driverCore.GetComponent<Animator>());
            }
            else
            {
                behaviourType.SetField("rootTransform", driverCore.transform);
                behaviourType.SetField("animator", driverCore.GetComponent<Animator>());
            }

            //Add all the Inputs to the Behaviour from the Input Map
            foreach (string inputName in template.inputMap.Keys)
            {
                behaviourType.CallMethod("AddInput", new object[] { template.inputMap[inputName], inputName });

            }

            //Inherit the Script Container
            //behaviourType.SetField("scriptContainer", template.scriptContainer);
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
                    foreach (object behaviourType in behaviours[gameObject].states[behaviours[gameObject].currentState])
                    {
                        behaviourType.CallMethod(message, parameters);
                    }
                }
                else
                {
                    foreach(object behaviourType in behaviours[gameObject].states[behaviours[gameObject].currentState])
                    {
                        if(((DriverTemplate)behaviourType.GetField("template")).ID == behaviour)
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
        public static object Message<T>(DriverCore cobject, string message, object[] parameters)
        {
            GameObject selectedObject = cobject.gameObject;

            foreach(object behaviour in cobject.states[cobject.currentState])
            {
                if (behaviour.GetType() == typeof(T))
                    return behaviour.CallMethod(message, parameters);
            }

            return null;
        }

        public static void MessageCore(GameObject gameObject, string message, object[] parameters)
        {
            if(behaviours.ContainsKey(gameObject))
                behaviours[gameObject].CallMethod(message, parameters);
        }

        /// <summary>
        /// Adds a Behaviour Specific Object to the Behaviour Object Pool 
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        public static void AddBehaviourObject(GameObject gameObject)
        {
            //gameObject.transform.parent = behaviourObjectPool.transform;
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
    }
}
