//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Behaviour
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Character Controller")]
    public class BehaviourTemplate_CharacterController : BehaviourTypeTemplate
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

        public float speed = 5f;

        [Tooltip("Determines how the Controller interactions with the Animator")]
        public MotionType motionType = MotionType.AnimatorSupported;

        [Tooltip("Smooths out the change in Movement Input")]
        public float motionInterpolation = 0.15f;

        [Tooltip("The amount of motion to send to the Character when they're Jumping")]
        public float airControl = 0;

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Behaviour.CharacterControllerBehaviour";

            SetInputMap(new string[] { "Horizontal", "Vertical", "Jump" }, new string[] { "MoveHorizontal", "MoveVertical", "Jump" });
        }
    }
}
