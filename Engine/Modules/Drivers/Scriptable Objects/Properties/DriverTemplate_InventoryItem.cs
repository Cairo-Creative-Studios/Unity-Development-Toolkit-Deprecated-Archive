using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace UDT.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Properties/Inventory Item", fileName = "[DRIVER] Inventory Item")]
    public class DriverTemplate_InventoryItem : DriverTemplate
    {
        [Header("")]
        [Header(" -- Inventory Item -- ")]
        /// <summary>
        /// Whether to Send Pick Up State Save Commands to the Saveable Script
        /// </summary>
        [Tooltip("Whether to Send Pick Up State Save Commands to the Saveable Script")]
        public bool persist = true;
        /// <summary>
        /// Objects with these Tags can Pick Up this Item
        /// </summary>
        [Tooltip("Objects with these Tags can Pick Up this Item")]
        [Foldout("Pickup")]
        public List<string> pickupTags = new List<string>();
        /// <summary>
        /// The maximum amount of this Item that can be held by an Object
        /// </summary>
        [Tooltip("The maximum amount of this Item that can be held by an Object")]
        [Foldout("Pickup")]
        public int max = 1;
        /// <summary>
        /// Whether to Destroy the Game Object of the Inventory Item when Picked Up
        /// </summary>
        [Tooltip("Whether to Destroy the Game Object of the Inventory Item when Picked Up")]
        public bool destroyOnPickup = true;
        /// <summary>
        /// Whether the Item can be dropped.
        /// </summary>
        [Tooltip("Whether the Item can be dropped.")]
        [Foldout("Drop")]
        public bool canBeDropped = false;
        /// <summary>
        /// Whether to use the Object's Prefab when the Item is dropped
        /// </summary>
        [Tooltip("Whether to use the Object's Prefab when the Item is dropped")]
        [Foldout("Drop")]
        public bool usePrefab = false;

        //Initialize the driver Class for this driver
        private void OnEnable()
        {
            this.driverProperties.main.driverClass = "UDT.Drivers.InventoryItem";
            SetScriptingEvents("Pickup,Putdown".TokenArray());
        }
    }
}
