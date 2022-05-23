using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [Serializable]
    public class StateContainer
    {
        public string behaviourName = "";
        public Dictionary<string, State> states = new Dictionary<string, State>();
        public List<State> stateInstances = new List<State>();
        public string curState = "";
        public string prevState = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behaviourName">The name of this MonoBehaviour.</param>
        /// <param name="states">List of States in the MonoBehavior class as StateInfo.</param>
        /// <param name="defaultState">The Default State (Set in Engine as the first Class in the script, unless otherwise specified).</param>
        public StateContainer(string behaviourName, Dictionary<string, State> states, string defaultState)
        {
            this.behaviourName = behaviourName;
            this.states = states;
            curState = defaultState;
        }
    }
}
