using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// The State Machine Manager allows you to add a State Machine to any Game Object. You can call custom State based Methods using the CallStateMethod function.
    /// </summary>
    public class StateMachineModule
    {
        /// <summary>
        /// The fsm enabled Game Objects.
        /// </summary>
        private static Dictionary<GameObject, List<StateContainer>> fsmObjects = new Dictionary<GameObject, List<StateContainer>>();

        public static void Init()
        {

        }

        /// <summary>
        /// Updates the objects in the state machine.
        /// </summary>
        public static void Update()
        {
            foreach (GameObject currentObject in fsmObjects.Keys)
            {
                CallStateMethod(currentObject, "Update");
            }
        }


        /// <summary>
        /// Enables the state machine for the given Monobehaviour.
        /// </summary>
        /// <param name="thisGameObject">The Game Object to apply the State Machine to.</param>
        /// <param name="behaviour">The Monobehaviour to add the State Machine to.</param>
        public static void EnableStateMachine(GameObject thisGameObject, object behaviour)
        {
            MonoBehaviour behaviourAsMono = (MonoBehaviour)behaviour;

            Type[] stateTypesInBehaviour = Engine.GetNestedTypesOfBase(behaviour.GetType(), "State");
            Dictionary<string, State> states = new Dictionary<string, State>();
            string firstTypeName = "";

            foreach (Type type in stateTypesInBehaviour)
            {
                states.Add(type.Name, (State)Activator.CreateInstance(type));

                if (firstTypeName == "")
                    firstTypeName = type.Name;
            }

            if (!fsmObjects.ContainsKey(thisGameObject))
                fsmObjects.Add(thisGameObject, new List<StateContainer>());

            fsmObjects[thisGameObject].Add(new StateContainer(behaviour.GetType().Name, states, firstTypeName));

            CallStateMethod(thisGameObject, "Enter");
        }

        /// <summary>
        /// Sets the state of a Finite State enabled Object, and Optionally specify the Behavior to allow Behaviours to have unique States.
        /// </summary>
        /// <param name="stateGameObject">Game object.</param>
        /// <param name="state">State.</param>
        /// <param name="behaviourName">Behaviour name.</param>
        public static void SetState(GameObject stateGameObject, string state, string behaviourName = "")
        {
            if (fsmObjects.ContainsKey(stateGameObject))
            {
                if (behaviourName == "")
                {
                    foreach (StateContainer stateContainer in fsmObjects[stateGameObject])
                    {
                        if (stateContainer.curState != "")
                        {
                            CallStateMethod(stateGameObject, "Exit");
                        }

                        stateContainer.prevState = stateContainer.curState;
                        stateContainer.curState = state;

                        CallStateMethod(stateGameObject, "Enter");
                    }
                }

            }
        }

        /// <summary>
        /// Uses Reflection to call methods inside of FSM Enabled objects
        /// </summary>
        /// <param name="stateGameObject">State game object.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="parameters">Parameters.</param>
        public static void CallStateMethod(GameObject stateGameObject, string methodName, object[] parameters = null)
        {
            foreach (StateContainer stateContainers in fsmObjects[stateGameObject])
            {
                Type behaviourType = Type.GetType(stateContainers.behaviourName);

                foreach (string stateName in stateContainers.states.Keys)
                {
                    if (stateName == stateContainers.curState)
                    {
                        MethodInfo method = behaviourType.GetNestedType(stateName).GetMethod(methodName);

                        if (method != null)
                            method.Invoke(stateContainers.states[stateName], parameters);

                    }
                }
            }
        }
    }
}
