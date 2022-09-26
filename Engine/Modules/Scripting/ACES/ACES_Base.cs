//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Scripting
{
    /// <summary>
    /// Base Class for all ACES Nodes
    /// </summary>
    public class ACES_Base
    {
        /// <summary>
        /// The Event sheet that contains this ACES Node
        /// </summary>
        public EventSheet eventSheet;
        /// <summary>
        /// The Parent Node Class/Event of this ACES Node
        /// </summary>
        public Block block;
        /// <summary>
        /// Types that the ACES Script is to be Used for.
        /// </summary>
        public List<Type> types;
        /// <summary>
        /// The Parameters of the Node
        /// </summary>
        public SDictionary<string, object> parameters = new SDictionary<string, object>();

        [RuntimeInitializeOnLoadMethod]
        public virtual void Init()
        {
            ScriptModule.AddAces(this);
        }
    }
}
