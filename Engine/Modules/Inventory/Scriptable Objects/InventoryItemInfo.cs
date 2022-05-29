using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "CairoGame/Inventory/Basic Item")]
    public class InventoryItemInfo : ScriptableObject
    {
        /// <summary>
        /// The ID of this Inventory Item
        /// </summary>
        public string ID = "Test Inventory Item";
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
