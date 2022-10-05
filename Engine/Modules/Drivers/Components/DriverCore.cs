//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using UDT.Reflection;
using NaughtyAttributes;

namespace UDT.Drivers
{
    /// <summary>
    /// Adds additional functionality to the Object that allows Drivers to communicate between each other easier.
    /// </summary>
    public class DriverCore : MonoBehaviour
    {
        public enum MoveMethod
        {
            Auto,
            Transform,
            CharacterController,
            NavMeshAgent
        }

        /// <summary>
        /// Container of Properties for the Driver Core
        /// </summary>
        [Serializable]
        public class PropertyInfo
        {
            /// <summary>
            /// The Driver Core's Tags, for Identification
            /// </summary>
            [Tooltip("The Driver Core's Tags, for Identification")]
            [HorizontalLine]
            public List<string> tags = new List<string>();
            /// <summary>
            /// The State that the Driver Core is Currently in. Only the Drivers within this State will be Enabled
            /// </summary>
            [Tooltip("The State that the Driver Core is Currently in. Only the Drivers within this State will be Enabled")]
            [HorizontalLine]
            [Dropdown("GetStateList")]
            public string currentState = "";
            /// <summary>
            /// The Template to use for the Driver Core
            /// </summary>
            [Tooltip("The Template to use for the Driver Core")]
            [Expandable]
            public DriverCoreTemplate template;
            /// <summary>
            /// A List of Drivers attached to the Game Object
            /// </summary>
            [Tooltip("A List of Drivers attached to the Game Object")]
            [HorizontalLine]
            [HideInInspector] public SDictionary<string, List<object>> states = new SDictionary<string, List<object>>();

            public DropdownList<string> GetStateList()
            {
                DropdownList<string> stateList = new DropdownList<string>();

                if (template != null)
                {
                    if (template.states.Count > 0)
                    {
                        foreach (string state in template.states.Keys)
                        {
                            stateList.Add(state, state);
                        }
                    }
                    else
                        stateList.Add("Empty", "");
                }
                else
                {
                    stateList.Add("Empty", "Empty");
                }

                return stateList;
            }
        }
        /// <summary>
        /// Container of Scope Properties for the Driver Core
        /// </summary>
        [Serializable]
        public class ScopeInfo
        {
            /// <summary>
            /// The Moving Velocity of the Object
            /// </summary>
            [Tooltip("The Moving Velocity of the Object")]
            [HorizontalLine]
            public Vector3 velocity = new Vector3();
            /// <summary>
            /// Properties of the Driver Core, acts as a List of Variables in the Game Object's Scope
            /// </summary>
            [Tooltip("Properties of the Driver Core, acts as a List of Variables in the Game Object's Scope")]
            [HorizontalLine]
            public SDictionary<string, object> properties = new SDictionary<string, object>();
            /// <summary>
            /// The Inputs Recieved from a Controller, to be Translated within the Core's Drivers
            /// </summary>
            [Tooltip("The Inputs Recieved from a Controller, to be Translated within the Core's Drivers")]
            [HorizontalLine]
            public SDictionary<string, float> inputs = new SDictionary<string, float>();
        }
        [Header("The Driver Core is the Container of Drivers, their functionality and properties")]
        /// <summary>
        /// The Properties of the Driver Core
        /// </summary>
        [Tooltip("The Properties of the Driver Core")]
        [HorizontalLine(color: EColor.White)]
        public PropertyInfo properties = new PropertyInfo();
        /// <summary>
        /// The Scope Properties of the Driver Core
        /// </summary>
        [Tooltip("The Scope Properties of the Driver Core")]
        [HorizontalLine(color: EColor.White)]
        public ScopeInfo scope = new ScopeInfo();


