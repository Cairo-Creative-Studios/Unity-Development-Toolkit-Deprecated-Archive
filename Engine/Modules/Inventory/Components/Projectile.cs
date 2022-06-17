using System;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// The Projectile class can be used for many different kinds of objects that are projected from an instantiator, but typically used for Bullets.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// The Projectile Info
        /// </summary>
        public ProjectileTemplate info;
        /// <summary>
        /// Projectile shooter information.
        /// </summary>
        public object owningEntity;
        public int team = -1;

        /// <summary>
        /// The direction the Projectile was shot
        /// </summary>
        public Vector3 moveDirection = new Vector3();
        /// <summary>
        /// The damage instegator to spawn once the Damage is Triggered
        /// </summary>
        public DamageTypeTemplate damageType;

        /// <summary>
        /// Creates a Damage Instegator 
        /// </summary>
        public void Damage()
        {
            DamageInstegator damageInstegator = new DamageInstegator(damageType, transform.position, owningEntity, team);
        }

        void Update()
        {
            //Base motion
            transform.position += moveDirection * info.speed;
        }
    }

}
