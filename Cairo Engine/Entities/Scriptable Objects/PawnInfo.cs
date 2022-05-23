
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "CairoGame/Entities/Pawn")]
    /// <summary>
    /// The Data for a Pawn
    /// </summary>
    public class PawnInfo : EntityInfo
    {
        /// <summary>
        /// Whether to use a Character Controller to move the Pawn. 
        /// The Character Controller is used for basic movement for most Characters in games.
        /// </summary>
        public bool useCharacterController = true;

    }
}

