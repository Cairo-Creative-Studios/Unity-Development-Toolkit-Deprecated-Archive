/**Base Class for all Spektor engine Objects**/
using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Behaviour;

namespace CairoEngine
{
    public class Object : MonoBehaviour
    {
        /// <summary>
        /// The ID of this Object
        /// </summary>
        public int OID = -1;
        /// <summary>
        /// The Level this Object is a part of.
        /// </summary>
        public Level level;
        /// <summary>
        /// The children of this Object.
        /// </summary>
        public List<GameObject> children = new List<GameObject>();
        /// <summary>
        /// The Behaviours that have been added to the Object.
        /// </summary>
        public List<BehaviourType> behaviours = new List<BehaviourType>();
    }

}
