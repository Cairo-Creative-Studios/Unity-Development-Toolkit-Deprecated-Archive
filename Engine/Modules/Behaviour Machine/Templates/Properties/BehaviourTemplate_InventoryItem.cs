﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Inventory Item", fileName = "[BEHAVIOUR] Inventory Item")]
    public class BehaviourTemplate_InventoryItem : BehaviourTemplate
    {
        [Header("")]
        [Header(" -- Inventory Item -- ")]
        [Header(" - Pickup - ")]
        /// <summary>
        /// Objects with these Tags can Pick Up this Item
        /// </summary>
        [Tooltip("Objects with these Tags can Pick Up this Item")]
        public List<string> pickupTags = new List<string>();
        /// <summary>
        /// The maximum amount of this Item that can be held by an Object
        /// </summary>
        [Tooltip("The maximum amount of this Item that can be held by an Object")]
        public int max = 1;
        [Header(" - Drop - ")]
        /// <summary>
        /// Whether the Item can be dropped.
        /// </summary>
        [Tooltip("Whether the Item can be dropped.")]
        public bool canBeDropped = false;
        /// <summary>
        /// Whether to use the Object's Prefab when the Item is dropped
        /// </summary>
        [Tooltip("Whether to use the Object's Prefab when the Item is dropped")]
        public bool usePrefab = false;

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Behaviour.InventoryItem";

            foreach (string defaultEvent in "Pickup,Putdown".TokenArray())
            {
                if (!scriptContainer.Output.ContainsKey(defaultEvent))
                    scriptContainer.Output.Add(defaultEvent, null);
            }
        }
    }
}
