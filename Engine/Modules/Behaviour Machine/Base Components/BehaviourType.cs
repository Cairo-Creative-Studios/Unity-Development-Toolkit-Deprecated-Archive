//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Behaviour
{
    /// <summary>
    /// The Behaviour Type class
    /// </summary>
    [Serializable]
    public class BehaviourType
    {
        /// <summary>
        /// Whether the Behaviour is enabled or not
        /// </summary>
        public bool Enabled = true;

        /// <summary>
        /// The Behaviour's Template
        /// </summary>
        public BehaviourTypeTemplate template;

        /// <summary>
        /// The Game Object this Behaviour is Controlling
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// The Transform of the Game Object this Behaviour is Controlling
        /// </summary>
        public Transform transform;

        /// <summary>
        /// The Root Cairo Object of the Behaviour
        /// </summary>
        public Object root;

        public SDictionary<string, float> inputs = new SDictionary<string, float>();
        public SDictionary<string, string> inputMap = new SDictionary<string, string>();

        /// <summary>
        /// Updates the Core of the Behaviour, done once every tick
        /// </summary>
        public void CoreUpdate()
        {
            SetInputs();
        }

        /// <summary>
        /// Get a Component from the Game Object this Behaviour is attached to
        /// </summary>
        /// <returns>The component.</returns>
        /// <param name="componentName">Component name.</param>
        public object GetComponent(string componentName)
        {
            return gameObject.GetComponent(componentName);
        }

        /// <summary>
        /// Gets a List of Components from the Game Object this Behaviour is attached to
        /// </summary>
        /// <returns>The components.</returns>
        /// <param name="componentName">Component name.</param>
        public object[] GetComponents(string componentName)
        {
            return gameObject.GetComponents(Type.GetType(componentName));
        }

        /// <summary>
        /// Adds a Component to the Game Object this Behaviour is attached to
        /// </summary>
        /// <param name="componentName">Component name.</param>
        public void AddComponent(string componentName)
        {
            gameObject.AddComponent(Type.GetType(componentName));
        }

        /// <summary>
        /// Sets the Active state of the Game Object that this Behaviour is attached to
        /// </summary>
        /// <param name="active">If set to <c>true</c> active.</param>
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        /// <summary>
        /// Get the Entity that this Behaviour is attached to
        /// </summary>
        /// <returns>The entity.</returns>
        public Entity GetEntity()
        {
            return gameObject.GetComponent<Entity>();
        }

        /// <summary>
        /// Get the Pawn that this Behaviour is attached to
        /// </summary>
        /// <returns>The pawn.</returns>
        public Pawn GetPawn()
        {
            return gameObject.GetComponent<Pawn>();
        }

        public Vehicle GetVehicle()
        {
            return gameObject.GetComponent<Vehicle>();
        }

        /// <summary>
        /// Enable this instance.
        /// </summary>
        public void Enable()
        {
            Enabled = true;

            //Send a Message to the Behaviour to Enable it
            BehaviourModule.Message(gameObject, "OnEnable", null, template.ID);
        }

        /// <summary>
        /// Disable this instance.
        /// </summary>
        public void Disable()
        {
            Enabled = false;

            //Send a Message to the Behaviour to Disable it
            BehaviourModule.Message(gameObject, "OnDisable", null, template.ID);
        }

        /// <summary>
        /// Gets the Inputs from the parent Entity.
        /// </summary>
        /// <returns>The inputs.</returns>
        private void SetInputs()
        {
            Entity entity = GetEntity();
            Vehicle vehicle = GetVehicle();

            //Get Inputs for Vehicle objects
            if(vehicle != null)
            {
                SDictionary<string, float> vehicleInputs = new SDictionary<string, float>();

                foreach(Seat seat in vehicle.seats.Keys)
                {
                    foreach(string input in seat.inputs.Keys)
                    {
                        vehicleInputs.Add(seat.ID+"_"+input, seat.inputs[input]);
                    }
                }

                foreach(string input in vehicleInputs.Keys)
                {
                    if (inputMap.ContainsKey(input))
                    {
                        if (vehicleInputs[input] != default(float))
                            inputs[inputMap[input]] = vehicleInputs[input];
                        else
                            inputs[inputMap[input]] = 0.0f;
                    }
                    //else
                    //    Debug.LogError("The "+template.ID+" Behaviour does not contain an Input Map for '" + input + "', but the Vehicle object is attempting to use it.");
                }
            }
            //Get Inputs for Entity Objects
            if(entity!=null)
            {
                SDictionary<string, float> entityInputs = GetEntity().inputs;

                foreach (string input in entityInputs.Keys)
                {
                    if(inputMap.ContainsKey(input))
                        if(inputs.ContainsKey(inputMap[input]))
                            inputs[inputMap[input]] = entityInputs[input];
                }
            }
        }

        /// <summary>
        /// Gets the value of the Input
        /// </summary>
        /// <returns>The input.</returns>
        /// <param name="input">Input.</param>
        private float GetInput(string input)
        {
            return inputs[input];

        }
    }
}
