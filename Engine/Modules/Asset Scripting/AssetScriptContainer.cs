using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CairoEngine
{
    ///<summary>
    /// The Asset Script Container holds all Asset based Scripting information within it, so the Scripting information can be Serialized and shared easily.
    ///</summary>
    [Serializable]
    public class AssetScriptContainer
    {
        /// <summary>
        /// Asset Variables contained within the Instance.
        /// </summary>
        [Tooltip("Asset Variables contained within the Instance.")]
        public List<AssetVariableBase> Variables = new List<AssetVariableBase>();
        /// <summary>
        /// The input.
        /// </summary>
        [Tooltip("Asset Events called within the Instance.")]
        public List<AssetMethod> Input = new List<AssetMethod>();
        /// <summary>
        /// Asset Events called by the Behaviour
        /// </summary>
        [Tooltip("Asset Events called by the Instance.")]
        public SDictionary<string, AssetMethod> Output = new SDictionary<string, AssetMethod>();
        /// <summary>
        /// Unity Events called by the Instance
        /// </summary>
        [Tooltip("Unity Events called by the Instance")]
        public SDictionary<string, UnityEvent> Events = new SDictionary<string, UnityEvent>();
    }
}
