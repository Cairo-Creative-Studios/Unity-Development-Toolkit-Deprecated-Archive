//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;
using CairoEngine.Behaviour.CarController;
using RVP;
//using RVP;

namespace CairoEngine.Behaviour
{
    [Serializable]
    public class CarControllerBehaviour : BehaviourType<BehaviourTemplate_CarController>
    {
        public VehicleParent vehicle;

        private float direction = 0;
        private Vector2 movementInput = new Vector2();

        private Dictionary<int, List<Tire>> tires = new Dictionary<int,List<Tire>>();

        public void Init()
        {
            vehicle = gameObject.GetComponent<VehicleParent>();

            if (!Engine.flags.Contains("CarSimStarted")&& Engine.singleton.enginePrefabs.ContainsKey("VehicleTimeMaster") && Engine.singleton.enginePrefabs.ContainsKey("VehicleGlobalControl"))
            {
                Object.Instantiate(Engine.singleton.enginePrefabs["VehicleGlobalControl"]);
                Object.Instantiate(Engine.singleton.enginePrefabs["VehicleTimeMaster"]);
                Engine.flags.Add("CarSimStarted");
            }
            else
            {
                Debug.LogWarning("The Vehicle Simulation can not be Started");

                if (!Engine.singleton.enginePrefabs.ContainsKey("VehicleTimeMaster"))
                    Debug.LogWarning("The Time Master Prefab value in Engine has lost it's reference, the Vehicle Simulation can not run without it.");

                if (!Engine.singleton.enginePrefabs.ContainsKey("VehicleGlobalControl"))
                    Debug.LogWarning("The Global Control Prefab value in Engine has lost it's reference, the Vehicle Simulation can not run without it.");
            }
        }

        /// <summary> 
        /// The the Character by the specified amount.
        /// </summary>
        /// <param name="amount">Amount.</param>
        public void Turn(float amount)
        {
            direction = direction + amount;
        }

        public void SetDirection(float direction)
        {
            this.direction = direction;
        }

        /// <summary>
        /// Sets the input.
        /// </summary>
        /// <param name="input">Input.</param>
        public void SetInput(Vector2 input)
        {
            movementInput = input;
        }

        /// <summary>
        /// Adds the tire.
        /// </summary>
        /// <param name="tire">Tire.</param>
        public void AddTire(Tire tire)
        {
            if (!tires.ContainsKey(tire.axle))
                tires.Add(tire.axle, new List<Tire>());

            tires[tire.axle].Add(tire);
        }

        public void Update()
        {
            vehicle.SetAccel(inputs["Accel"]);
            vehicle.SetBrake(inputs["Brake"]);
            vehicle.SetSteer(inputs["Steer"]);
        }
    }
}
