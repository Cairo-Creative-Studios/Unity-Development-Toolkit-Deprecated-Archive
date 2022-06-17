using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Inventory/Basic Item")]
    public class InventoryItemTemplate : ObjectTemplate
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
        public GameObject droppedPrefab;

        public enum PickupType
        {
            Attach,
            Destroy
        }

        public PickupType pickupType = 0;
    }
}
