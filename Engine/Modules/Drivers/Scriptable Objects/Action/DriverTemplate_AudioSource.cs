using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace CairoEngine.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Action/AudioSource", fileName = "[DRIVER] Audio Source")]
    public class DriverTemplate_AudioSource : DriverTemplate
    {
        [Header("")]
        [Header(" -- Audio Source -- ")]
        /// <summary>
        /// Audio Clips to use with the Audio Source
        /// </summary>
        [Tooltip("Audio Clips to use with the Audio Source")]
        [Foldout("Audio Source")]
        public SDictionary<string, List<AudioClip>> audioClips = new SDictionary<string, List<AudioClip>>();
        /// <summary>
        /// The Path to the Transform to attach the Audio Source to
        /// </summary>
        [Tooltip("The Path to the Transform to attach the Audio Source to")]
        [Foldout("Component Paths")]
        public string audioSourcePath = "";

        //Initialize the driver Class for this driver
        private void OnEnable()
        {
            this.driverClass = "CairoEngine.Drivers.AudoSource";

            foreach(string defaultEvent in "Played,Stopped".TokenArray())
            {
                if (!scriptContainer.output.ContainsKey(defaultEvent))
                    scriptContainer.output.Add(defaultEvent, null);
                if (!scriptContainer.events.ContainsKey(defaultEvent))
                    scriptContainer.events.Add(defaultEvent, null);
            }
        }
    }
}
