//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections.Generic;
using UnityEngine;
using B83.Unity.Attributes;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Entities/Vehicle")]
    /// <summary>
    /// The Data for a Pawn
    /// </summary>
    public class VehicleTemplate : ObjectTemplate
    {
        /// <summary>
        /// The Class to use for the Vehicle
        /// </summary>
        [MonoScript(type = typeof(Vehicle))] public string Class = "CairoEngine.Vehicle";
        /// <summary>
        /// The amount of Health the Entity should start with
        /// </summary>
        public float health = 100.0f;
        /// <summary>
        /// The Seats of the Vehicle
        /// </summary>
        public List<Seat> seats = new List<Seat>();
    }
}

