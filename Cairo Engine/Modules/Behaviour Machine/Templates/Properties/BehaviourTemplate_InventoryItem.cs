using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Inventory Item", fileName = "[BEHAVIOUR] Inventory Item")]
    public class BehaviourTemplate_InventoryItem : BehaviourTemplate
    {
        /// <summary>
        /// The maximum amount of this Item that can be held by an entity
        /// </summary>
        public int max = 1;
        /// <summary>
        /// Whether the Item can be dropped.
        /// </summary>
        public bool canBeDropped = false;
        /// <summary>
        /// The Prefab to use if the Item is Dropped (if none is specified, the Item will simply be deleted).
        /// </summary>
        public CObject itemObject;

        public enum PickupType
        {
            Attach,
            Destroy
        }

        public PickupType pickupType = 0;


        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Behaviour.InventoryItem";
        }
    }
}
