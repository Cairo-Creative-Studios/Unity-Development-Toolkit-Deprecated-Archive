using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class SpawnPoint : MonoBehaviour
    {
        public enum SpawnEntity
        {
            Player,
            Enemy,
            Character,
            Object
        }
        /// <summary>
        /// Determines what Entity Spawns here.
        /// </summary>
        [Tooltip("Determines what Entity Spawns here.")]
        public SpawnEntity spawnEntity = 0;
        /// <summary>
        /// Identifies which Team this spawner belongs to.
        /// </summary>
        [Tooltip("What Team spawns here. ")]
        public int team = 0;

        public void Start()
        {

        }
    }
}
