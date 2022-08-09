//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Behaviour
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Car Controller", fileName = "[BEHAVIOUR] Car Controller")]
    public class BehaviourTemplate_CarController : BehaviourTemplate
    {
        [Header("")]
        [Header(" -- Car Controller -- ")]
        /// <summary>
        /// The Seats of the Vehicle
        /// </summary>
        [Tooltip("The Seats of the Vehicle")]
        public List<Seat> seats = new List<Seat>();

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Behaviour.CarController";

            SetInputMap(new string[] { "Steer", "Accel", "Brake"}, new string[] { "Driver_MoveHorizontal", "Driver_LeftZAxis", "Driver_RightZAxis"});

            foreach (string defaultEvent in "Accelerate, Brake, Stopped, Hit".TokenArray())
            {
                if (!scriptContainer.Output.ContainsKey(defaultEvent))
                    scriptContainer.Output.Add(defaultEvent, null);
            }
        }
    }
}
