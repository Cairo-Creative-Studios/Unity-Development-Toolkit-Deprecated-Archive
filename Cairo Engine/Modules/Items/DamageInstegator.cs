//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// You can extend the Damage Instegator to create new kinds of Damage, but most kinds of Damage can and should be made by creating 
    /// a new Damage Type Info and modifying it's values. 
    /// </summary>
    public class DamageInstegator : Resource
    {

        public class Properties
        {
            [Tooltip("The Name of this Damage, to identify what kind of Damage is to be dealt to Entities (Damage is calculated differently per-entity based on certain conditions).")]
            public string type = "Default Damage";
            [Tooltip("The radius of Damage to be dealt. 0 means that the Damage doesn't check for Entities outside of the Damage Source,\nlarger that 0 checks all objects within this Radius to see if they're Entities that can be Damaged by this Damage Type")]
            public float radius = 0.0f;
            [Tooltip("Multiply to the amount of Damage by the distance from the Center of the Damage Source to the Damage Target.\n Useful cases would be explosions, where the amount of damage decreases the further you are from the center of the burst.")]
            public float distanceFraction = 1.0f;
            [Tooltip("The amount of Damage to use for the Damage Instegator.")]
            public float damage = 1.0f;
        }
        Properties properties;

        public class Instance 
        {
            [Tooltip("The position at which the object that caused Damage to the Entity is when the Damage Calculation occurs.")]
            public Vector3 damageSource;
            [Tooltip("The Entity that created this Damage Instegator")]
            public object spawnedFrom;
            [Tooltip("The ID of the Team that owns this Damage Instegator")]
            public int team;
        }
        Instance instance;

        public static void Instantiate(DamageInstegator instegator, Vector3 position, CObject spawnedFrom, int team)
        {
            DamageInstegator instancedInstegator = ScriptableObject.Instantiate(instegator);
            instancedInstegator.instance.damageSource = position;
            instancedInstegator.instance.spawnedFrom = spawnedFrom;
            instancedInstegator.instance.team = team;
            instancedInstegator.Instegate();
        }

        /// <summary>
        /// The actual trigger of Damage Calculation. Finds the Entities that should be Damaged, and then passes Damage information off to them.
        /// </summary>
        private void Instegate()
        {
            Collider[] hitColliders = Physics.OverlapSphere(instance.damageSource, properties.radius);
            foreach (Collider collider in hitColliders)
            {
                CObject damageReciever = collider.GetComponent<CObject>();
                if(damageReciever != null)
                    damageReciever.properties["health"] = (float)damageReciever.properties["health"] - properties.damage;
            }
        }
    }

}
