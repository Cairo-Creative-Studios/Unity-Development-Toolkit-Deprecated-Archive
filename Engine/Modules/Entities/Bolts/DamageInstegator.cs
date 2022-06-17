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
    public class DamageInstegator
    {
        /// <summary>
        /// Holds information about how Damage should be dealt. 
        /// </summary>
        public DamageTypeTemplate template;

        /// <summary>
        /// The position at which the object that caused Damage to the Entity is when the Damage Calculation occurs.
        /// </summary>
        public Vector3 damageSource;

        /// <summary>
        /// The Entity that created this Damage Instegator
        /// </summary>
        public object owningEntity;

        /// <summary>
        /// The ID of the Team that owns this Damage Instegator
        /// </summary>
        public int team;

        /// <summary>
        /// Contructing a Damage Instegator sets it up and determines what Entities should be Damaged by it.
        /// </summary>
        /// <param name="damageType">Damage type.</param>
        /// <param name="damageSource">Damage source.</param>
        /// <param name="owningEntity">Owning entity.</param>
        /// <param name="team">Team.</param>
        public DamageInstegator(DamageTypeTemplate damageType, Vector3 damageSource, object owningEntity = null, int team = -1)
        {
            template = damageType;
            this.damageSource = damageSource;
            this.owningEntity = owningEntity;
            this.team = team;

            Instegate();
        }

        /// <summary>
        /// The actual trigger of Damage Calculation. Finds the Entities that should be Damaged, and then passes Damage information off to them.
        /// </summary>
        private void Instegate()
        {
            float damage = template.damage;

            Collider[] hitColliders = Physics.OverlapSphere(damageSource, template.radius);
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.tag == "Entity")
                {
                    Entity hitEntity = collider.GetComponent<Entity>();
                    hitEntity.health -= damage;
                }
            }
        }
    }

}
