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

        [Tooltip("The Path to the Object within the Prefab that Contains the Character Controller")]
        public string controllerPath = "";
        public string rigidBodyPath = "";
        [Tooltip("The Base Movement Speed of the Character Controller")]
        public float speed = 5f;

        [Tooltip("Smooths out the change in Movement Input")]
        public float motionInterpolation = 0.15f;

        [Tooltip("The amount of motion to send to the Character when they're Jumping")]
        public float airControl = 0;

        [Tooltip("The Rate at which the Character Controller turns")]
        public float turnRate = 0.5f;

        [Tooltip("An Amount to add to the current Velocity of the Object when it has Jumped")]
        public float jumpStrength = 10f;

        [Tooltip("Interpolates from the current Y velocity to Gravity by this rate while Jumping")]
        public float jumpingFallRate = 0.05f;

        [Tooltip("Interpolates from the current Y velocity to Gravity by this rate when not Jumping")]
        public float fallRate = 0.1f;

        [Tooltip("A Duration of time to stay at the peak of a Jump if the input remains held down")]
        public float jumpSustain = 1f;

        [Tooltip("A Duration of time to remain in the Grounded State before beginning to Fall")]
        public float cayoteTime = 0.25f;

        [Tooltip("A Duration of time to remain in the Jumping State when not pressing the Jump Input button")]
        public float hangTime = 0.25f;
        [Tooltip("Whether to align the Character to the Ground it's standing on")]
        public bool alignToGround = true;
        [Tooltip("The Path to the Pivot to use for Ground Alignment")]
        public string groundPivotPath = "Pivot";

        [Tooltip("Determines how the Controller interactions with the Animator")]
        public MotionType motionType = MotionType.AnimatorSupported;

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
        }
    }
}
