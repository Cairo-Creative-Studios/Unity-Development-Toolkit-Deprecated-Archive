using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class WorldInventoryItem : MonoBehaviour
    {
        /// <summary>
        /// The Inventory Item info for this Item.
        /// </summary>
        public Item item;

        /// <summary>
        /// The ID of the Owner of this Item
        /// </summary>
        public int ownerID = -1;

        /// <summary>
        /// The Item has been Picked Up by an Entity.
        /// </summary>
        public virtual void Pickup(object holder)
        {

        }
    }
}