        [HideInInspector] public UnityEngine.CharacterController characterController;
        [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;

        void Start()
        {
            List<string> keyList = new List<string>();
            keyList.AddRange(properties.template.states.Keys);
            properties.currentState = keyList[0];

            foreach (String state in properties.template.states.Keys)
            {
                //Add all the default Behaviours of the Driver Core
                foreach (ExpandableDriverTemplate driverTemplate in properties.template.states[state].drivers)
                {
                    DriverModule.AddDriver(gameObject, state, driverTemplate.template, this);
                }
            }
            characterController = GetComponent<UnityEngine.CharacterController>();
        }

        void Update()
        {
            //Update the Drivers within each State
            foreach (string key in properties.states.Keys)
            {
                if (key == properties.currentState)
                {
                    foreach (object driver in properties.states[key])
                    {
                        ((MonoBehaviour)driver).enabled = true;
                        driver.CallMethod("SetInputs", new object[] { scope.inputs });
                    }
                }
                else
                {
                    foreach (object driver in properties.states[key])
                    {
                        ((MonoBehaviour)driver).enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Get a Driver by it's Type and ID.
        /// </summary>
        /// <returns>The driver.</returns>
        /// <param name="ID">The ID of the Driver.</param>
        /// <typeparam name="T">The Type of the Driver</typeparam>
        public object GetDriver<T>(string ID)
        {
            foreach (string key in properties.states.Keys)
            {
                foreach (object driver in properties.states[key])
                {
                    if (driver.GetType().Name == typeof(T).Name && ((DriverTemplate)driver.GetField("template")).driverProperties.main.ID == ID)
                    {
                        return driver;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get's a Driver from the active State, given it's Type
        /// </summary>
        /// <returns>The driver.</returns>
        /// <typeparam name="T">The Type of the Driver to get</typeparam>
        public object GetDriver<T>()
        {
            foreach (object driver in properties.states[properties.currentState])
            {
                if (driver.GetType().Name == typeof(T).Name)
                    return driver;
            }
            return null;
        }

        /// <summary>
        /// Sends a Message to a Behaviour
        /// </summary>
        /// <typeparam name="T">The Type of the Behavour to Message</typeparam>
        /// <returns>The message to send to the Behaviour</returns>
        public object Message<T>(string message, object[] parameters)
        {
            foreach (object driver in properties.states[properties.currentState])
            {
                if (driver.GetType() == typeof(T))
                    return driver.CallMethod(message, parameters);
            }

            return null;
        }

        /// <summary>
        /// Enables all the Drivers in Core
        /// </summary>
        public void Enable()
        {
            foreach (object driver in properties.states[properties.currentState])
            {
                ((MonoBehaviour)driver).enabled = true;
            }
        }

        /// <summary>
        /// Disables all the Drivers in the Core
        /// </summary>
        public void Disable()
        {
            foreach (object driver in properties.states[properties.currentState])
            {
                ((MonoBehaviour)driver).enabled = false;
            }
        }

        /// <summary>
        /// Sets the Value of the Property with the given Name
        /// </summary>
        /// <returns>The property.</returns>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public object SetProperty(string name, object value)
        {
            if (!scope.properties.ContainsKey(name))
                scope.properties.Add(name, value);
            else
                scope.properties[name] = value;
            return value;
        }

        /// <summary>
        /// Gets the Value of the Property with the Given Name
        /// </summary>
        /// <returns>The property.</returns>
        /// <param name="name">Name.</param>
        public object GetProperty(string name)
        {
            return scope.properties[name];
        }

        /// <summary>
        /// Ensures that the Component Property is Valid, and Returns it.
        /// </summary>
        /// <returns>The component property.</returns>
        /// <param name="name">Name.</param>
        /// <param name="path">Path.</param>
        /// <param name="component">Component.</param>
        public object ValidateComponentProperty(string name, string path, Type component)
        {
            //Tries to get the Character Controller attached to the Object, otherwise Creates one
            if (GetProperty(name) == null)
            {
                SetProperty(name, gameObject.transform.Find(path).gameObject.GetComponent(component));

                if (GetProperty(name) == null)
                    SetProperty(name, gameObject.transform.Find(path).gameObject.AddComponent(component));
            }
            return GetProperty(name);
        }

        public void SetState(string state)
        {
            if (properties.states.ContainsKey(state))
                properties.currentState = state;
        }

        void OnEnable()
        {

        }

        /// <summary>
        /// Moves the Core by the given speed
        /// </summary>
        /// <param name="speed">Speed.</param>
        public void Move(Vector3 speed, MoveMethod moveMethod = MoveMethod.Auto)
        {
            switch (moveMethod)
            {
                case MoveMethod.Auto:
                    //Use the Character Controller for movement if one is active on the Object
                    if (characterController != null)
                    {
                        characterController.Move(speed * Time.deltaTime);
                    }
                    //Otherwise, Update the Transform
                    else
                    {
                        transform.position += speed * Time.deltaTime;
                    }
                    break;
                case MoveMethod.CharacterController:
                    characterController.Move(speed * Time.deltaTime);
                    break;
                case MoveMethod.NavMeshAgent:
                    navMeshAgent.Move(speed * Time.deltaTime);
                    break;
            }
        }
    }
}
