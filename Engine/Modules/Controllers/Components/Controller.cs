using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class Controller : MonoBehaviour
    {
        /// <summary>
        /// The Controller's Template
        /// </summary>
        public ControllerTemplate template;

        /// <summary>
        /// The possessed pawn, as an Object to allow for simple custom Pawns with Casting.
        /// </summary>
        public object possessedPawn;
        public List<object> possessedEntities;

        /// <summary>
        /// The Controller's Inputs and their current State.
        /// </summary>
        public SerializableDictionary<string, float> inputs = new SerializableDictionary<string, float>();

        /// <summary>
        /// Whether the Controller has already been Checked in or not
        /// </summary>
        public bool checkedIn = false;

        void Start()
        {
            if (!checkedIn)
                ControllerModule.CheckIn(this);
        }
    }
}
