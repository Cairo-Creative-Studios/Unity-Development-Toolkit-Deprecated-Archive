using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// The Inventory Manager enables global control over Items that are added and used in the game by Entities.
    /// </summary>
    public class InventoryModule
    {
        /// <summary>
        /// The pool of Items that the Inventory Manager uses. 
        /// </summary>
        private static List<Item> itemPool = new List<Item>();

        /// <summary>
        /// The Item Infos loaded into Game.
        /// </summary>
        private static List<InventoryItemInfo> itemInfos = new List<InventoryItemInfo>();

        public static void Init()
        {
            itemInfos.AddRange(Resources.LoadAll<InventoryItemInfo>(""));
        }

        public static void Update()
        {

        }

        /// <summary>
        /// Creates an Item and adds it to the Item Pool
        /// </summary>
        /// <param name="itemName">Item name.</param>
        public static int CreateItem(string itemName)
        {
            itemPool.Add(new Item(GetItemInfo(itemName), itemPool.Count));
            return itemPool.Count;
        }

        /// <summary>
        /// Deletes an Item from the Item Pool, and removes it from the Owning Entity as well.
        /// </summary>
        /// <param name="IID">Item ID of the Item to Delete.</param>
        public static void DeleteItem(int IID)
        {
            itemPool[IID].owner.DeleteInventoryItem(itemPool[IID]);
            itemPool.RemoveAt(IID);
        }

        /// <summary>
        /// Gives a specific Item to an Entity
        /// </summary>
        /// <param name="to">The Entity to Give the Item to.</param>
        /// <param name="itemIID">The IID of the Item (Index in Item Pool).</param>
        public static void GiveItem(Entity to, int itemIID)
        {
            to.inventory.Add(itemPool[itemIID]);
        }

        /// <summary>
        /// Gives the Item with the Given ID to the Entity
        /// </summary>
        /// <param name="to">The Entity to give the Item to.</param>
        /// <param name="itemID">The name of the Item, given in Item Info.</param>
        public static void GiveNewItem(Entity to, string itemID)
        {
            GiveItem(to, CreateItem(itemID));
        }

        /// <summary>
        /// Gets the item info by it's ID.
        /// </summary>
        /// <param name="ID">Identifier.</param>
        private static InventoryItemInfo GetItemInfo(string ID)
        {
            foreach (InventoryItemInfo itemInfo in itemInfos)
            {
                if (itemInfo.ID == ID)
                {
                    return itemInfo;
                }
            }
            return null;
        }
    }
}
