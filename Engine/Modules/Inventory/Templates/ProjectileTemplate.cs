using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Inventory/Projectile")]
    public class ProjectileTemplate : ObjectTemplate
    {
        /// <summary>
        /// The prefab to spawn for the Projectile.
        /// </summary>
        [Tooltip("The prefab to spawn for the Projectile.")]
        public GameObject projectilePrefab;
        /// <summary>
        /// The Speed at which the Projectile should start Moving.
        /// </summary>
        [Tooltip("The Speed at which the Projectile should start Moving.")]
        public float speed = 1.0f;
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

        /// <summary>
        /// How strong the influence of magnetizing toward the target Transform is.
        /// </summary>
        [Tooltip("How strong the influence of magnetizing toward the target Transform is.")]
        public float magnetStrength = 0.0f;

        /// <summary>
        /// The Type of Damage to Apply when Damage Calculation is to be done.
        /// </summary>
        [Tooltip("The Type of Damage to Apply when Damage Calculation is to be done.")]
        public DamageTypeTemplate damageType;

        /// <summary>
        /// The Life Span of the Projectile. If above zero, it will die after this duration of time passes after it's Spawn.
        /// </summary>
        [Tooltip("The Life Span of the Projectile. If above zero, it will die after this duration of time passes after it's Spawn.")]
        public float lifespan = -1;
    }
}

