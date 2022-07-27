//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;
using CairoEngine.StateMachine;

namespace CairoEngine.Behaviour
{
    [Serializable]
    public class CharacterControllerBehaviour : BehaviourType<BehaviourTemplate_CharacterController>
    {
        #region Global
        private CharacterController characterController;

        private float direction = 0;
        private Vector3 movementInput = new Vector3();
        private bool onGround = false;

        public virtual void Init()
        {

            //Tries to get the Character Controller attached to the Object, otherwise Creates one
            if(!root.properties.ContainsKey("characterController"))
            {
                root.properties.Add("characterController",gameObject.transform.Find(template.controllerPath).gameObject.GetComponent<CharacterController>());

                if (root.properties["characterController"] == null)
                    root.properties["characterController"] = gameObject.transform.Find(template.controllerPath).gameObject.AddComponent<CharacterController>();
            }
            //Get a local reference
            characterController = (CharacterController)root.properties["characterController"];

            SMModule.AddComponent(gameObject,SMModule.CreateStateMachine(this));
        }

        public void Update()
        {
            RaycastHit hit;

            if(Physics.Raycast(characterController.transform.position, Vector3.down, out hit, characterController.height))
            {
                onGround = true;
            }
            else
            {
                onGround = false;
            }

            animator.SetFloat("HorizontalSpeed", Mathf.Abs(root.velocity.x) + Mathf.Abs(root.velocity.z));
            animator.SetBool("isGrounded", onGround);
            animator.SetFloat("VerticalSpeed", root.velocity.y);
        }

        /// <summary>
        /// Move the Character controller
        /// </summary>
        public virtual void Move()
        {
            ((CharacterController)root.properties["characterController"]).Move(root.velocity * Time.deltaTime);
        }
        #endregion

        public class Grounded : State<CharacterControllerBehaviour>
        {
            float currentCayoteTime = 0;

            public void Enter()
            {
                currentCayoteTime = 0;
            }

            public virtual void Update()
            {
                //Other Updates
                UpdateStates();
                UpdatePhysics();
                UpdateAnimator();
            }

            public virtual void UpdateStates()
            {
                if (root.inputs["Jump"] > 0)
                {
                    SetState("Jumping");
                }

                if (root.onGround)
                {
                    currentCayoteTime = 0;
                }
                else
                {
                    currentCayoteTime += Time.deltaTime;

                    if (currentCayoteTime > root.template.cayoteTime*60)
                    {
                        SetState("Falling");
                    }
                }
            }

            public virtual void UpdatePhysics()
            {
                //Interpolate Movement Input toward the current Controller Inputs
                root.movementInput = root.movementInput.Lerp(new Vector3(root.inputs["Horizontal"], root.movementInput.y, root.inputs["Vertical"]), root.template.motionInterpolation);

                //Interpolate the Angle of the Root Direction toward the current Moving Direction
                if (Mathf.Abs(root.movementInput.x) > 0.1 || Mathf.Abs(root.movementInput.z) > 0.1)
                    root.direction = Mathf.LerpAngle(root.direction, Mathf.Atan2(root.movementInput.x, root.movementInput.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y, 0.2f);
                    
                //Get the Move Direction
                Vector3 moveDir = Quaternion.Euler(0, root.direction, 0) * Vector3.forward;

                //Add Movement Input to the Object's Velocity
                root.root.velocity.x = moveDir.normalized.x * root.template.speed * Mathf.Clamp(Mathf.Abs(root.movementInput.x) + Mathf.Abs(root.movementInput.z), 0, 1);
                root.root.velocity.z = moveDir.normalized.z * root.template.speed * Mathf.Clamp(Mathf.Abs(root.movementInput.x) + Mathf.Abs(root.movementInput.z), 0, 1);
                root.root.velocity = root.root.velocity.Lerp(Physics.gravity, root.template.jumpingFallRate);

                //Move
                root.Move();
            }

            public virtual void UpdateAnimator()
            {

                root.root.transform.eulerAngles = new Vector3(0, root.direction + 180, 0);
            }
        }

        public class Jumping : State<CharacterControllerBehaviour>
        {
            float hangTime = 0;
            bool sustain = false;
            float sustainTime = 0;
            float waitTime = 0;
            bool waited = false;

            public virtual void Enter()
            {
                root.root.velocity.y = 1;
                waitTime = 0;
                hangTime = 0;
                sustain = false;
                sustainTime = 0;
                waited = false;
                waitTime = 0;
            }

