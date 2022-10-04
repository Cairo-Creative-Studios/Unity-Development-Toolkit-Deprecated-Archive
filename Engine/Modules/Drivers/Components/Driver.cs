//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Reflection;
using CairoEngine.AssetScripting;
using NaughtyAttributes;

namespace CairoEngine.Drivers
{
    /// <summary>
    /// The Driver Driver MonoDriver is a Controller of other Components/MonoDrivers that have been attached to a Game Object. 
    /// </summary>
    [Serializable]
    public class Driver<T> : MonoBehaviour
    {
        /// <summary>
        /// Whether the Driver is enabled or not
        /// </summary>
        public bool Enabled = true;

        [Header(" - Driver - ")]
        /// <summary>
        /// The Driver Core that this Driver is a part of
        /// </summary>
        [Tooltip("The Driver Core that this Driver is a part of")]
        [Foldout("Properties")]
        public DriverCore core;
        /// <summary>
        /// The Driver Core's Template
        /// </summary>
        [Tooltip("The Driver's Template")]
        [Foldout("Properties")]
        public T template;

        /// <summary>
        /// The Transform of the Object within the Instanced Prefab to use for Movement
        /// </summary>
        [Tooltip("The Transform of the Object within the Instanced Prefab to use for Movement")]
        [Foldout("Components")]
        [HideInInspector] public Transform rootTransform;
        /// <summary>
        /// The Animator attached to the Object that communicates with the Driver
        /// </summary>
        [Tooltip("The Animator attached to the Object that communicates with the Driver")]
        [Foldout("Components")]
        public Animator animator;
        /// <summary>
        /// Asset Script Container, contains Events, Methods and Variables to interface with the Driver.
        /// </summary>
        [Tooltip("Asset Script Container, contains Events, Methods and Variables to interface with the Driver.")]
        [Foldout("Components")]
        public AssetScriptContainer scriptContainer;

        /// <summary>
        /// The Current Input Values of the Driver
        /// </summary>
        [Tooltip("The current Input Values of the Driver")]
        [Foldout("Controller")]
        public SDictionary<string, float> inputs = new SDictionary<string, float>();
        /// <summary>
        /// The Input translation Dictionary
        /// </summary>
        [Tooltip("The Input translation Dictionary")]
        [Foldout("Controller")]
        public SDictionary<string, string> inputTranslation = new SDictionary<string, string>();

        /// <summary>
        /// Whether to Enable the Audio for the Driver
        /// </summary>
        [Tooltip("Whether to Enable the Audio for the Driver")]
        [Foldout("Audio")]
        public bool enableAudio = false;
        /// <summary>
        /// The Table of Audio that the Driver uses
        /// </summary>
        [Tooltip("The Table of Audio that the Driver uses")]
        [Foldout("Audio")]
        public SDictionary<string, List<AudioClip>> audioTable = new SDictionary<string, List<AudioClip>>();

        public void AddInput(string inputBind, string inputName)
        {
            inputTranslation.Add(inputBind, inputName);
            inputs.Add(inputName, 0.0f);
        }

        /// <summary>
        /// Sets the Active state of the Game Object that this Driver is attached to
        /// </summary>
        /// <param name="active">If set to <c>true</c> active.</param>
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        /// <summary>
        /// Enable this instance.
        /// </summary>
        public void Enable()
        {
            Enabled = true;
        }

        /// <summary>
        /// Disable this instance.
        /// </summary>
        public void Disable()
        {
            Enabled = false;
        }

        /// <summary>
        /// Set the Input for the Driver
        /// </summary>
        /// <returns>The inputs.</returns>
        public virtual void SetInputs(SDictionary<string, float> newInputs)
        {
            string[] keys = { "" };
            Array.Resize<string>(ref keys, newInputs.Count);
            newInputs.Keys.CopyTo(keys, 0);

            foreach (string input in keys)
            {
                if (inputTranslation.ContainsKey(input))
                    if (inputs.ContainsKey(inputTranslation[input]))
                        inputs[inputTranslation[input]] = newInputs[input];
            }
        }

        /// <summary>
        /// Load Audio Defaults
        /// </summary>
        public void InitializeAudio()
        {
            audioTable = (SDictionary<string, List<AudioClip>>)template.GetField("audioTable");
            string audioList = (string)template.GetField("audioList");
            string audioPool = (string)template.GetField("audioPool");

            foreach (string clip in audioList.TokenArray())
            {
                if (audioTable[clip] == null)
                {
                    List<AudioClip> clipList = DriverModule.GetAudio(audioPool, clip);
                    if (clipList != null)
                        audioTable[clip] = clipList;
                }
            }
        }

        /// <summary>
        /// Plays an Audio Clip from the Driver's Audio Table
        /// </summary>
        /// <param name="clipList">Clip list.</param>
        public void PlayAudioClip(string clipList)
        {
            if (enableAudio)
                if (audioTable.ContainsKey(clipList))
                    MessageDriver<AudioSource>("PlayOneShot", new object[] { audioTable[clipList] });
        }

        /// <summary>
        /// Messages another Driver that is attached to the same CObject
        /// </summary>
        /// <param name="message">The Message to send.</param>
        /// <param name="parameters">Parameters passed with the Message.</param>
        /// <typeparam name="T1">The Type of the Driver to send the Message to.</typeparam>
        public object MessageDriver<T1>(string message, object[] parameters = null)
        {
            return DriverModule.Message<T1>(core, message, parameters);
        }

        /// <summary>
        /// Gets the value of the Input
        /// </summary>
        /// <returns>The input.</returns>
        /// <param name="input">Input.</param>
        private float GetInput(string input)
        {
            return inputs[input];
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public object GetProperty(string name)
        {
            return core.GetProperty(name);
        }

        public object SetProperty(string name, object value)
        {
            return core.SetProperty(name, value);
        }

        /// <summary>
        /// Sets the Value of an animation Property
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public void SetAnimationParameter(string name, object value)
        {
            DriverTemplate driverTemplate = (DriverTemplate)(object)template;

            if (animator != null && driverTemplate.driverProperties.animation.animationParameterMap.ContainsKey(name))
            {
                switch (value.GetType().Name)
                {
                    case "Boolean":
                        animator.SetBool(driverTemplate.driverProperties.animation.animationParameterMap[name].name, (bool)value);
                        break;
                    case "Float":
                        animator.SetFloat(driverTemplate.driverProperties.animation.animationParameterMap[name].name, ((float)value) * driverTemplate.driverProperties.animation.animationParameterMap[name].scale);
                        break;
                    case "Single":
                        animator.SetFloat(driverTemplate.driverProperties.animation.animationParameterMap[name].name, ((float)value) * driverTemplate.driverProperties.animation.animationParameterMap[name].scale);
                        break;
                }
            }
        }
    }
}
