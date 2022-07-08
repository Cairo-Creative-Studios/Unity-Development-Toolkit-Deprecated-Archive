//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class Pawn : Entity
    {
        public PawnTemplate pawnTemplate;
        /// <summary>
        /// The Character Controller that controls movement of this Pawn.
        /// </summary>
        public CharacterController characterController;
        public Vector3 velocity = new Vector3();
    }
}
