//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class Vehicle : MonoBehaviour
    {
        public VehicleTemplate template;
        public Vector3 velocity = new Vector3();
        public SDictionary<Seat, Entity> seats = new SDictionary<Seat, Entity>();

        /// <summary>
        /// Enter the specified pawn into the Vehicle
        /// </summary>
        /// <param name="entity">entity.</param>
        public void Enter(Entity entity, string seatName)
        {
            Seat seat = GetSeat(seatName);

            if(seat != null)
            {
                entity.gameObject.GetComponent<Object>().Disable();
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
        public void Exit(Entity entity)
        {
            Seat seat = GetEntitysSeat(entity);

            if (seat != null)
            {
                entity.gameObject.GetComponent<Object>().Enable();
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
            foreach(Seat seat in seats.Keys)
            {
                if(seat.ID == ID)
                {
                    return seat;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the Seat that an Entity is sitting in
        /// </summary>
        /// <returns>The entitys seat.</returns>
        /// <param name="entity">Entity.</param>
        private Seat GetEntitysSeat(Entity entity)
        {
            foreach(Seat seat in seats)
            {
                if(seats[seat] == entity)
                {
                    return seat;
                }
            }
            return null;
        }
    }
}
