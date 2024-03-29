﻿using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace UDT.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Action/APin", fileName = "[DRIVER] Pin")]
    public class DriverTemplate_Pin : DriverTemplate
    {
        [Header("")]
        [Header(" -- Audio Source -- ")]
        /// <summary>
        /// Clips used by the Audio Source
        /// </summary>
        [Tooltip("Clips used by the Audio Source")]
        [Foldout("Properties")]
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
            this.driverProperties.main.driverClass = "UDT.Drivers.AudoSource";
            SetScriptingEvents("Played,Stopped".TokenArray());
        }
    }
}
