//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using UDT;
using System.Collections.Generic;
using NaughtyAttributes;

namespace UDT.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Action/Bullet", fileName = "[DRIVER] Bullet")]
    public class DriverTemplate_Bullet : DriverTemplate
    {
        public enum TimeoutFunction
        {
            Destroy,
            Return,
            CallEvent
        }

        /// <summary>
        /// Collection of Interaction Properties for the Bullet
        /// </summary>
        [Serializable]
        public class InteractionInfo
        {
            /// <summary>
            /// A List of Tags the Bullet can Hit. If left empty, it will send Hit Events to all Damageables it collides with
            /// </summary>
            [Tooltip("A List of Tags the Bullet can Hit. If left empty, it will send Hit Events to all Damageables it collides with")]
            public List<string> hitTags = new List<string>();
            /// <summary>
            /// The Type of Damage to Apply when Damage Calculation is to be done.
            /// </summary>
            [Tooltip("The Type of Damage to Apply when Damage Calculation is to be done.")]
            public DamageInstegator damageInstegator;
        }
        /// <summary>
        /// Collection of State Properties for the Bullet
        /// </summary>
        [Serializable]
        public class StateInfo
        {
            /// <summary>
            /// Whether the Bullet is to Fire Automatically as soon as it's created
            /// </summary>
            [Tooltip("Whether the Bullet is to Fire Automatically as soon as it's created")]
            public bool autofire = true;
            /// <summary>
            /// The driver of the LifeSpan Timeout
            /// </summary>
            [Tooltip("The driver of the LifeSpan Timeout")]
            public TimeoutFunction timeoutFunction = 0;
            /// <summary>
            /// The Life Span of the Projectile. If above zero, it will die after this duration of time passes after it's Spawn.
            /// </summary>
            [Tooltip("The Life Span of the Projectile. If above zero, it will die after this duration of time passes after it's Spawn.")]
            public float lifespan = 2;
        }
        /// <summary>
        /// Collection of Input Properties for the Bullet
        /// </summary>
        [Serializable]
        public class InputInfo
        {
            /// <summary>
            /// Whether to use Input from a Controller to move around
            /// </summary>
            [Tooltip("Whether to use Input from a Controller to move around")]
            public bool controllable = false;
            /// <summary>
            /// Acceleration to top Input Movement speed in any given direction
            /// </summary>
            [Tooltip("Acceleration to top Input Movement speed in any given direction")]
            public Vector3 inputAcceleration = new Vector3(0.2f, 0.2f, 0.2f);
            /// <summary>
            /// Max Input movement speed in any given direction
            /// </summary>
            [Tooltip("Max Input movement speed in any given direction")]
            public Vector3 inputMaxSpeed = new Vector3(1f, 1f, 1f);
        }
        /// <summary>
        /// Collection of Display Properties for the Bullet
        /// </summary>
        public class DisplayInfo
        {
            /// <summary>
            /// Angles the Game Object in the direction that it's moving
            /// </summary>
            [Tooltip("Angles the Game Object in the direction that it's moving")]
            public bool setAngle = false;
            /// <summary>
            /// Angle to set the Projectile to at start if setAngle is false
            /// </summary>
            [Tooltip("Angle to set the Projectile to at start if setAngle is false")]
            public Vector3 startingAngle = new Vector3(0, 0, 0);
            /// <summary>
            /// Rotation of the Projectile over time, if setAngle is false
            /// </summary>
            [Tooltip("Rotation of the Projectile over time, if setAngle is false")]
            public Vector3 rotation = new Vector3(0, 0, 0);
        }
        /// <summary>
        /// Collection of Motion Properties for the Bullet
        /// </summary>
        [Serializable]
        public class MotionInfo
        {
            /// <summary>
            /// Whether to force the direction below when the Driver is enabled
            /// </summary>
            [Tooltip("Whether to force the direction below when the Driver is enabled")]
            public bool forceDirection = false;
            /// <summary>
            /// The Direction to Start moving in when the Driver is enabled, if Force Direction is set to true
            /// </summary>
            [Tooltip("The Direction to Start moving in when the Driver is enabled, if Force Direction is set to true")]
            public Vector3 startDirection = new Vector3(0, 0, 0);
            /// <summary>
            /// The rate at which to Accelerate to the moving Speed
            /// </summary>
            [Tooltip("The rate at which to Accelerate to the moving Speed")]
            public float acceleration = 0.5f;
            /// <summary>
            /// The Speed at which the Projectile should start Moving.
            /// </summary>
            [Tooltip("The Speed at which the Projectile should start Moving.")]
            public float speed = 1.0f;
            /// <summary>
            /// The Speed in which to move Directionally based on Controller Input.
            /// </summary>
            [Tooltip("The Speed in which to move Directionally based on Controller Input.")]
            public float directionalSpeed = 10f;
            /// <summary>
            /// Whether Gravity should affect the movement of the Projectile.
            /// </summary>
            [Tooltip("Whether Gravity should affect the movement of the Projectile.")]
            public bool enableGravity = false;
        }
        /// <summary>
        /// Collection of Magnet Properties for the Bullet
        /// </summary>
        [Serializable]
        public class MagnetInfo
        {
            /// <summary>
            /// How strong the influence of magnetizing toward the target Transform is.
            /// </summary>
            [Tooltip("How strong the influence of magnetizing toward the target Transform is.")]
            public float magnetStrength = 0.0f;
            /// <summary>
            /// The Bullet will update it's angle toward Objects with these Tags.
            /// </summary>
            [Tooltip("The Bullet will update it's angle toward Objects with these Tags")]
            public List<string> magnetTags = new List<string>();
            /// <summary>
            /// Distance in Euler Angles the Target object can be from the Weapon's look Forward to magnetize
            /// </summary>
            [Tooltip("Distance in Euler Angles the Target object can be from the Weapon's look Forward to magnetize")]
            public float magnetTargetDistance = 1f;
        }
        /// <summary>
        /// Collection of Extra Properties for the Bullet
        /// </summary>
        [Serializable]
        public class ExtrasInfo
        {
            /// <summary>
            /// An optional list of Commands to give to the Projectile to affect it's Movement over time.
            /// </summary>
            [Tooltip("An optional list of Commands to give to the Projectile to affect it's Movement over time.")]
            public string motionCommands = "";
        }
        /// <summary>
        /// The Bullet Properties Container Class
        /// </summary>
        [Serializable]
        public class BulletProperties
        {
            [Header("These are Properties/Parameters to modify the functionality of the Bullet Driver when it is Enabled on a Game Object.")]
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The interaction properties of the Bullet
            /// </summary>
            [Tooltip("Interaction Properties of the Bullet")]
            public InteractionInfo interaction = new InteractionInfo();
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The State Properties of the Bullet
            /// </summary>
            [Tooltip("The State Properties of the Bullet")]
            public StateInfo state = new StateInfo();
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The Input Properties of the Bullet
            /// </summary>
            [Tooltip("The Input Properties of the Bullet")]
            public InputInfo input = new InputInfo();
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The Display Properties of the Bullet
            /// </summary>
            [Tooltip("The Display Properties of the Bullet")]
            public DisplayInfo display = new DisplayInfo();
            [HorizontalLine]
            /// <summary>
            /// The Motion Properties of the Bullet
            /// </summary>
            [Tooltip("The Motion Properties of the Bullet")]
            public MotionInfo motion = new MotionInfo();
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The Magnet Properties of the Bullet
            /// </summary>
            [Tooltip("The Magnet Propeties of the Bullet")]
            public MagnetInfo magnet = new MagnetInfo();
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The Extra PRoperties of the Bullet
            /// </summary>
            [Tooltip("The Extra Properties of the Bullet")]
            public ExtrasInfo extras = new ExtrasInfo();
        }
        [HorizontalLine(color: EColor.White)]
        [Tooltip("The Properties of the Bullet Driver")]
        [BoxGroup("Bullet")]
        public BulletProperties bulletProperties;

        /// <summary>
        /// Initializes the Driver Template
        /// </summary>
        private void OnEnable()
        {
            //Set the Driver Class
            this.driverProperties.main.driverClass = "UDT.Drivers.Bullet";
            //Set Default Inputs
            SetInputTranslation(new string[] { "MoveHorizontal", "MoveVertical", "RotateY", "RotateX", "Accelerate" }, new string[] { "MoveHorizontal", "MoveVertical", "RotateY", "RotateX", "Accelerate" });
            //Set Default Animation Parameters
            SetAnimationParameters("HorizontalSpeed, VerticalSpeed, ForwardSpeed".TokenArray());
            //Set Default Scripting Events
            SetScriptingEvents("Shot,Hit,Timeout".TokenArray());
        }
    }
}
