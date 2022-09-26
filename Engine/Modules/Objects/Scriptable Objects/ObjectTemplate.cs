//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Objects
{
    [CreateAssetMenu(menuName = "Cairo Game/Object")]
    public class ObjectTemplate : ScriptableObject
    {
        /// <summary>
        /// The Tags for this Object
        /// </summary>
        [Tooltip("The Tags for this Object")]
        public List<string> tags = new List<string>();
        /// <summary>
        /// Saves this Object's State with the Level, so it will return to the State when returning to the Level
        /// </summary>
        [Tooltip("Saves this Object's State with the Level, so it will return to the State when returning to the Level")]
        public bool persist = false;
        /// <summary>
        /// The Prefab to Instantiate when this Object is Spawned
        /// </summary>
        [Tooltip("The Prefab to generate with this Object")]
        public GameObject prefab;
        /// <summary>
        /// A Prefix to add the Object's that go within the same Object List in the Object Pool
        /// </summary>
        [Tooltip("The ID of the Object Pool that this Object belongs to")]
        public string poolID = "";
    }
}
