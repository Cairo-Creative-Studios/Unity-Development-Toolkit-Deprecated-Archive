using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Behaviour
{
    [Serializable]
    public class InventoryItem
    {
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

        /// <summary>
        /// Drops the Item
        /// </summary>
        public virtual void Drop()
        {

        }
    }
}
