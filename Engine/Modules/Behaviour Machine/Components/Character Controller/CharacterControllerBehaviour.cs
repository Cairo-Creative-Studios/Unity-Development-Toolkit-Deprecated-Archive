using System;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Behaviour
{
    public class CharacterControllerBehaviour : BehaviourType
    {
        public CharacterController characterController;

        private float direction = 0;
        private Vector2 movementInput = new Vector2();
        private Vector3 velocity = new Vector3();

        public override void Init()
        {
            characterController = gameObject.AddComponent<CharacterController>();
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

        public override void BehaviourUpdate()
        {
            
        }
    }
}
