using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Scripting
{
    [Serializable]
    ///<summary>
    ///A Trigger is like a Node, but is called at all times regardless of placement in Scripts 
    ///</summary>
    public class Trigger : Node
    {
        /// <summary>
        /// The Actions that are to be called if the Conditions are met
        /// </summary>
        public List<Action> actions = new List<Action>();
        /// <summary>
        /// The Properties that are being used for the Event
        /// </summary>
        public List<object> properties = new List<object>();

        public override void Init(Script script)
        {
            base.Init(script);
        }
    }
}
