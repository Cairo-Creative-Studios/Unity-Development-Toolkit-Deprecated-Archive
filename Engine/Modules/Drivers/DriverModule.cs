/*! \addtogroup drivermodule Driver Module
 *  Additional documentation for group 'Driver Module'
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
    /// The Driver Manager (Previously called the Driver Module) handles Drivers 
    /// </summary>
    public class DriverModule : MonoBehaviour
    {
        public bool initialize = true;
        private static GameObject driverObjectPool;

        /// <summary>
        /// All the Game Objects with their Drivers managed by the Driver Module
        /// </summary>
        public static Dictionary<GameObject, DriverCore> drivers = new Dictionary<GameObject, DriverCore>();
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
        /// Adds the Driver and passes the given Driver Type Info to it.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="Template">The Template of the Driver to add to the object</param>
        public static void AddDriver(GameObject gameObject, string state, object Template, DriverCore core = null)
        {
            //Get the Behavour Template and Driver
            DriverTemplate template = (DriverTemplate)Template;
            object driverType = gameObject.AddComponent(Type.GetType(template.driverProperties.main.driverClass));

            driverType.SetField("template", template);
            driverType.SetField("core", core);

            //Get the Root Object Driver attached to the Game Object 
            if (!drivers.ContainsKey(gameObject))
            {
                //Create one if it doesn't exist, so the Driver Module can interface with it, and add it to the Drivers List
                drivers.Add(gameObject, gameObject.GetComponent<DriverCore>());
            }

            DriverCore driverCore = gameObject.GetComponent<DriverCore>();
            if (driverCore == null)
                driverCore = gameObject.AddComponent<DriverCore>();

            //Add the Driver to the Object's Driver List
            if (!driverCore.states.ContainsKey(state))
                driverCore.states.Add(state, new List<object>());
            driverCore.states[state].Add(driverType);

            //Set Driver Properties
            driverType.SetField("gameObject", gameObject);
            driverType.SetField("transform", gameObject.transform);

            //Get the Root Transform and Animator using the Paths in the Template, to child objects instanced with the Prefab
            if (driverCore.template != null)
            {
                if (driverCore.template.rootPath != "")
                    driverType.SetField("rootTransform", driverCore.transform.Find(driverCore.template.rootPath));
                else
                    driverType.SetField("rootTransform", driverCore.transform);
                if (driverCore.template.animatorPath != "")
                    driverType.SetField("animator", driverCore.transform.Find(driverCore.template.animatorPath).gameObject.GetComponent<Animator>());
                else
                    driverType.SetField("animator", driverCore.GetComponent<Animator>());
            }
            else
            {
                driverType.SetField("rootTransform", driverCore.transform);
                driverType.SetField("animator", driverCore.GetComponent<Animator>());
            }

            //Add all the Inputs to the Driver from the Input Translation Dictionary
            foreach (string inputName in template.driverProperties.input.inputTranslation.Keys)
            {
                driverType.CallMethod("AddInput", new object[] { template.driverProperties.input.inputTranslation[inputName], inputName });
            }

            //Inherit the Script Container
            //driverType.SetField("scriptContainer", template.scriptContainer);
        }

        /// <summary>
        /// Sends a Message to Drivers in a Game Object. If <paramref name="driver"/> is set, it will only send the Message to the Driver with that Name.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="message">Message.</param>
        /// <param name="parameters">Parameters.</param>
        /// <param name="driver">Driver.</param>
        public static void Message(GameObject gameObject, string message, object[] parameters = null, string driver = "")
        {
            if (drivers.ContainsKey(gameObject))
            {
                if (driver == "")
                {
                    foreach (object driverType in drivers[gameObject].states[drivers[gameObject].currentState])
                    {
                        driverType.CallMethod(message, parameters);
                    }
                }
                else
                {
                    foreach (object driverType in drivers[gameObject].states[drivers[gameObject].currentState])
                    {
                        if (((DriverTemplate)driverType.GetField("template")).driverProperties.main.ID == driver)
                        {
                            driverType.CallMethod(message, parameters);
                        }
                    }
                }
            }
            else
                Debug.LogWarning("Can't send Message to Object, " + gameObject.name + ", as no Drivers have been added to it.");
        }

        /// <summary>
        /// Sends a Message to a Driver in the Specified Cairo Object
        /// </summary>
        /// <returns>The message.</returns>
        /// <param name="cobject">Cobject.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static object Message<T>(DriverCore cobject, string message, object[] parameters)
        {
            GameObject selectedObject = cobject.gameObject;

            foreach (object driver in cobject.states[cobject.currentState])
            {
                if (driver.GetType() == typeof(T))
                    return driver.CallMethod(message, parameters);
            }

            return null;
        }

        public static void MessageCore(GameObject gameObject, string message, object[] parameters)
        {
            if (drivers.ContainsKey(gameObject))
                drivers[gameObject].CallMethod(message, parameters);
        }

        /// <summary>
        /// Adds a Driver Specific Object to the Driver Object Pool 
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        public static void AddDriverObject(GameObject gameObject)
        {
            //gameObject.transform.parent = driverObjectPool.transform;
        }

        public static void RemoveDriverObject(GameObject gameObject)
        {
            if (drivers.ContainsKey(gameObject))
            {
                drivers.Remove(gameObject);
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
