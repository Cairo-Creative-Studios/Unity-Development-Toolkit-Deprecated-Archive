//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Reflection;
using CairoEngine.Controllers;

namespace CairoEngine.Drivers
{
    /// <summary>
    /// Adds additional functionality to the Object that allows Drivers to communicate between each other easier.
    /// </summary>
    public class DriverCore : MonoBehaviour
    {
		public List<string> tags = new List<string>();
        /// <summary>
        /// The Template to use for the Driver Core
        /// </summary>
        [Tooltip("The Template to use for the Driver Core")]
        public DriverCoreTemplate template;

        [Tooltip("When modified, will subscribe the Core to the Controller with the Given Template when the Core is created")]
        public ControllerTemplate possessingControllerTemplate = null;

        /// <summary>
        /// A List of Drivers attached to the Game Object
        /// </summary>
        [Tooltip("A List of Drivers attached to the Game Object")]
        public SDictionary<string, List<object>> states = new SDictionary<string, List<object>>();

        public string currentState = "";
        /// <summary>
        /// The Moving Velocity of the Object
        /// </summary>
        [Tooltip("The Moving Velocity of the Object")]
        public Vector3 velocity = new Vector3();
        /// <summary>
        /// Properties of the Driver Core, acts as a List of Variables in the Game Object's Scope
        /// </summary>
        [Tooltip("Properties of the Driver Core, acts as a List of Variables in the Game Object's Scope")]
        public SDictionary<string, object> properties = new SDictionary<string, object>();

		public SDictionary<string, float> inputs = new SDictionary<string, float>();

        UnityEngine.CharacterController characterController;

        void Start()
		{
            List<string> keyList = new List<string>();
            keyList.AddRange(template.states.Keys);
            currentState = keyList[0];

            foreach(String state in template.states.Keys)
            {
                //Add all the default Behaviours of the Driver Core
                foreach (DriverTemplate driverTemplate in template.states[state])
                {
                    DriverModule.AddDriver(gameObject, state, driverTemplate, this);
                }
            }
            characterController = GetComponent<UnityEngine.CharacterController>();
        }

		void Update()
		{
            //Update the Drivers within each State
            foreach(string key in states.Keys)
            {
                if(key == currentState)
                {
                    foreach(object driver in states[key])
                    {
                        ((MonoBehaviour)driver).enabled = true;
                        driver.CallMethod("SetInputs", new object[] { inputs });
                    }
                }
                else
                {
                    foreach (object driver in states[key])
                    {
                        ((MonoBehaviour)driver).enabled = false;
                    }
                }
            }
		}

		/// <summary>
		/// Sends a Message to a Behaviour
		/// </summary>
		/// <typeparam name="T">The Type of the Behavour to Message</typeparam>
		/// <returns>The message to send to the Behaviour</returns>
		public object Message<T>(string message, object[] parameters)
        {
            foreach (object driver in states[currentState])
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
			foreach(object driver in states[currentState])
			{
				((MonoBehaviour)driver).enabled = true;
			}
		}

		/// <summary>
		/// Disables all the Drivers in the Core
		/// </summary>
		public void Disable()
		{
			foreach (object driver in states[currentState])
			{
				((MonoBehaviour)driver).enabled = false;
			}
		}

		public object SetProperty(string name, object value)
		{
			if (!properties.ContainsKey(name))
				properties.Add(name, value);
			else
				properties[name] = value;
			return value;
		}

		public object GetProperty(string name)
		{
			return properties[name];
		}

        public void SetState(string state)
        {
            if (states.ContainsKey(state))
                currentState = state;
        }

        void OnEnable()
        {

        }

        /// <summary>
        /// Moves the Core by the given speed
        /// </summary>
        /// <param name="speed">Speed.</param>
        public void Move(Vector3 speed)
        {
            //Use the Character Controller for movement if one is active on the Object
            if (characterController != null)
            {
                characterController.Move(speed);
            }
            //Otherwise, Update the Transform
            else
            {
                transform.position += speed;
            }
        }
    }
}
