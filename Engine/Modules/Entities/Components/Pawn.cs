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


        // Start is called before the first frame update
        void Start()
        {
            //characterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            //characterController.SimpleMove(velocity);
        }
    }
}
