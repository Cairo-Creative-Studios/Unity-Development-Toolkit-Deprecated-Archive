/*! \addtogroup drivermodule driver Template
 *  Additional documentation for group 'driver Module'
 *  @{
 */

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.AssetScripting;
using NaughtyAttributes;

namespace CairoEngine.Drivers
{
    public class DriverTemplate : ScriptableObject
    {
		public string ID = "Default";

        /// <summary>
        /// The Class belonging to this driver
        /// </summary>
        [Foldout("Properties")]
        [HideInInspector] public string driverClass = "";
        /// <summary>
        /// An Optional Prefab to spawn for drivers that generate their own Game Objects
        /// </summary>
        [Tooltip("An Optional Prefab to spawn for drivers that generate their own Game Objects")]
        [Foldout("Properties")]
        public GameObject prefab;
        /// <summary>
        /// The Asset Script container to use with the Driver
        /// </summary>
        [Tooltip("The Asset Script container to use with the Driver")]
        [Foldout("Properties")]
        public AssetScriptContainer scriptContainer;

        /// <summary>
        /// The Binds from the Controller sending inputs to the driver mapped to the driver's unique Inputs
        /// </summary>
        [Tooltip("The Binds from the Controller sending inputs to the driver mapped to the driver's unique Inputs")]
        [Foldout("Input")]
        public SDictionary<string, string> inputTranslation = new SDictionary<string, string>();

        /// <summary>
        /// The Audio Pool to associate this driver with. This will allow the driver to use sounds from other drivers within the pool, if this instance doesn't have a Sound it needs to perform for each action.
        /// </summary>
        [Tooltip("The Audio Pool to associate this driver with. This will allow the driver to use sounds from other drivers within the pool, if this instance doesn't have a Sound it needs to perform for each action.")]
        [Foldout("Audio")]
        public string audioPool = "Weapons";
        /// <summary>
        /// The Table of sounds to use for this Behaivour. If any of the Audio Clip Lists are empty, the Engine will attempt to fill them with entries from the Behaivour Pool.
        /// </summary>
        [Tooltip("The Table of sounds to use for this Behaivour. If any of the Audio Clip Lists are empty, the Engine will attempt to fill them with entries from the Behaivour Pool.")]
        [Foldout("Audio")]
        public SDictionary<string, List<AudioClip>> audioTable = new SDictionary<string, List<AudioClip>>();
        /// <summary>
        /// When true, when this driver is loaded it's Audio Table will be used as the Defaults for the Audio Pool.
        /// </summary>
        [Tooltip("When true, when this driver is loaded it's Audio Table will be used as the Defaults for the Audio Pool.")]
        [Foldout("Audio")]
        public bool useAsPoolDefaults = true;

		/// <summary>
		/// Animation Properties used by the Driver, mapped to the Properties within the Animator attached to the Game Object.
		/// </summary>
		[Tooltip("Animation Parameters used by the Driver, mapped to the Parameters within the Animator attached to the Game Object.")]
        [Foldout("Animations")]
        public SDictionary<string, parameter> animationParameterMap = new SDictionary<string, parameter>();

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
                if (!inputTranslation.ContainsKey(input))
                    inputTranslation.Add(input, map[i]);

                i++;
            }
        }

        /// <summary>
        /// Attempts to Get the Default Audio from the driver Module for the Pool given to this driver
        /// </summary>
        public void GetDefaultAudio()
        {
            foreach (string list in audioTable.Keys)
            {
                if (audioTable[list].Count < 1)
                {
                    //audioTable[list] = DriverManager.GetAudio(audioPool, list);
                }
            }
        }

		[Serializable]
		public class parameter
		{
			public string name = "";
			public float scale = 1.0f;
			public parameter(string name)
			{
				this.name = name;
			}
		}
	}
}
