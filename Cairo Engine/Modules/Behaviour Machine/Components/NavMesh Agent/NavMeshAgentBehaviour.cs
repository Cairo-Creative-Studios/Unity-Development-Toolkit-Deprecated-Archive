using System;
using UnityEngine;
using UnityEngine.AI;

namespace CairoEngine.Behaviour
{
    public class NavMeshAgentBehaviour : BehaviourType<BehaviourTemplate_NavMeshAgent>
    {
        NavMeshAgent agent;

        public virtual void Init()
        {
            //Tries to get the Character Controller attached to the Object, otherwise Creates one
            if (!root.properties.ContainsKey("NavMeshAgent"))
            {
                root.properties.Add("NavMeshAgent", gameObject.transform.Find(template.agentPath).gameObject.GetComponent<CharacterController>());

                if (root.properties["NavMeshAgent"] == null)
                    root.properties["NavMeshAgent"] = gameObject.transform.Find(template.agentPath).gameObject.AddComponent<CharacterController>();
            }
            //Get a local reference
            agent = (NavMeshAgent)root.properties["characterController"];

        }

        public virtual void Update()
        {

        }

        public virtual void Move()
        {

        }
    }
}
