//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

namespace CairoEngine
{
    [Serializable]
    public class Seat
    {
        /// <summary>
        /// The Name of the Seat
        /// </summary>
        public string ID = "Driver";
        /// <summary>
        /// The Transform of the Seat
        /// </summary>
        public Transform root;
        /// <summary>
        /// The Priority of the Seat, decides which seat to give to an Entity
        /// </summary>
        public float priority = 1.0f;
        /// <summary>
        /// Whether the Seat can be stolen by an Entity
        /// </summary>
        public bool canSteal = true;
        /// <summary>
        /// Whether to block Pawns from the same Team from Entering this Seat if a Team Member is already sitting there
        /// </summary>
        public bool blockSameTeam = true;
        /// <summary>
        /// The Inputs recieved from the Controller of the Entity that is sitting here
        /// </summary>
        public SDictionary<string, float> inputs = new SDictionary<string, float>();
    }
}
