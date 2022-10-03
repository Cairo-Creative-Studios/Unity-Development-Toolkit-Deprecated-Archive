//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;
using System.Collections.Generic;
using NaughtyAttributes;

namespace CairoEngine.Drivers
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

        [Header("")]
        [Header(" -- Bullet -- ")]
        public int i;
        /// <summary>
        /// A List of Tags the Bullet can Hit. If left empty, it will send Hit Events to all Damageables it collides with
        /// </summary>
        [Tooltip("A List of Tags the Bullet can Hit. If left empty, it will send Hit Events to all Damageables it collides with")]
        [Foldout("Interaction")]
        public List<string> hitTags = new List<string>();
        /// <summary>
        /// The Type of Damage to Apply when Damage Calculation is to be done.
        /// </summary>
        [Tooltip("The Type of Damage to Apply when Damage Calculation is to be done.")]
        [Foldout("Interaction")]
        public DamageInstegator damageInstegator;
        [HorizontalLine]
        /// <summary>
        /// Whether the Bullet is to Fire Automatically as soon as it's created
        /// </summary>
        [Tooltip("Whether the Bullet is to Fire Automatically as soon as it's created")]
        [Foldout("State")]
        public bool autofire = true;
        /// <summary>
        /// The driver of the LifeSpan Timeout
        /// </summary>
        [Tooltip("The driver of the LifeSpan Timeout")]
        [Foldout("State")]
        public TimeoutFunction timeoutFunction = 0;
        /// <summary>
        /// The Life Span of the Projectile. If above zero, it will die after this duration of time passes after it's Spawn.
        /// </summary>
        [Tooltip("The Life Span of the Projectile. If above zero, it will die after this duration of time passes after it's Spawn.")]
        [Foldout("State")]
        public float lifespan = 2;
        [Header("===")]
        /// <summary>
        /// Whether to use Input from a Controller to move around
        /// </summary>
        [Tooltip("Whether to use Input from a Controller to move around")]
        [Foldout("Input")]
        public bool controllable = false;
        /// <summary>
        /// Acceleration to top Input Movement speed in any given direction
        /// </summary>
        [Tooltip("Acceleration to top Input Movement speed in any given direction")]
        [Foldout("Input")]
        public Vector3 inputAcceleration = new Vector3(0.2f, 0.2f, 0.2f);
        /// <summary>
        /// Max Input movement speed in any given direction
        /// </summary>
        [Tooltip("Max Input movement speed in any given direction")]
        [Foldout("Input")]
        public Vector3 inputMaxSpeed = new Vector3(1f, 1f, 1f);
        [Header("")]
        /// <summary>
        /// Angles the Game Object in the direction that it's moving
        /// </summary>
        [Tooltip("Angles the Game Object in the direction that it's moving")]
        [Foldout("Display")]
        public bool setAngle = false;
        /// <summary>
        /// Angle to set the Projectile to at start if setAngle is false
        /// </summary>
        [Tooltip("Angle to set the Projectile to at start if setAngle is false")]
        [Foldout("Display")]
        public Vector3 startingAngle = new Vector3(0, 0, 0);
        /// <summary>
        /// Rotation of the Projectile over time, if setAngle is false
        /// </summary>
        [Tooltip("Rotation of the Projectile over time, if setAngle is false")]
        [Foldout("Display")]
        public Vector3 rotation = new Vector3(0, 0, 0);
        [Header("===")]
        /// <summary>
        /// Whether to force the direction below when the Driver is enabled
        /// </summary>
        [Tooltip("Whether to force the direction below when the Driver is enabled")]
        [Foldout("Motion")]
        public bool forceDirection = false;
        /// <summary>
        /// The Direction to Start moving in when the Driver is enabled, if Force Direction is set to true
        /// </summary>
        [Tooltip("The Direction to Start moving in when the Driver is enabled, if Force Direction is set to true")]
        [Foldout("Motion")]
        public Vector3 startDirection = new Vector3(0, 0, 0);
        /// <summary>
        /// The rate at which to Accelerate to the moving Speed
        /// </summary>
        [Tooltip("The rate at which to Accelerate to the moving Speed")]
        [Foldout("Motion")]
        public float acceleration = 0.5f;
        /// <summary>
        /// The Speed at which the Projectile should start Moving.
        /// </summary>
        [Tooltip("The Speed at which the Projectile should start Moving.")]
        [Foldout("Motion")]
        public float speed = 1.0f;
        /// <summary>
        /// The Speed in which to move Directionally based on Controller Input.
        /// </summary>
        [Tooltip("The Speed in which to move Directionally based on Controller Input.")]
        [Foldout("Motion")]
        public float directionalSpeed = 10f;
        /// <summary>
        /// Whether Gravity should affect the movement of the Projectile.
        /// </summary>
        [Tooltip("Whether Gravity should affect the movement of the Projectile.")]
        [Foldout("Motion")]
        public bool enableGravity = false;
        [Header("===")]
        /// <summary>
        /// How strong the influence of magnetizing toward the target Transform is.
        /// </summary>
        [Tooltip("How strong the influence of magnetizing toward the target Transform is.")]
        [Foldout("Magnet")]
        public float magnetStrength = 0.0f;
        /// <summary>
        /// The Bullet will update it's angle toward Objects with these Tags.
        /// </summary>
        [Tooltip("The Bullet will update it's angle toward Objects with these Tags")]
        [Foldout("Magnet")]
        public List<string> magnetTags = new List<string>();
        /// <summary>
        /// Distance in Euler Angles the Target object can be from the Weapon's look Forward to magnetize
        /// </summary>
        [Tooltip("Distance in Euler Angles the Target object can be from the Weapon's look Forward to magnetize")]
        [Foldout("Magnet")]
        public float magnetTargetDistance = 1f;
        [Header("===")]
        /// <summary>
        /// An optional list of Commands to give to the Projectile to affect it's Movement over time.
        /// </summary>
        [Tooltip("An optional list of Commands to give to the Projectile to affect it's Movement over time.")]
        [Foldout("Extras")]
        public string motionCommands = "";

        //Initialize the driver Class for this driver
        private void OnEnable()
        {
            this.driverClass = "CairoEngine.Drivers.Bullet";

            //Set Default Inputs
            SetInputMap(new string[] { "MoveHorizontal", "MoveVertical", "RotateY", "RotateX", "Accelerate" }, new string[] { "MoveHorizontal", "MoveVertical", "RotateY", "RotateX", "Accelerate" });

            foreach (string defaultAnimationProperty in "HorizontalSpeed, VerticalSpeed, ForwardSpeed".TokenArray())
            {
                if (!animationParameterMap.ContainsKey(defaultAnimationProperty))
                    animationParameterMap.Add(defaultAnimationProperty, new parameter(defaultAnimationProperty));
            }

            foreach (string defaultEvent in "Shot,Hit,Timeout".TokenArray())
            {
                if (!scriptContainer.output.ContainsKey(defaultEvent))
                    scriptContainer.output.Add(defaultEvent, null);
                if (!scriptContainer.events.ContainsKey(defaultEvent))
                    scriptContainer.events.Add(defaultEvent, null);
            }
        }
    }
}
