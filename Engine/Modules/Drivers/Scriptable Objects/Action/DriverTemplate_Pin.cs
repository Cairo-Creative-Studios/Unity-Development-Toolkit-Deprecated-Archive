using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Drivers
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Pin", fileName = "[BEHAVIOUR] Pin")]
    public class DriverTemplate_Pin : DriverTemplate
    {
        [Header("")]
        [Header(" -- Audio Source -- ")]
        [Tooltip("The Path to the Transform to attach the Audio Source to")]
        public string audioSourcePath = "";

        public SDictionary<string, List<AudioClip>> audioClips = new SDictionary<string, List<AudioClip>>();

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Drivers.AudoSource";

            foreach (string defaultEvent in "Played,Stopped".TokenArray())
            {
                if (!scriptContainer.Output.ContainsKey(defaultEvent))
                    scriptContainer.Output.Add(defaultEvent, null);
            }
        }
    }
}
