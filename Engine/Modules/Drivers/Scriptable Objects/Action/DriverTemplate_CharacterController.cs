//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;
using System.Collections.Generic;
using NaughtyAttributes;

namespace CairoEngine.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Action/Character Controller", fileName = "[DRIVER] Character Controller")]
    public class DriverTemplate_CharacterController : DriverTemplate
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
        /// <summary>
        /// Container of Feature Properties for the Character Controller
        /// </summary>
        [Serializable]
        public class FeatureInfo
        {
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
        }
        /// <summary>
        /// Container of Component Paths for the Character Controller
        /// </summary>
        [Serializable]
        public class ComponentPathInfo
        {
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
        }
        /// <summary>
        /// Container of Motion Properties for the Character Controller
        /// </summary>
        [Serializable]
        public class MotionInfo
        {
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
            /// <summary>
            /// An Amount to add to the current Velocity of the Object when it has Jumped
            /// </summary>
            [Tooltip("An Amount to add to the current Velocity of the Object when it has Jumped")]
            public float jumpStrength = 10f;
        }
        /// <summary>
        /// Container of Air Control Properties for the Character Controller
        /// </summary>
        [Serializable]
        public class AirContolInfo
        {
            /// <summary>
            /// The amount of motion to send to the Character when they're Jumping
            /// </summary>
            [Tooltip("The amount of motion to send to the Character when they're Jumping")]
            [Foldout("Air Motion")]
            public float airControl = 0;
            /// <summary>
            /// An Interpolation Value to apply to moving in the direction of gravity, VS the Input Velocity
            /// </summary>
            [Tooltip("Interpolates from the current Y velocity to Gravity by this rate when not Jumping")]
            [Foldout("Air Motion")]
            public float fallRate = 0.1f;
        }
        /// <summary>
        /// Container of Modifier Properties for the Character Controller
        /// </summary>
        [Serializable]
        public class ModifierInfo
        {
            /// <summary>
            /// A Duration of time to stay at the peak of a Jump if the input remains held down
            /// </summary>
            [Tooltip("A Duration of time to stay at the peak of a Jump if the input remains held down")]
            [Foldout("Modifiers")]
            public float jumpSustain = 1f;
            /// <summary>
            /// A Duration of time to remain in the Grounded State before beginning to Fall
            /// </summary>
            [Tooltip("A Duration of time to remain in the Grounded State before beginning to Fall")]
            [Foldout("Modifiers")]
            public float cayoteTime = 0.25f;
            /// <summary>
            /// A Duration of time to remain in the Jumping State when not pressing the Jump Input button
            /// </summary>
            [Tooltip("A Duration of time to remain in the Jumping State when not pressing the Jump Input button")]
            [Foldout("Modifiers")]
            public float hangTime = 0.25f;
        }
        /// <summary>
        /// Container of Character Controller Properties
        /// </summary>
        [Serializable]
        public class CharacterControllerProperties
        {
            [Header("Properties that control the functionality of the Character Controller Driver")]
            /// <summary>
            /// Feature Properties for the Character Controller
            /// </summary>
            [Tooltip("Feature Properties for the Character Controller")]
            [HorizontalLine(color: EColor.Gray)]
            public FeatureInfo features = new FeatureInfo();
            /// <summary>
            /// Component Paths for the Character Controller
            /// </summary>
            [Tooltip("Component Paths for the Character Controller")]
            [HorizontalLine(color: EColor.Gray)]
            public ComponentPathInfo componentPaths = new ComponentPathInfo();
            /// <summary>
            /// Motion Properties for the Character Controller
            /// </summary>
            [Tooltip("Motion Properties for the Character Controller")]
            [HorizontalLine(color: EColor.Gray)]
            public MotionInfo motion = new MotionInfo();
            /// <summary>
            /// Air Control Properties for the Character Controller
            /// </summary>
            [Tooltip("Air Control Properties for the Character Controller")]
            [HorizontalLine(color: EColor.Gray)]
            public AirContolInfo airControl = new AirContolInfo();
            /// <summary>
            /// Modifier Properties for the Character Controller
            /// </summary>
            [Tooltip("Modifier Properties for the Character Controller")]
            [HorizontalLine(color: EColor.Gray)]
            public ModifierInfo modifiers = new ModifierInfo();
        }
        /// <summary>
        /// The Properties of the Character Controller Driver
        /// </summary>
        [Tooltip("The Properties of the Character Controller Driver")]
        [HorizontalLine(color: EColor.White)]
        [BoxGroup("Character Controller")]
        public CharacterControllerProperties characterControllerProperties = new CharacterControllerProperties();

        //Initialize the driver Class for this driver
        private void OnEnable()
        {
            //Set Default Class
            this.driverProperties.main.driverClass = "CairoEngine.Drivers.CharacterController";

            //Set Default Inputs
            SetInputTranslation(new string[] { "Horizontal", "Vertical", "Jump" }, new string[] { "MoveHorizontal", "MoveVertical", "Jump" });

            SetScriptingEvents("Idle, Run, Land, Jump, Fall, Hit".TokenArray());

            SetAnimationParameters("HorizontalSpeed, VerticalSpeed, isGrounded".TokenArray());
        }
    }
}
