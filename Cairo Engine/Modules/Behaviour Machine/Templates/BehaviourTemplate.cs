//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class BehaviourTemplate : Resource
    {
        /// <summary>
        /// The Class belonging to this Behaviour
        /// </summary>
        [HideInInspector] public string behaviourClass = "";

        [Header(" - Default Behaviour Values - ")]
        /// <summary>
        /// The Binds from the Controller sending inputs to the Behaviour mapped to the Behaviour's unique Inputs
        /// </summary>
        [Tooltip("The Binds from the Controller sending inputs to the Behaviour mapped to the Behaviour's unique Inputs")]
        public SDictionary<string, string> inputMap = new SDictionary<string, string>();
        /// <summary>
        /// An Optional Prefab to spawn for Behaviours that generate their own Game Objects
        /// </summary>
        [Tooltip("An Optional Prefab to spawn for Behaviours that generate their own Game Objects")]
        public GameObject preset;

        [Header(" - Sounds - ")]
        /// <summary>
        /// The Audio Pool to associate this Behaviour with. This will allow the Behaviour to use sounds from other Behaviours within the pool, if this instance doesn't have a Sound it needs to perform for each action.
        /// </summary>
        [Tooltip("The Audio Pool to associate this Behaviour with. This will allow the Behaviour to use sounds from other Behaviours within the pool, if this instance doesn't have a Sound it needs to perform for each action.")]
        public string audioPool = "Weapons";
        /// <summary>
        /// The Table of sounds to use for this Behaivour. If any of the Audio Clip Lists are empty, the Engine will attempt to fill them with entries from the Behaivour Pool.
        /// </summary>
        [Tooltip("The Table of sounds to use for this Behaivour. If any of the Audio Clip Lists are empty, the Engine will attempt to fill them with entries from the Behaivour Pool.")]
        public SDictionary<string, List<AudioClip>> audioTable = new SDictionary<string, List<AudioClip>>();
        /// <summary>
        /// When true, when this Behaviour is loaded it's Audio Table will be used as the Defaults for the Audio Pool.
        /// </summary>
        [Tooltip("When true, when this Behaviour is loaded it's Audio Table will be used as the Defaults for the Audio Pool.")]
        public bool useAsPoolDefaults = true;

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

        /// <summary>
        /// Attempts to Get the Default Audio from the Behaviour Module for the Pool given to this Behaviour
        /// </summary>
        public void GetDefaultAudio()
        {
            foreach(string list in audioTable.Keys)
            {
                if (audioTable[list].Count < 1)
                {
                    audioTable[list] = BehaviourModule.GetAudio(audioPool, list);
                }
            }
        }
    }
}
