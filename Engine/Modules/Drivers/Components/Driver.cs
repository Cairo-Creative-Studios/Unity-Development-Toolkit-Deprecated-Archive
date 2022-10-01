//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Reflection;

namespace CairoEngine.Drivers
{
    /// <summary>
    /// The Behaviour Driver MonoBehaviour is a Controller of other Components/MonoBehaviours that have been attached to a Game Object. 
    /// </summary>
    [Serializable]
    public class Driver<T> : MonoBehaviour
    {
        /// <summary>
        /// Whether the Behaviour is enabled or not
        /// </summary>
        public bool Enabled = true;

        [Header(" - Behaviour - ")]
        /// <summary>
        /// The Driver Core that this Behaviour is a part of
        /// </summary>
        public DriverCore core;
        /// <summary>
        /// The Behaviour's Template
        /// </summary>
        public T template;

        /// <summary>
        /// The Transform of the Object within the Instanced Prefab to use for Movement
        /// </summary>
        [HideInInspector] public Transform rootTransform;

        [Tooltip("Asset Script Container, contains Events, Methods and Variables to interface with the Driver.")]
        public AssetScriptContainer scriptContainer;


	 	[Header(" - Input - ")]
        public SDictionary<string, float> inputs = new SDictionary<string, float>();
        public SDictionary<string, string> inputMap = new SDictionary<string, string>();

        [Header(" - Animation - ")]
        /// <summary>
        /// The Animator attached to the Object that communicates with the Behaviour
        /// </summary>
        [Tooltip("The Animator attached to the Object that communicates with the Behaviour")]
        public Animator animator;

        [Header(" - Audio - ")]
        [Tooltip("The Table of Audio that the Behaviour uses")]
        public SDictionary<string, List<AudioClip>> audioTable = new SDictionary<string, List<AudioClip>>();
        public bool enableAudio = false;

        public void AddInput(string inputBind, string inputName)
        {
            inputMap.Add(inputBind, inputName);
            inputs.Add(inputName, 0.0f);
        }

        /// <summary>
        /// Sets the Active state of the Game Object that this Behaviour is attached to
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
        /// Set the Input for the Behaviour
        /// </summary>
        /// <returns>The inputs.</returns>
        public virtual void SetInputs(SDictionary<string, float>  newInputs)
        {
            string[] keys = { "" };
            Array.Resize<string>(ref keys, newInputs.Count);
            newInputs.Keys.CopyTo(keys,0);

            foreach (string input in keys)
            {
                if(inputMap.ContainsKey(input))
                    if(inputs.ContainsKey(inputMap[input]))
                        inputs[inputMap[input]] = newInputs[input];
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
        /// Plays an Audio Clip from the Behaviour's Audio Table
        /// </summary>
        /// <param name="clipList">Clip list.</param>
        public void PlayAudioClip(string clipList)
        {
            if (enableAudio)
                if (audioTable.ContainsKey(clipList))
                    MessageBehaviour<AudioSource>("PlayOneShot", new object[] { audioTable[clipList] });
        }

        /// <summary>
        /// Messages another Behaviour that is attached to the same CObject
        /// </summary>
        /// <param name="message">The Message to send.</param>
        /// <param name="parameters">Parameters passed with the Message.</param>
        /// <typeparam name="T1">The Type of the Behaviour to send the Message to.</typeparam>
        public object MessageBehaviour<T1>(string message, object[] parameters = null)
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

			if (animator != null && driverTemplate.animationParameterMap.ContainsKey(name))
			{
				switch (value.GetType().Name)
				{
					case "Boolean":
						animator.SetBool(driverTemplate.animationParameterMap[name].name, (bool)value);
						break;
					case "Float":
						animator.SetFloat(driverTemplate.animationParameterMap[name].name, ((float)value) * driverTemplate.animationParameterMap[name].scale);
						break;
					case "Single":
						animator.SetFloat(driverTemplate.animationParameterMap[name].name, ((float)value) * driverTemplate.animationParameterMap[name].scale);
						break;
				}
			}
		}
	}
}
