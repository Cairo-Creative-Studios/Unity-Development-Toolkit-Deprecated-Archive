//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class ObjectTemplate : Resource
    {
        /// <summary>
        /// The Prefab to Instantiate when this Object is Spawned
        /// </summary>
        public GameObject prefab;
        /// <summary>
        /// The Behaviours to give to the Object when it's spawned
        /// </summary>
        public List<BehaviourTypeTemplate> behaviours = new List<BehaviourTypeTemplate>();
        /// <summary>
        /// A Prefix to add the Object's that go within the same Object List in the Object Pool
        /// </summary>
        public string poolPrefix = "";
    }
}
