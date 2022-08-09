using System;
using UnityEngine;

namespace CairoEngine.Behaviour
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Nav Mesh Agent", fileName = "[BEHAVIOUR] Nav Mesh Agent")]
    public class BehaviourTemplate_NavMeshAgent : BehaviourTemplate
    {
        [Header("")]
        [Header(" -- Nav Mesh Agent -- ")]
        [Tooltip("The Path to the Nav Mesh Agent within the Object's Prefab")]
        public string agentPath = "";
        [Tooltip("The desired Range from the Target that the Agent wants to be in, if it's not in range it will keep following the Target")]
        public float range = 100f;

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Behaviour.NavMeshAgent";

            foreach (string defaultEvent in "Following,Stopped,Arrived".TokenArray())
            {
                if (!scriptContainer.Output.ContainsKey(defaultEvent))
                    scriptContainer.Output.Add(defaultEvent, null);
            }
        }
    }
}
