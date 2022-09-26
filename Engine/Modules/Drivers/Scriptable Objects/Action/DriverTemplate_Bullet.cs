//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;
using System.Collections.Generic;

namespace CairoEngine.Drivers
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Bullet", fileName = "[BEHAVIOUR] Bullet")]
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
        [Header(" - Interaction - ")]
        /// <summary>
        /// A List of Tags the Bullet can Hit. If left empty, it will send Hit Events to all Damageables it collides with
        /// </summary>
        [Tooltip("A List of Tags the Bullet can Hit. If left empty, it will send Hit Events to all Damageables it collides with")]
        public List<string> hitTags = new List<string>();

        [Header(" - State - ")]
        public bool autofire = true;
        [Tooltip("The Behaviour of the LifeSpan Timeout")]
        public TimeoutFunction timeoutFunction = 0;

        [Header(" - Display - ")]
        [Tooltip("Angles the Game Object in the direction that it's moving")]
        public bool setAngle = false;
        [Tooltip("Angle to set the Projectile to at start if setAngle is false")]
        public Vector3 startingAngle = new Vector3(0, 0, 0);
        [Tooltip("Rotation of the Projectile over time, if setAngle is false")]
        public Vector3 rotation = new Vector3(0, 0, 0);

        [Header(" - Motion - ")]
        [Tooltip("Whether to use Input from a Controller to move around")]
        public bool controllable = false;
        /// <summary>
        /// The Life Span of the Projectile. If above zero, it will die after this duration of time passes after it's Spawn.
        /// </summary>
        [Tooltip("The Life Span of the Projectile. If above zero, it will die after this duration of time passes after it's Spawn.")]
        public float lifespan = 2;
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
        public float directionalSpeed = 10f;
        /// <summary>
        /// Whether Gravity should affect the movement of the Projectile
        /// </summary>
        [Tooltip("Whether Gravity should affect the movement of the Projectile.")]
        public bool enableGravity = false;
        /// <summary>
        /// An optional list of Commands to give to the Projectile to affect it's Movement over time.
        /// </summary>
        [Tooltip("An optional list of Commands to give to the Projectile to affect it's Movement over time.")]
        public string motionCommands = "";


        [Header(" - Magnet - ")]
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


        [Header(" - Impact - ")]
        /// <summary>
        /// The Type of Damage to Apply when Damage Calculation is to be done.
        /// </summary>
        [Tooltip("The Type of Damage to Apply when Damage Calculation is to be done.")]
        public DamageInstegator damageInstegator;

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Drivers.Bullet";

            //Set Default Inputs
            SetInputMap(new string[] { "MoveHorizontal", "MoveVertical", "RotateY", "RotateX", "Accelerate" }, new string[] { "MoveHorizontal", "MoveVertical", "RotateY", "RotateX", "Accelerate" });

            foreach (string defaultAnimationProperty in "HorizontalSpeed, VerticalSpeed, ForwardSpeed".TokenArray())
            {
                if (!animationParameterMap.ContainsKey(defaultAnimationProperty))
                    animationParameterMap.Add(defaultAnimationProperty, new parameter(defaultAnimationProperty));
            }

            foreach (string defaultEvent in "Shot,Hit,Timeout".TokenArray())
            {
                if (!scriptContainer.Output.ContainsKey(defaultEvent))
                    scriptContainer.Output.Add(defaultEvent, null);
                if (!scriptContainer.Events.ContainsKey(defaultEvent))
                    scriptContainer.Events.Add(defaultEvent, null);
            }
        }
    }
}
