//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using CairoEngine.InventoryManagement;
using Homebrew;
using UnityEngine;

namespace CairoEngine.Drivers
{
    [Serializable]
    public class InventoryItem : Driver<DriverTemplate_InventoryItem>
    {
        /// <summary>
        /// The ID of the Owner of this Item
        /// </summary>
        [Tooltip("The ID of the Owner of this Item")]
        [Foldout("Properties")]
        public int ownerID = -1;
        /// <summary>
        /// Whether the Item has been Picked Up or not
        /// </summary>
        [Tooltip("Whether the Item has been Picked Up or not")]
        [Foldout("Properties")]
        public bool pickedUp = false;


        void Start()
        {
            InventoryModule.AddItem(this);
        }

        void Update()
        {
            if (pickedUp)
                Pickup(null);
        }

        void OnTriggerEnter(Collider collider)
        {
            GameObject colliderGameObject = collider.gameObject;
            DriverCore colliderCore = colliderGameObject.GetComponent<DriverCore>();
            Inventory inventory = colliderGameObject.GetComponent<Inventory>();

            if (colliderCore != null && inventory != null)
            {
                foreach (string selfTag in core.properties.tags)
                {
                    foreach (string otherTag in inventory.template.pickupTags)
                    {
                        if (selfTag == otherTag)
                        {
                            inventory.Pickup(this);
                            Pickup(inventory.core);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The Item has been Picked Up by an Entity.
        /// </summary>
        public virtual void Pickup(DriverCore holder)
        {
            pickedUp = true;

            if (template.persist)
                core.Message<Saveable>("Save", null);

            if (pickedUp && template.destroyOnPickup)
                GameObject.Destroy(gameObject);
        }

        /// <summary>
        /// Drops the Item
        /// </summary>
        public virtual void Drop()
        {

        }
    }
}
