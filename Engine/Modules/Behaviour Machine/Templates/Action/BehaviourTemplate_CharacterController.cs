//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;
using System.Collections.Generic;

namespace CairoEngine.Behaviour
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Character Controller", fileName = "[BEHAVIOUR] Character Controller")]
    public class BehaviourTemplate_CharacterController : BehaviourTemplate
    {
        public enum MotionType
        {
            AnimatorDriven,
            AnimatorSupported,
            Procedural,
            ProceduralAnimation
        }
        public enum ControllerType
        {
            CharacterController,
            RigidBody,
            SimpleCollider
        }
        [Header("")]
        [Header(" -- Character Controller -- ")]
        [Header(" - Features - ")]
        /// <summary>
        /// Determines what Component is used as the base of the Character Controller
        /// </summary>
        [Tooltip("Determines what Component is used as the base of the Character Controller")]
        public ControllerType controllerType;
        /// <summary>
        /// Whether to Align the Character to the Ground. (Must use a Pivot Object as the Parent of the Character, for Alignment)
        /// </summary>
        [Tooltip("Whether to Align the Character to the Ground. (Must use a Pivot Object as the Parent of the Character, for Alignment)")]
        public bool slopeAlignment = true;

        [Header(" - Child Paths - ")]
        /// <summary>
        /// The Path to the Child Object within the Prefab that Contains the Character Controller
        /// </summary>
        [Tooltip("The Path to the Child Object within the Prefab that Contains the Character Controller")]
        public string controllerPath = "";
        /// <summary>
        /// The Path to the Child Object within the Prefab that Contains the Rigid Body
        /// </summary>
        [Tooltip("The Path to the Child Object within the Prefab that Contains the Rigid Body")]
        public string rigidBodyPath = "";
        /// <summary>
        /// The Path to the Pivot to use for Ground Alignment
        /// </summary>
        [Tooltip("The Path to the Pivot to use for Ground Alignment")]
        public string groundPivotPath = "Pivot";

        [Header(" - Base Movement Values - ")]
        /// <summary>
        /// The Base Movement Speed of the Character Controller
        /// </summary>
        [Tooltip("The Base Movement Speed of the Character Controller")]
        public float speed = 5f;
        /// <summary>
        /// Smooths out the change in Movement Speed
        /// </summary>
        [Tooltip("Smooths out the change in Movement Speed")]
        public float motionInterpolation = 0.15f;
        /// <summary>
        /// The Rate at which the Character Controller turns
        /// </summary>
        [Tooltip("The Rate at which the Character Controller turns")]
        public float turnRate = 0.5f;


        [Header(" - Air Values - ")]
        /// <summary>
        /// The amount of motion to send to the Character when they're Jumping
        /// </summary>
        [Tooltip("The amount of motion to send to the Character when they're Jumping")]
        public float airControl = 0;
        /// <summary>
        /// An Amount to add to the current Velocity of the Object when it has Jumped
        /// </summary>
        [Tooltip("An Amount to add to the current Velocity of the Object when it has Jumped")]
        public float jumpStrength = 10f;
        /// <summary>
        /// An Interpolation Value to apply to moving in the direction of gravity, VS the Input Velocity
        /// </summary>
        [Tooltip("Interpolates from the current Y velocity to Gravity by this rate when not Jumping")]
        public float fallRate = 0.1f;

        [Header(" - Helpers - ")]
        /// <summary>
        /// A Duration of time to stay at the peak of a Jump if the input remains held down
        /// </summary>
        [Tooltip("A Duration of time to stay at the peak of a Jump if the input remains held down")]
        public float jumpSustain = 1f;
        /// <summary>
        /// A Duration of time to remain in the Grounded State before beginning to Fall
        /// </summary>
        [Tooltip("A Duration of time to remain in the Grounded State before beginning to Fall")]
        public float cayoteTime = 0.25f;
        /// <summary>
        /// A Duration of time to remain in the Jumping State when not pressing the Jump Input button
        /// </summary>
        [Tooltip("A Duration of time to remain in the Jumping State when not pressing the Jump Input button")]
        public float hangTime = 0.25f;

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            //Set Default Class
            this.behaviourClass = "CairoEngine.Behaviour.CharacterController";

            //Set Default Inputs
            SetInputMap(new string[] { "Horizontal", "Vertical", "Jump" }, new string[] { "MoveHorizontal", "MoveVertical", "Jump" });

            //Set Default Audio Clips
            //foreach (string clipName in "".TokenArray())
            //{
            //    if (!audioTable.ContainsKey(clipName))
            //    {
            //        audioTable.Add(clipName, new List<AudioClip>());
            //    }
            //    else if (audioTable[clipName] == null)
            //    {
            //        audioTable[clipName] = new List<AudioClip>();
            //    }
            //}

            foreach (string defaultEvent in "Idle, Run, Land, Jump, Fall, Hit".TokenArray())
            {
                if (!scriptContainer.Output.ContainsKey(defaultEvent))
                    scriptContainer.Output.Add(defaultEvent, null);
            }
        }
    }
}
