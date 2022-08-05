//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;
using System.Collections.Generic;

namespace CairoEngine.Behaviour
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Bullet", fileName = "[BEHAVIOUR] Bullet")]
    public class BehaviourTemplate_Bullet : BehaviourTemplate
    {
        [Header(" - Motion - ")]
        /// <summary>
        /// The Life Span of the Projectile. If above zero, it will die after this duration of time passes after it's Spawn.
        /// </summary>
        [Tooltip("The Life Span of the Projectile. If above zero, it will die after this duration of time passes after it's Spawn.")]
        public float lifespan = 2;
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
            this.behaviourClass = "CairoEngine.Behaviour.Bullet";
        }
    }
}
