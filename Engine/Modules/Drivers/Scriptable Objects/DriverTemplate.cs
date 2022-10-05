/*! \addtogroup drivermodule driver Template
 *  Additional documentation for group 'driver Module'
 *  @{
 */

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using UDT.AssetScripting;
using NaughtyAttributes;

namespace UDT.Drivers
{
    public class DriverTemplate : ScriptableObject
    {
        /// <summary>
        /// Container of Main Properties for the Driver
        /// </summary>
        [Serializable]
        public class MainInfo
        {
            /// <summary>
            /// The ID of the Driver Template, used for telling the different between multiple Drivers of the Same Type
            /// </summary>
            [Tooltip("The ID of the Driver Template, used for telling the different between multiple Drivers of the Same Type")]
            public string ID = "Default";
            /// <summary>
            /// The Class belonging to this driver
            /// </summary>
            [HideInInspector] public string driverClass = "";
            /// <summary>
            /// An Optional Prefab to spawn for drivers that generate their own Game Objects
            /// </summary>
            [Tooltip("An Optional Prefab to spawn for drivers that generate their own Game Objects")]
            public GameObject prefab;
        }
        /// <summary>
        /// Contain of Scripting Properties for the Driver
        /// </summary>
        [Serializable]
        public class ScriptingInfo
        {
            /// <summary>
            /// The Asset Script container to use with the Driver
            /// </summary>
            [Tooltip("The Asset Script container to use with the Driver")]
            public AssetScriptContainer scriptContainer;
        }
        /// <summary>
        /// Container of Input Properties for the Driver
        /// </summary>
        [Serializable]
        public class InputInfo
        {
            /// <summary>
            /// The Binds from the Controller sending inputs to the driver mapped to the driver's unique Inputs
            /// </summary>
            [Tooltip("The Binds from the Controller sending inputs to the driver mapped to the driver's unique Inputs")]
            public SDictionary<string, string> inputTranslation = new SDictionary<string, string>();
        }
        /// <summary>
        /// Container of Audio Properties for the Driver
        /// </summary>
        [Serializable]
        public class AudioInfo
        {
            /// <summary>
            /// The Audio Pool to associate this driver with. This will allow the driver to use sounds from other drivers within the pool, if this instance doesn't have a Sound it needs to perform for each action.
            /// </summary>
            [Tooltip("The Audio Pool to associate this driver with. This will allow the driver to use sounds from other drivers within the pool, if this instance doesn't have a Sound it needs to perform for each action.")]
            public string audioPool = "Weapons";
            /// <summary>
            /// The Table of sounds to use for this Behaivour. If any of the Audio Clip Lists are empty, the Engine will attempt to fill them with entries from the Behaivour Pool.
            /// </summary>
            [Tooltip("The Table of sounds to use for this Behaivour. If any of the Audio Clip Lists are empty, the Engine will attempt to fill them with entries from the Behaivour Pool.")]
            public SDictionary<string, List<AudioClip>> audioTable = new SDictionary<string, List<AudioClip>>();
            /// <summary>
            /// When true, when this driver is loaded it's Audio Table will be used as the Defaults for the Audio Pool.
            /// </summary>
            [Tooltip("When true, when this driver is loaded it's Audio Table will be used as the Defaults for the Audio Pool.")]
            public bool useAsPoolDefaults = true;
        }
        /// <summary>
        /// Container of Animation Properties for the Driver
        /// </summary>
        [Serializable]
        public class AnimationInfo
        {
            /// <summary>
            /// Animation Properties used by the Driver, mapped to the Properties within the Animator attached to the Game Object.
            /// </summary>
            [Tooltip("Animation Parameters used by the Driver, mapped to the Parameters within the Animator attached to the Game Object.")]
            public SDictionary<string, parameter> animationParameterMap = new SDictionary<string, parameter>();
        }
        /// <summary>
        /// The Base Driver Properties Container
        /// </summary>
        [Serializable]
        public class DriverProperties
        {
            [Header("The Base Properties for the Driver")]
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The Main Properties for the Driver
            /// </summary>
            [Tooltip("The Main Properties for the Driver")]
            public MainInfo main = new MainInfo();
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The Scripting Propertire for the Driver
            /// </summary>
            [Tooltip("The Scripting Properties for the Driver")]
            public ScriptingInfo scripting = new ScriptingInfo();
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The Input Properties for the Driver
            /// </summary>
            [Tooltip("The Input Properties for the Driver")]
            public InputInfo input = new InputInfo();
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The Audio Properties for the Driver
            /// </summary>
            [Tooltip("The Audio Properties for the Driver")]
            public AudioInfo audio = new AudioInfo();
            [HorizontalLine(color: EColor.Gray)]
            /// <summary>
            /// The Animation Properties for the Driver
            /// </summary>
            [Tooltip("The Animation Properties for the Driver")]
            public AnimationInfo animation = new AnimationInfo();
        }
        [HorizontalLine(color: EColor.White)]
        [Tooltip("The Base Properties of the Driver")]
        [BoxGroup("Driver")]
        public DriverProperties driverProperties;

        [Serializable]
        public class InputName
        {
            public string input = "";

            public InputName(string input)
            {
                this.input = input;
            }
        }

        public void SetInputTranslation(string[] inputs, string[] map)
        {
            int i = 0;
            foreach (string input in inputs)
            {
                if (!this.driverProperties.input.inputTranslation.ContainsKey(input))
                    this.driverProperties.input.inputTranslation.Add(input, map[i]);

                i++;
            }
        }

        public void SetAnimationParameters(string[] parameters)
        {
            foreach (string defaultAnimationProperty in parameters)
            {
                if (!driverProperties.animation.animationParameterMap.ContainsKey(defaultAnimationProperty))
                    driverProperties.animation.animationParameterMap.Add(defaultAnimationProperty, new parameter(defaultAnimationProperty));
            }
        }

        public void SetScriptingEvents(string[] events)
        {
            foreach (string defaultEvent in events)
            {
                if (!driverProperties.scripting.scriptContainer.output.ContainsKey(defaultEvent))
                    driverProperties.scripting.scriptContainer.output.Add(defaultEvent, null);
                if (!driverProperties.scripting.scriptContainer.events.ContainsKey(defaultEvent))
                    driverProperties.scripting.scriptContainer.events.Add(defaultEvent, null);
            }
        }

        /// <summary>
        /// Attempts to Get the Default Audio from the driver Module for the Pool given to this driver
        /// </summary>
        public void GetDefaultAudio()
        {
            foreach (string list in driverProperties.audio.audioTable.Keys)
            {
                if (driverProperties.audio.audioTable[list].Count < 1)
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
