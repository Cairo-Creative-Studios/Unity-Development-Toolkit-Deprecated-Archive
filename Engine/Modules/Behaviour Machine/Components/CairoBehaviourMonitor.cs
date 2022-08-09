using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Reflection;

namespace CairoEngine
{
    /// <summary>
    /// The Cairo Behaviour Monitor plugs in to the Unity Inspector so values of Behaviours can be viewed within the Inspector.
    /// </summary>
    public class CairoBehaviourMonitor
    {
        /// <summary>
        /// The Behaviour this Monitor is attached to.
        /// </summary>
        [HideInInspector]
        public object behaviour;

        /// <summary>
        /// Whether the Behaviour is enabled or not
        /// </summary>
        [Tooltip("Whether the Behaviour is enabled or not")]
        public bool Enabled = true;

        [Header(" - Behaviour - ")]
        /// <summary>
        /// The Game Object this Behaviour is Controlling
        /// </summary>
        [Tooltip("The Game Object this Behaviour is Controlling")]
        public GameObject gameObject;
        /// <summary>
        /// The Root Cairo Object of the Behaviour
        /// </summary>
        [Tooltip(" The Root Cairo Object of the Behaviour")]
        public CObject root;
        /// <summary>
        /// The Behaviour's Template
        /// </summary>
        [Tooltip("The Behaviour's Template")]
        public BehaviourTemplate template;

        [Header(" - Input - ")]
        /// <summary>
        /// Input Values Recieved from the Object that contains this Behaviour
        /// </summary>
        [Tooltip("Input Values Recieved from the Object that contains this Behaviour")]
        public SDictionary<string, float> inputs = new SDictionary<string, float>();
        /// <summary>
        /// Inputs Mapped from Unique Controller Input Names, to constant Input Names of the Behaviour
        /// </summary>
        [Tooltip("Inputs Mapped from Unique Controller Input Names, to constant Input Names of the Behaviour")]
        public SDictionary<string, string> inputMap = new SDictionary<string, string>();

        [Header(" - Animation - ")]
        /// <summary>
        /// The Animator attached to the Object that communicates with the Behaviour
        /// </summary>
        [Tooltip("The Animator attached to the Object that communicates with the Behaviour")]
        public Animator animator;

        [Header(" - Audio - ")]
        /// <summary>
        /// The Table of Audio that the Behaviour uses
        /// </summary>
        [Tooltip("The Table of Audio that the Behaviour uses")]
        public SDictionary<string, List<AudioClip>> audioTable = new SDictionary<string, List<AudioClip>>();
        /// <summary>
        /// Whether Audio is Enabled for this Behaviour
        /// </summary>
        [Tooltip("Whether Audio is Enabled for this Behaviour")]
        public bool enableAudio = false;

        /// <summary>
        /// The Asset Scripting Container allows game Logic to be built with Drag and Drop Variables and Methods, rather than with Code.
        /// </summary>
        [Tooltip("The Asset Scripting Container allows game Logic to be built with Drag and Drop Variables and Methods, rather than with Code.")]
        public AssetScriptContainer scriptContainer;

        void Update()
        {
            if(behaviour != null)
            {
                Enabled = (bool)behaviour.GetField("Enabled");
                gameObject = (GameObject)behaviour.GetField("gameObject");
                root = (CObject)behaviour.GetField("root");
                template = (BehaviourTemplate)behaviour.GetField("template");
                inputs = (SDictionary<string, float>)behaviour.GetField("inputs");
                inputMap = (SDictionary<string, string>)behaviour.GetField("inputMap");
                animator = (Animator)behaviour.GetField("animator");
                enableAudio = (bool)behaviour.GetField("enableAudio");
                scriptContainer = (AssetScriptContainer)behaviour.GetField("scriptContainer");
            }
        }
    }
}
