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
    public class BehaviourManager
    {
        public static Dictionary<GameObject, List<BehaviourType>> behaviours = new Dictionary<GameObject, List<BehaviourType>>();
    
        public static void Init()
        {

        }

        public static void Update()
        {
            foreach(GameObject gameObject in behaviours.Keys)
            {
                foreach (BehaviourType behaviourType in behaviours[gameObject])
                    behaviourType.Update();
            }
        }

        /// <summary>
        /// Adds the Behaviour and passes the given Behaviour Type Info to it.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="behaviourTypeInfo">Behaviour type info.</param>
        public static void AddBehaviour(GameObject gameObject, BehaviourTypeInfo  behaviourTypeInfo)
        {
            BehaviourType behaviourType = (BehaviourType)gameObject.AddComponent(Type.GetType(behaviourTypeInfo.behaviourClass));
            behaviourType.info = behaviourTypeInfo;
            behaviours[gameObject].Add(behaviourType);
        }

        /// <summary>
        /// Sends a Message to Behaviours in a Game Object. If <paramref name="behaviour"/> is set, it will only send the Message to the Behaviour with that Name.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="message">Message.</param>
        /// <param name="parameters">Parameters.</param>
        /// <param name="behaviour">Behaviour.</param>
        public static void Message(GameObject gameObject, string message,object[] parameters,string behaviour = "")
        {
            if (behaviours.ContainsKey(gameObject))
            {
                bool methodFound = false;
                if (behaviour != "")
                {
                    foreach (BehaviourType behaviourType in behaviours[gameObject])
                    {
                        MethodInfo method = behaviourType.GetType().GetMethod(message);
                        if(method != null)
                        {
                            methodFound = true;
                            method.Invoke(behaviourType, parameters);
                        }
                    }
                    if (!methodFound)
                        Debug.LogWarning("Cannot declare non-existent Method for sent message to " + gameObject);
                }
                else
                {
                    bool found = false;
                    foreach(BehaviourType behaviourType in behaviours[gameObject])
                    {
                        if(behaviourType.info.behaviourClass == behaviour)
                        {
                            found = true;

                            MethodInfo method = behaviourType.GetType().GetMethod(message);
                            if (method != null)
                            {
                                methodFound = true;
                                method.Invoke(behaviourType, parameters);
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
    }
}
