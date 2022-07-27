using System;
using UnityEngine;

namespace CairoEngine.Behaviour
{
    public class BehaviourTemplate_NavMeshAgent : BehaviourTypeTemplate
    {
        [Tooltip("The Path to the Nav Mesh Agent within the Object's Prefab")]
        public string agentPath = "";

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Behaviour.NavMeshAgentBehaviour";
        }
    }
}
