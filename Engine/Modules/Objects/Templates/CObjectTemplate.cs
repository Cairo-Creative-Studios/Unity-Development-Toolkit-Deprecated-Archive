//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Object")]
    public class CObjectTemplate : Resource
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
        /// The Root Transform, used for Movement related behaviour if you want to seperate it from other objects within the Prefab.
        /// </summary>
        [Tooltip("The Root Transform, used for Movement related behaviour if you want to seperate it from other objects within the Prefab.")]
        public string rootPath = "";
        /// <summary>
        /// The Animator to use for Communications between the Object's Animations and Behaviours
        /// </summary>
        [Tooltip("The Animator to use for Communications between the Object's Animations and Behaviours")]
        public string animatorPath = "";

        /// <summary>
        /// The Behaviours to give to the Object when it's spawned
        /// </summary>
        [Tooltip("The Behaviours to give to the Object when it's Spawned.")]
        public List<BehaviourTemplate> behaviours = new List<BehaviourTemplate>();
        /// <summary>
        /// A Prefix to add the Object's that go within the same Object List in the Object Pool
        /// </summary>
        [Tooltip("The ID of the Object Pool that this Object belongs to")]
        public string poolID = "";
    }
}
