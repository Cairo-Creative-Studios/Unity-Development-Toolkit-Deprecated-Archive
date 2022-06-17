using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Entities/Entity")]

    [ExecuteAlways]
    /// <summary>
    /// Base Entity info. This Scriptable Object class is a Template for classes that extend from the Entity Class.
    /// </summary>
    public class EntityTemplate : ScriptableObject
    {
        /// <summary>
        /// The ID of this Entity, used for Picking
        /// </summary>
        public string ID = "";
        /// <summary>
        /// When enabled, The Scriptable Object will Generate a new GameObject.
        /// </summary>
        public bool createPrefab;
        /// <summary>
        /// The Name of the Class for this Entity, used for Generating the Prefab
        /// </summary>
        public string entityClassName = "";
        /// <summary>
        /// The Entity Prefab (The Game Object that should be instantiated when the Entity is Spawned)
        /// </summary>
        public GameObject prefab;
        /// <summary>
        /// The amount of Health the Entity should start with
        /// </summary>
        public float health = 100.0f;
        /// <summary>
        /// The Inventory to give to the Entity once it spawns into the game.
        /// </summary>
        public List<InventoryItemTemplate> defaultInventory = new List<InventoryItemTemplate>();
    }
}