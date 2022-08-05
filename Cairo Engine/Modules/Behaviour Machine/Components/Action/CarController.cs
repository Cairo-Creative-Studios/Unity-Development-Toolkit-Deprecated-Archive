//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;
using RVP;
//using RVP;

namespace CairoEngine.Behaviour
{
    [Serializable]
    public class CarController : CairoBehaviour<BehaviourTemplate_CarController>
    {
        public VehicleParent vehicle;

        private float direction = 0;
        private Vector2 movementInput = new Vector2();

        public SDictionary<Seat, CObject> seats = new SDictionary<Seat, CObject>();

        public void Init()
        {
            vehicle = gameObject.GetComponent<VehicleParent>();

            if (!Engine.flags.Contains("CarSimStarted")&& Engine.singleton.enginePrefabs.ContainsKey("VehicleTimeMaster") && Engine.singleton.enginePrefabs.ContainsKey("VehicleGlobalControl"))
            {
                GameObject.Instantiate(Engine.singleton.enginePrefabs["VehicleGlobalControl"]);
                GameObject.Instantiate(Engine.singleton.enginePrefabs["VehicleTimeMaster"]);
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
        /// Enter the specified pawn into the Vehicle
        /// </summary>
        /// <param name="entity">entity.</param>
        public void Enter(CObject entity, string seatName)
        {
            Seat seat = GetSeat(seatName);

            if (seat != null)
            {
                entity.gameObject.GetComponent<CObject>().Disable();
                seats[seat] = entity;

            }
            else
            {
                Debug.Log("Seat " + seatName + " was not found.");
            }
        }

        /// <summary>
        /// Exit the specified entity from the Vehicle
        /// </summary>
        /// <param name="entity">Entity.</param>
        public void Exit(CObject entity)
        {
            Seat seat = GetEntitysSeat(entity);

            if (seat != null)
            {
                entity.gameObject.GetComponent<CObject>().Enable();
                seats[seat] = null;
            }
        }

        /// <summary>
        /// Gets the Seat with the given Name
        /// </summary>
        /// <returns>The seat.</returns>
        /// <param name="ID">Name.</param>
        private Seat GetSeat(string ID)
        {
            foreach (Seat seat in seats.Keys)
            {
                if (seat.ID == ID)
                {
                    return seat;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the Seat that a Character is sitting in
        /// </summary>
        /// <returns>The entitys seat.</returns>
        /// <param name="entity">Entity.</param>
        private Seat GetEntitysSeat(CObject entity)
        {
            foreach (Seat seat in seats.Keys)
            {
                if (seats[seat] == entity)
                {
                    return seat;
                }
            }
            return null;
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

        public void Update()
        {
            vehicle.SetAccel(inputs["Accel"]);
            vehicle.SetBrake(inputs["Brake"]);
            vehicle.SetSteer(inputs["Steer"]);
        }
    }
}
