//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Behaviour
{
    [Serializable]
    public class CharacterControllerBehaviour : BehaviourType
    {
        private BehaviourTemplate_CharacterController behaviourTemplate;
        public CharacterController characterController;
        public Animator characterAnimator;

        private float direction = 0;
        private Vector2 movementInput = new Vector2();
        private Vector2 lookInput = new Vector2();

        public void Init()
        {
            behaviourTemplate = (BehaviourTemplate_CharacterController)template;
            characterController = gameObject.GetComponent<CharacterController>();

            if (characterController == null)
                characterController = gameObject.AddComponent<CharacterController>();

            characterAnimator = gameObject.GetComponent<Animator>();
        }

        public void Update()
        {
            UpdateInput();
            UpdatePhysics();
            UpdateAnimation();
        }

        private void UpdateInput()
        {
            movementInput = movementInput.Lerp(new Vector2(inputs["Horizontal"], inputs["Vertical"]), behaviourTemplate.movementInterpolation);
            lookInput = new Vector2(inputs["LookHorizontal"], inputs["LookVertical"]);

            if (inputs["Jump"] > 0)
            {
                OnJump();
            }
        }

        private void UpdatePhysics()
        {
            root.velocity = root.velocity.Lerp(Physics.gravity, 0.1f);
            characterController.Move(root.velocity);

            if (characterController.isGrounded)
            {
                if (root.velocity.y < 0)
                {
                    StateMachineModule.SetState(gameObject, "Ground");
                }
            }
            else
            {
                if (root.velocity.y < 0)
                {
                    if (StateMachineModule.GetState(gameObject) != "Jump")
                        StateMachineModule.SetState(gameObject, "Fall");
                }
            }
        }

        private void UpdateAnimation()
        {
            if(characterAnimator != null){

                characterAnimator.SetFloat("VectorX", movementInput.x);
                characterAnimator.SetFloat("VectorY", movementInput.y);
                characterAnimator.SetBool("Jumping", StateMachineModule.GetState(gameObject) == "Jump");
                characterAnimator.SetBool("Falling", StateMachineModule.GetState(gameObject) == "Fall");
            }
        }

        private void OnJump()
        {
            if (StateMachineModule.GetState(gameObject) == "Ground")
                StateMachineModule.SetState(gameObject, "Jump");
        }

        public class Ground : State<CharacterControllerBehaviour>
        {
            public void Update()
            {
                UpdatePhysics();
                UpdateAnimator();
            }

            private void UpdatePhysics()
            {
                parent.root.velocity.y = 0;
            }

            private void UpdateAnimator()
            {
                if(parent.characterAnimator != null)
                    parent.transform.eulerAngles = parent.transform.eulerAngles.Lerp(new Vector3(0, parent.direction, 0), 0.1f);
            }
        }

        public class Jump : State<CharacterControllerBehaviour>
        {
        }

        public class Fall : State<CharacterControllerBehaviour>
        {
            public void Update()
            {
                UpdatePhysics();
            }

            private void UpdatePhysics()
            {
                parent.root.velocity = parent.root.velocity.Lerp(Physics.gravity, 0.1f);
            }
        }

        public class Attack : State<CharacterControllerBehaviour>
        {

        }
    }
}
