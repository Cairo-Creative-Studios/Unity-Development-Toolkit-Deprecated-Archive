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

        private float direction = 0;
        private Vector2 movementInput = new Vector2();

        public virtual void Init()
        {
            behaviourTemplate = (BehaviourTemplate_CharacterController)template;
            characterController = gameObject.transform.Find(behaviourTemplate.controllerPath).gameObject.GetComponent<CharacterController>();

            if (characterController == null)
                characterController = gameObject.transform.Find(behaviourTemplate.controllerPath).gameObject.AddComponent<CharacterController>();
        }

        private void OnJump()
        {
        }

        public void Update()
        {
            root.velocity = root.velocity.Lerp(Physics.gravity, 0.1f);
            movementInput = movementInput.Lerp(new Vector2(inputs["Horizontal"], inputs["Vertical"]), behaviourTemplate.motionInterpolation);
            if(Mathf.Abs(movementInput.x) > 0.1|| Mathf.Abs(movementInput.y) > 0.1)
                direction = Mathf.LerpAngle(direction, Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y, 0.2f);
            Vector3 moveDir = Quaternion.Euler(0, direction, 0) * Vector3.forward;

            root.velocity.x = moveDir.normalized.x * behaviourTemplate.speed * Mathf.Clamp(Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y), 0, 1);
            root.velocity.z = moveDir.normalized.z * behaviourTemplate.speed * Mathf.Clamp(Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y), 0, 1);

            characterController.SimpleMove(root.velocity);

            if (animator != null)
                UpdateAnimation();
        }

        public void UpdateAnimation()
        {
            animator.SetFloat("HorizontalSpeed", root.velocity.x + root.velocity.z);
            animator.SetBool("isGrounded", characterController.isGrounded);
            animator.SetFloat("VerticalSpeed", root.velocity.y);

            root.transform.eulerAngles = new Vector3(0,direction+180,0);
        }

        public class Default : State<CharacterControllerBehaviour>
        {
            public SDictionary<string, float> inputs = new SDictionary<string, float>();

            public virtual void Update()
            {
                UpdateInput();
                UpdatePhysics();
                UpdateAnimator();

                inputs = parent.inputs;
                Debug.Log("Not Falling");
            }

            public virtual void UpdateInput()
            {
                parent.movementInput = parent.movementInput.Lerp(new Vector2(inputs["Horizontal"], inputs["Vertical"]), parent.behaviourTemplate.motionInterpolation);

                if (inputs["Jump"] > 0)
                {
                    parent.OnJump();
                }
            }

            public virtual void UpdatePhysics()
            {
                parent.root.velocity = parent.root.velocity.Lerp(Physics.gravity, 0.1f);
                parent.characterController.Move(parent.root.velocity);
            }

            public virtual void UpdateAnimator()
            {
                if (parent.animator != null)
                {
                    parent.animator.SetFloat("VectorX", parent.movementInput.x);
                    parent.animator.SetFloat("VectorY", parent.movementInput.y);
                    parent.animator.SetBool("Jumping", StateMachineModule.GetState(parent.gameObject) == "Jump");
                    parent.animator.SetBool("Falling", StateMachineModule.GetState(parent.gameObject) == "Fall");
                }
            }
        }

    }
}
