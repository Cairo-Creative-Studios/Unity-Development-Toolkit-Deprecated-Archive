//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Reflection;

namespace CairoEngine.Behaviour
{
    /// <summary>
    /// The Behaviour Type class
    /// </summary>
    [Serializable]
    public class CairoBehaviour<T>
    {
        /// <summary>
        /// Whether the Behaviour is enabled or not
        /// </summary>
        public bool Enabled = true;

        [Header(" - Behaviour - ")]
        /// <summary>
        /// The Game Object this Behaviour is Controlling
        /// </summary>
        public GameObject gameObject;
        /// <summary>
        /// The Root Cairo Object of the Behaviour
        /// </summary>
        public CObject root;
        /// <summary>
        /// The Behaviour's Template
        /// </summary>
        public T template;

        /// <summary>
        /// The Transform of the Game Object this Behaviour is Controlling
        /// </summary>
        [HideInInspector] public Transform transform;
        /// <summary>
        /// The Transform of the Object within the Instanced Prefab to use for Movement
        /// </summary>
        [HideInInspector] public Transform rootTransform;

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

        /// <summary>
        /// The Asset Scripting Container allows game Logic to be built with Drag and Drop Variables and Methods, rather than with Code.
        /// </summary>
        [Tooltip("The Asset Scripting Container allows game Logic to be built with Drag and Drop Variables and Methods, rather than with Code.")]
        public AssetScriptContainer scriptContainer = new AssetScriptContainer();

        /// <summary>
        /// Updates the Core of the Behaviour, done once every tick
        /// </summary>
        public void CoreUpdate()
        {
            if (root == null)
                root = gameObject.GetComponent<CObject>();

            SetInputs();
        }

        public void AddInput(string inputBind, string inputName)
        {
            inputMap.Add(inputBind, inputName);
            inputs.Add(inputName, 0.0f);
        }

        /// <summary>
        /// Get a Component from the Game Object this Behaviour is attached to
        /// </summary>
        /// <returns>The component.</returns>
        /// <param name="componentName">Component name.</param>
        public object GetComponent(string componentName)
        {
            return gameObject.GetComponent(componentName);
        }

        /// <summary>
        /// Gets a List of Components from the Game Object this Behaviour is attached to
        /// </summary>
        /// <returns>The components.</returns>
        /// <param name="componentName">Component name.</param>
        public object[] GetComponents(string componentName)
        {
            return gameObject.GetComponents(Type.GetType(componentName));
        }

        /// <summary>
        /// Adds a Component to the Game Object this Behaviour is attached to
        /// </summary>
        /// <param name="componentName">Component name.</param>
        public void AddComponent(string componentName)
        {
            gameObject.AddComponent(Type.GetType(componentName));
        }

        /// <summary>
        /// Calls an Asset Method if it is set
        /// </summary>
        /// <param name="methodName">Method name.</param>
        public void Output(string methodName, object[] parameters)
        {
            BehaviourTemplate behaviourTemplate = (BehaviourTemplate)(object)template;
            behaviourTemplate.scriptContainer.Output[methodName].Call(parameters);
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
        /// Gets the Inputs from the parent Entity.
        /// </summary>
        /// <returns>The inputs.</returns>
        public virtual void SetInputs()
        {
            SDictionary<string, float> entityInputs = root.inputs;

            foreach (string input in entityInputs.Keys)
            {
                if(inputMap.ContainsKey(input))
                    if(inputs.ContainsKey(inputMap[input]))
                        inputs[inputMap[input]] = entityInputs[input];
            }
        }

        /// <summary>
        /// Load Audio Defaults
        /// </summary>
        /// <param name="audioList">Audio list.</param>
        public void InitializeAudio() 
        {
            audioTable = (SDictionary<string, List<AudioClip>>)template.GetField("audioTable");
            string audioList = (string)template.GetField("audioList");
            string audioPool = (string)template.GetField("audioPool");

            foreach (string clip in audioList.TokenArray())
            {
                if (audioTable[clip] == null)
                {
                    List<AudioClip> clipList = BehaviourModule.GetAudio(audioPool, clip);
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
            return BehaviourModule.Message<T>(root, message, parameters);
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
            return root.GetProperty(name);
        }

        public object SetProperty(string name, object value)
        {
            return root.SetProperty(name, value);
        }
    }
}
