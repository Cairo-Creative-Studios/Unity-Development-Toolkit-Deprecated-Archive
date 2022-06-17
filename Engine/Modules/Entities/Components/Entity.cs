using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// The Entity class is a Template for classes that use similar Entity-like behaviour, but should not be used on it's own.
    /// </summary>
    public class Entity : Object
    {
        /// <summary>
        /// The Controller of this Entity
        /// </summary>
        public object controller;
        /// <summary>
        /// The Inputs recieved from the Controller of the Entity.
        /// </summary>
        public Dictionary<string, float> inputs = new Dictionary<string, float>();
        /// <summary>
        /// The Data of this Entity
        /// </summary>
        public EntityTemplate entityInfo;
        /// <summary>
        /// The current Inventory of the Entity
        /// </summary>
        public List<Item> inventory = new List<Item>();
        /// <summary>
        /// The health of the Entity
        /// </summary>
        public float health = 100.0f;
        /// <summary>
        /// Stats other than Health to apply to the Entity
        /// </summary>
        public Dictionary<string, float> stats = new Dictionary<string, float>();


        /// <summary>
        /// The Damage Instegator that killed this Pawn.
        /// </summary>
        public DamageInstegator killingDamageInstegator;

        /// <summary>
        /// Damage is dealt to the enemy, and calculated here. You can use Stats to modify the amount of Damage dealt, and handle killing conditions here as well.
        /// </summary>
        /// <param name="damage">The amount of Damage done to the Entity</param>
        /// <param name="damageInstegator">The Damage Instegator.</param>
        public virtual void CalculateDamage(float damage, DamageInstegator damageInstegator)
        {
            health -= damage;

            if (health < 0.0f)
            {
                killingDamageInstegator = damageInstegator;
                StateMachineModule.SetState(gameObject, "Dead");
            }
        }

        /// <summary>
        /// Deletes an Inventory Item from the Entity's Inventory.
        /// </summary>
        /// <param name="item">Item.</param>
        public void DeleteInventoryItem(Item item)
        {
            inventory.Remove(item);
        }

        void Update()
        {
            Debug.Log("I exist!");
        }
    }

}
