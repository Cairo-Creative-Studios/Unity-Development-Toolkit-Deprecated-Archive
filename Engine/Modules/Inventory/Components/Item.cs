using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class Item
    {
        /// <summary>
        /// The Item Data
        /// </summary>
        public InventoryItemTemplate info;
        /// <summary>
        /// The Item's Placement in the Item Pool
        /// </summary>
        public int IID = 0;
        /// <summary>
        /// Whether the Item is a Game Object or not
        /// </summary>
        public bool isGameObject = false;
        /// <summary>
        /// The World Inventory Item Behaviour attached to the Game Object.
        /// </summary>
        public WorldInventoryItem worldInventoryItem;
        /// <summary>
        /// The owning Entity of this Item.
        /// </summary>
        public Entity owner;

        public Item(InventoryItemTemplate info, int IID)
        {
            this.info = info;
            this.IID = IID;
        }
    }
}
