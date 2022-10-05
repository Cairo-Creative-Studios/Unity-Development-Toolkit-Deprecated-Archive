using System;
using System.Collections.Generic;
using UDT.InventoryManagement;
using NaughtyAttributes;
using UnityEngine;

namespace UDT.Drivers
{
    public class Inventory : Driver<DriverTemplate_Inventory>
    {
        /// <summary>
        /// The Items in the Inventory
        /// </summary>
        [Tooltip("The Items in the Inventory")]
        [Foldout("Properties")]
        public List<DriverTemplate_InventoryItem> items = new List<DriverTemplate_InventoryItem>();

        void Start()
        {
            InventoryModule.AddInventory(this);
        }

        public void Pickup(InventoryItem item)
        {
            items.Add(item.template);
            item.Pickup(core);
        }
    }
}