            public virtual void Update()
            {
                //Add a short delay to the Jump for animation
                waitTime += Time.deltaTime;
                if (waitTime > 0.2 && !waited)
                {
                    root.root.velocity.y = root.template.jumpStrength;
                    waited = true;
                }
                else
                {
                    root.onGround = false;
                    root.animator.SetBool("isGrounded", root.onGround);
                }

                UpdateStates();
                UpdatePhysics();
            }

            public virtual void UpdateStates()
            {
                if(root.inputs["Jump"] > 0)
                {
                    hangTime = 0;
                    sustain = true;
                }
                else
                {
                    hangTime += Time.deltaTime;
                    sustain = false;

                    if(hangTime > root.template.hangTime * 60&&root.root.velocity.y<-1)
                    {
                        SetState("Falling");
                    }
                }

                if (root.onGround && root.root.velocity.y < 0 && waited)
                    SetState("Grounded");
            }

            public virtual void UpdatePhysics()
            {
                //Interpolate Movement Input toward the current Controller Inputs
                root.movementInput = root.movementInput.Lerp(new Vector3(root.inputs["Horizontal"], root.movementInput.y, root.inputs["Vertical"]), root.template.motionInterpolation*root.template.airControl);

                //Interpolate the Angle of the Root Direction toward the current Moving Direction
                if (Mathf.Abs(root.movementInput.x) > 0.1 || Mathf.Abs(root.movementInput.z) > 0.1)
                    root.direction = Mathf.LerpAngle(root.direction, Mathf.Atan2(root.movementInput.x, root.movementInput.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y, 0.2f);

                //Get the Move Direction
                Vector3 moveDir = Quaternion.Euler(0, root.direction, 0) * Vector3.forward;

                //Add Movement Input to the Object's Velocity
                root.root.velocity.x = moveDir.normalized.x * root.template.speed * Mathf.Clamp(Mathf.Abs(root.movementInput.x) + Mathf.Abs(root.movementInput.z), 0, 1);
                root.root.velocity.z = moveDir.normalized.z * root.template.speed * Mathf.Clamp(Mathf.Abs(root.movementInput.x) + Mathf.Abs(root.movementInput.z), 0, 1);

                if (waited)
                {
                    if (-0.2f < root.root.velocity.y && root.root.velocity.y > 0.5)
                    {
                        if (sustain && sustainTime < root.template.jumpSustain * 60)
                        {
                            sustainTime += Time.deltaTime;
                            root.root.velocity = root.root.velocity.Lerp(new Vector3(root.root.velocity.x, 0, root.root.velocity.z), root.template.jumpingFallRate);
                        }
                        else
                        {
                            SetState("Falling");
                        }
                    }
                    else
                    {
                        if (root.inputs["Jump"] > 0 && root.root.velocity.y > 0)
                            root.root.velocity = root.root.velocity.Lerp(Physics.gravity, root.template.jumpingFallRate);
                        else
                            SetState("Falling");
                    }
                }

                //Move
                root.Move();
            }
        }

        public class Falling : State<CharacterControllerBehaviour>
        {
            public virtual void Update()
            {
                UpdateStates();
                UpdatePhysics();
            }

            public virtual void UpdateStates()
            {
                if (root.onGround)
                    SetState("Grounded");
            }


            public virtual void UpdatePhysics()
            {
                //Interpolate Movement Input toward the current Controller Inputs
                root.movementInput = root.movementInput.Lerp(new Vector3(root.inputs["Horizontal"], root.movementInput.y, root.inputs["Vertical"]), root.template.motionInterpolation * root.template.airControl);

                //Interpolate the Angle of the Root Direction toward the current Moving Direction
                if (Mathf.Abs(root.movementInput.x) > 0.1 || Mathf.Abs(root.movementInput.z) > 0.1)
                    root.direction = Mathf.LerpAngle(root.direction, Mathf.Atan2(root.movementInput.x, root.movementInput.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y, 0.2f);


                //Get the Move Direction
                Vector3 moveDir = Quaternion.Euler(0, root.direction, 0) * Vector3.forward;

                //Add Movement Input to the Object's Velocity
                root.root.velocity.x = moveDir.normalized.x * root.template.speed * Mathf.Clamp(Mathf.Abs(root.movementInput.x) + Mathf.Abs(root.movementInput.z), 0, 1);
                root.root.velocity.z = moveDir.normalized.z * root.template.speed * Mathf.Clamp(Mathf.Abs(root.movementInput.x) + Mathf.Abs(root.movementInput.z), 0, 1);
                root.root.velocity = root.root.velocity.Lerp(Physics.gravity, root.template.fallRate);

                //Move
                root.Move();
            }
        }
    }
}
