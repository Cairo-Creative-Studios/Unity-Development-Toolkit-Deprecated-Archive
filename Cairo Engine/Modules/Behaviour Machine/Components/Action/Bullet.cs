using System;
using UnityEngine;

namespace CairoEngine.Behaviour
{
    public class Bullet : CairoBehaviour<BehaviourTemplate_Bullet>
    {
        /// <summary>
        /// Projectile shooter information.
        /// </summary>
        public CObject parentObject;
        public int team = -1;
        float life;
        /// <summary>
        /// The damage instegator to spawn once the Damage is Triggered
        /// </summary>
        public DamageInstegator damageInstegator;

        public void Init()
        {
            
        }

        /// <summary>
        /// Creates a Damage Instegator 
        /// </summary>
        public void Damage()
        {
            DamageInstegator.Instantiate(damageInstegator, transform.position, parentObject, team);
        }

        public void Update()
        {
            //moveDirection.Normalize();
            transform.position += rootTransform.forward * template.speed;

            life += Time.deltaTime;
            if (life > template.lifespan)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
