//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [Serializable]
    public class StateContainer
    {
        public string behaviourName = "";
        public Dictionary<string, object> states = new Dictionary<string, object>();
        public List<object> stateInstances = new List<object>();
        public string curState = "";
        public string prevState = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behaviourName">The name of this MonoBehaviour.</param>
        /// <param name="states">List of States in the MonoBehavior class as StateInfo.</param>
        /// <param name="defaultState">The Default State (Set in Engine as the first Class in the script, unless otherwise specified).</param>
        public StateContainer(string behaviourName, Dictionary<string, object> states, string defaultState)
        {
            this.behaviourName = behaviourName;
            this.states = states;
            curState = defaultState;
        }
    }
}
