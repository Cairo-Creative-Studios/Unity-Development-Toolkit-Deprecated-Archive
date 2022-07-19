//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class BehaviourTypeTemplate : Resource
    {
        /// <summary>
        /// The Class belonging to this Behaviour
        /// </summary>
        [HideInInspector] public string behaviourClass = "";

        /// <summary>
        /// The Binds from the Controller sending inputs to the Behaviour mapped to the Behaviour's unique Inputs
        /// </summary>
        [Tooltip("The Binds from the Controller sending inputs to the Behaviour mapped to the Behaviour's unique Inputs")]
        public SDictionary<string, string> inputMap = new SDictionary<string, string>();

        [Tooltip("An Option Prefab to use for Preset Values")]
        public GameObject preset;

        [Serializable]
        public class InputName
        {
            public string input = "";

            public InputName(string input)
            {
                this.input = input;
            }
        }

        public void SetInputMap(string[] inputs, string[] map)
        {
            int i = 0;
            foreach (string input in inputs)
            {
                if (!inputMap.ContainsKey(input))
                    inputMap.Add(input, map[i]);

                i++;
            }
        }
    }
}
