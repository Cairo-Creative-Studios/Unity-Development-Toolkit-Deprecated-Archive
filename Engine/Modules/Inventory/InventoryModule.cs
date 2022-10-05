using System;
using System.Collections.Generic;
using UDT.SaveSystem;
using UDT.Drivers;
using UnityEngine;

namespace UDT.InventoryManagement
{
    public class InventoryModule : MonoBehaviour
    {
        public static InventoryModule singleton;
        public List<DriverTemplate_InventoryItem> defaultItems = new List<DriverTemplate_InventoryItem>();
        public List<InventoryItem> activeInventorItems = new List<InventoryItem>();
        public List<Inventory> activeInventories = new List<Inventory>();

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            singleton = new GameObject().AddComponent<InventoryModule>();
            singleton.gameObject.name = "Cairo Inventory Module";
            GameObject.DontDestroyOnLoad(singleton.gameObject);
        }

        /// <summary>
        /// Saves the contents of the Specified Inventory
        /// </summary>
        /// <param name="inventory">Inventory.</param>
        public static void Save(Inventory inventory)
        {
            SaveSystemModule.SetProperty("Inventory_" + inventory.template.driverProperties.main.ID, inventory.items);
        }

        /// <summary>
        /// Loads the contents of the Specified Inventory
        /// </summary>
        /// <param name="inventory">Inventory.</param>
        public static void Load(Inventory inventory)
        {
            inventory.items = (List<DriverTemplate_InventoryItem>)SaveSystemModule.GetProperty("Inventory_" + inventory.template.driverProperties.main.ID);
        }

        /// <summary>
        /// Loads the Default Items from Resources
        /// </summary>
        public static void LoadFromResources()
        {
            singleton.defaultItems.AddRange(Resources.LoadAll<DriverTemplate_InventoryItem>(""));
        }

        /// <summary>
        /// Give the Specified Inventory Item to the Specified Inventory
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="inventory">Inventory.</param>
        public static void Give(InventoryItem item, Inventory inventory)
        {
            inventory.Pickup(item);
            item.Pickup(inventory.core);
        }

        /// <summary>
        /// Adds an Inventory to the Module
        /// </summary>
        /// <param name="inventory">Inventory.</param>
        public static void AddInventory(Inventory inventory)
        {
            singleton.activeInventories.Add(inventory);
        }

        /// <summary>
        /// Adds an Inventory Item to the Module
        /// </summary>
        /// <param name="item">Item.</param>
        public static void AddItem(InventoryItem item)
        {
            singleton.activeInventorItems.Add(item);
        }
    }
}
