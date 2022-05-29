using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class Controller : MonoBehaviour
    {
        /// <summary>
        /// The possessed pawn, as an Object to allow for simple custom Pawns with Casting.
        /// </summary>
        public object possessedPawn;
        public List<object> possessedEntities;
        /// <summary>
        /// The Controller's Inputs and their current State.
        /// </summary>
        public Dictionary<string, float> inputs = new Dictionary<string, float>();
    }
}
