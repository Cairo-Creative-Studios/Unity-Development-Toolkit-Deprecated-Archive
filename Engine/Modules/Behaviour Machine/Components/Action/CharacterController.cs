//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;
using CairoEngine.StateMachine;

namespace CairoEngine.Behaviour
{
    [Serializable]
    public class CharacterController : CairoBehaviour<BehaviourTemplate_CharacterController>
    {
        #region Global
        private UnityEngine.CharacterController characterController;

        private float direction = 0;
        private Vector3 movementInput = new Vector3();
        private bool onGround = false;
        private Transform pivot;
        private Vector3 lastPivotPosition = new Vector3();
        private float offGroundTimer = 0;
        private int tick = 0;
        private bool canJump = false;

        public virtual void Init()
        {
            if (template.controllerPath != "")
            {
                if(gameObject.transform.Find(template.controllerPath).gameObject.GetComponent<UnityEngine.CharacterController>() == null)
                {
                    characterController = (UnityEngine.CharacterController)SetProperty("characterControllerComponent", gameObject.transform.Find(template.controllerPath).gameObject.AddComponent<UnityEngine.CharacterController>());
                }
                else
                {
                    characterController = (UnityEngine.CharacterController)SetProperty("characterControllerComponent", gameObject.transform.Find(template.controllerPath).gameObject.GetComponent<UnityEngine.CharacterController>());
                }
            }
            else
            {
                if (gameObject.GetComponent<UnityEngine.CharacterController>() == null)
                {
                    characterController = (UnityEngine.CharacterController)SetProperty("characterControllerComponent", gameObject.AddComponent<UnityEngine.CharacterController>());
                }
                else
                {
                    characterController = (UnityEngine.CharacterController) SetProperty("characterControllerComponent", gameObject.GetComponent<UnityEngine.CharacterController>());
                }
            }

            //Get a local reference
            StateMachineModule.AddComponent(gameObject, StateMachineModule.CreateStateMachine(this));
            pivot = gameObject.transform.Find(template.groundPivotPath);
        }

        public void Update()
        {
            tick++;

            //Interpolate Movement Input toward the current Controller Inputs
            movementInput = new Vector3(inputs["Horizontal"], movementInput.y, inputs["Vertical"]);

            
            if(animator != null)
            {
                animator.SetFloat("HorizontalSpeed", Mathf.Abs(characterController.velocity.x) + Mathf.Abs(characterController.velocity.z));
                animator.SetBool("isGrounded", onGround);
                animator.SetFloat("VerticalSpeed", characterController.velocity.y);
            }
        }

        /// <summary>
        /// Move the Character controller
        /// </summary>
        public virtual void Move(float interpolation)
        {
            //Interpolate the Angle of the Root Direction toward the current Moving Direction
            if (Mathf.Abs(movementInput.x) > 0.1 || Mathf.Abs(movementInput.z) > 0.1)
                direction = Mathf.LerpAngle(direction, Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y, template.turnRate);

            //Get the Move Direction
            Vector3 moveDir = Quaternion.Euler(0, direction, 0) * Vector3.forward * Mathf.Clamp(Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.z), 0, 1);

            //Add Movement Input to the Object's Velocity
            root.velocity.x = moveDir.normalized.x * template.speed;
            root.velocity.z = moveDir.normalized.z * template.speed;

            RaycastHit reflectHit;
            Vector3 reflect = moveDir;
            reflect.y = 0;

            //if (Physics.Raycast(characterController.transform.position + new Vector3(0, characterController.height/2, 0), reflect.normalized, out reflectHit, characterController.radius))
            //{
            //    reflect = Vector3.Reflect(root.velocity, reflectHit.normal);
            //    root.velocity.x = Mathf.Lerp(root.velocity.x, reflect.x, (Mathf.Pow(reflect.y,3)/1000));
            //    root.velocity.z = Mathf.Lerp(root.velocity.z, reflect.z, (Mathf.Pow(reflect.y,3)/1000));
            //}

            if(Physics.Raycast(characterController.transform.position+ new Vector3(0, characterController.height, 0), Vector3.up, out reflectHit, 0.2f))
            {
                if (root.velocity.y > 0)
                {
                    root.velocity.y = -1;
                }
            }

            onGround = false;
            RaycastHit[] hits = Physics.BoxCastAll(characterController.transform.position + new Vector3(0, characterController.radius, 0), new Vector3(characterController.radius/2, 0.1f, characterController.radius/2), Vector3.down);
            float groundSlopePercentage = 0;


            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.distance <= characterController.radius && hit.collider.gameObject.tag != "Player")
                    {
                        if(template.slopeAlignment)
                            pivot.eulerAngles = pivot.eulerAngles.LerpAngle(Quaternion.FromToRotation(Vector3.up, hit.normal).eulerAngles, 0.25f);

                        onGround = true;

                        reflect = Vector3.Reflect(root.velocity, reflectHit.normal);
                        groundSlopePercentage = Mathf.Lerp(root.velocity.x, reflect.x, (Mathf.Pow(Mathf.Clamp(Mathf.Abs(reflect.x) + Mathf.Abs(reflect.z), 0, 10), 3) / 1000));
                    }
                }
            }
            if (!onGround&&template.slopeAlignment)
            {
                pivot.eulerAngles = pivot.eulerAngles.LerpAngle(Vector3.zero, 0.1f);
            }

            //TODO: Move Faster in the facing direction when getting closer to top speed, regardless of 

            //Apply Gravity in the Direction of the current Slope Angle, use the Slope Angle Percentage to find how much Gravity should be applied
            root.velocity = root.velocity.Lerp(Quaternion.Euler(rootTransform.eulerAngles + new Vector3(0,180,0)) * Physics.gravity, template.fallRate);

            if (onGround)
            {
                if (root.velocity.y < 0)
                {
                    root.velocity.y *= groundSlopePercentage / 9;
                    root.velocity.y--;
                }
            }

            characterController.Move(root.velocity*Time.deltaTime);

            //Update Pivot
            pivot.position = pivot.position.Lerp(characterController.transform.position, 0.95f);
            lastPivotPosition = pivot.position;
        }
        #endregion

        public class Grounded : State<CharacterController>
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
                    if (root.canJump)
                        SetState("Jumping");
                    root.canJump = false;
                }
                else
                    root.canJump = true;

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

                //Move
                root.Move(root.template.motionInterpolation);
            }

            public virtual void UpdateAnimator()
            {

                root.rootTransform.localRotation = Quaternion.Euler(new Vector3(0, root.direction, 0));
            }
        }

        public class Jumping : State<CharacterController>
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
                if (waitTime > 0 && !waited)
                {
                    waited = true;
                }
                else
                {
                    root.onGround = false;
                    root.animator.SetBool("isGrounded", root.onGround);
                }

                    root.root.velocity.y += root.template.jumpStrength;


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

                if (waited)
                {
                    if (-0.2f < root.root.velocity.y && root.root.velocity.y > 0.5)
                    {
                        if (sustain && sustainTime < root.template.jumpSustain * 60)
                        {
                            sustainTime += Time.deltaTime;
                        }
                        else
                        {
                            SetState("Falling");
                        }
                    }
                    else
                    {
                        if (root.inputs["Jump"] < 0 && root.root.velocity.y < 0)
                            SetState("Falling");
                    }
                }

                //Move
                root.Move(root.template.motionInterpolation*root.template.airControl);
            }
        }

        public class Falling : State<CharacterController>
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
                //Add Movement Input to the Object's Velocity


                //Move
                root.Move(root.template.motionInterpolation * root.template.airControl);
            }
        }
    }
}
