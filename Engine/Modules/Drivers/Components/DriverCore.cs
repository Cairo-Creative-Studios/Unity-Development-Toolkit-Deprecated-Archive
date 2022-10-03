//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Reflection;
using NaughtyAttributes;

namespace CairoEngine.Drivers
{
    /// <summary>
    /// Adds additional functionality to the Object that allows Drivers to communicate between each other easier.
    /// </summary>
    public class DriverCore : MonoBehaviour
    {
        /// <summary>
        /// The Driver Core's Tags, for Identification
        /// </summary>
        [Tooltip("The Driver Core's Tags, for Identification")]
        [Foldout("Properties")]
		public List<string> tags = new List<string>();
        /// <summary>
        /// The Template to use for the Driver Core
        /// </summary>
        [Tooltip("The Template to use for the Driver Core")]
        [Foldout("Properties")]
        public DriverCoreTemplate template;
        /// <summary>
        /// A List of Drivers attached to the Game Object
        /// </summary>
        [Tooltip("A List of Drivers attached to the Game Object")]
        [Foldout("Properties")]
        public SDictionary<string, List<object>> states = new SDictionary<string, List<object>>();
        /// <summary>
        /// The State that the Driver Core is Currently in. Only the Drivers within this State will be Enabled
        /// </summary>
        [Tooltip("The State that the Driver Core is Currently in. Only the Drivers within this State will be Enabled")]
        [Foldout("Properties")]
        [Dropdown("GetStateList")]
        public string currentState = "";

        /// <summary>
        /// The Moving Velocity of the Object
        /// </summary>
        [Tooltip("The Moving Velocity of the Object")]
        [Foldout("Scope")]
        public Vector3 velocity = new Vector3();
        /// <summary>
        /// Properties of the Driver Core, acts as a List of Variables in the Game Object's Scope
        /// </summary>
        [Tooltip("Properties of the Driver Core, acts as a List of Variables in the Game Object's Scope")]
        [Foldout("Scope")]
        public SDictionary<string, object> properties = new SDictionary<string, object>();
        /// <summary>
        /// The Inputs Recieved from a Controller, to be Translated within the Core's Drivers
        /// </summary>
        [Tooltip("The Inputs Recieved from a Controller, to be Translated within the Core's Drivers")]
        [Foldout("Scope")]
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
        /// Get a Driver by it's Type and ID.
        /// </summary>
        /// <returns>The driver.</returns>
        /// <param name="ID">The ID of the Driver.</param>
        /// <typeparam name="T">The Type of the Driver</typeparam>
        public object GetDriver<T>(string ID)
        {
            foreach(string key in states.Keys)
            {
                foreach(object driver in states[key])
                {
                    if(driver.GetType().Name == typeof(T).Name&&((DriverTemplate)driver.GetField("template")).ID == ID)
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
            foreach(object driver in states[currentState])
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
                characterController.Move(speed * Time.deltaTime);
            }
            //Otherwise, Update the Transform
            else
            {
                transform.position += speed * Time.deltaTime;
            }
        }
    }
}
