using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Stats/Damage Type")]
    public class DamageTypeTemplate : ScriptableObject
    {
        /// <summary>
        /// The Name of this Damage, to identify what kind of Damage is to be dealt to Entities (Damage is calculated differently per-entity based on certain conditions).
        /// </summary>
        public string type = "Default Damage";
        /// <summary>
        /// The radius of Damage to be dealt. 0 means that the Damage doesn't check for Entities outside of the Damage Source,
        /// larger that 0 checks all objects within this Radius to see if they're Entities that can be Damaged by this Damage Type.
        /// </summary>
        public float radius = 0.0f;
        /// <summary>
        /// Multiply to the amount of Damage by the distance from the Center of the Damage Source to the Damage Target. 
        /// Useful cases would be explosions, where the amount of damage decreases the further you are from the center of the burst.
        /// </summary>
        public float distanceFraction = 1.0f;
        /// <summary>
        /// The amount of Damage to use for the Damage Instegator.
        /// </summary>
        public float damage = 1.0f;
    }
}