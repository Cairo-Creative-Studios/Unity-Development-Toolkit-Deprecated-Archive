using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Behaviour
{
    /// <summary>
    /// The Behaviour Type class
    /// </summary>
    public class BehaviourType : MonoBehaviour
    {
        /// <summary>
        /// The Behaviour's Template
        /// </summary>
        public BehaviourTypeTemplate template;
        /// <summary>
        /// The Inputs communicated to the Behaviour
        /// </summary>
        public SerializableDictionary<string, double> inputs = new SerializableDictionary<string, double>();

        /// <summary>
        /// Initialize the Behaviour
        /// </summary>
        public virtual void Init()
        {

        }

        /// <summary>
        /// Update the Behaviour
        /// </summary>
        public virtual void BehaviourUpdate()
        {

        }
    }
}
