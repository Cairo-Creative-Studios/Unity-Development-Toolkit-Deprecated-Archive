//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using UnityEngine.AI;
using CairoEngine.StateMachine;
using CairoEngine.Reflection;

namespace CairoEngine.Drivers
{
    [Serializable]
    public class NavMeshAgent : Driver<DriverTemplate_NavMeshAgent>
    {
        UnityEngine.AI.NavMeshAgent agent;
        Transform target;
        NavMeshPath path;

        void Start()
        {
            //Tries to get the Character Controller attached to the Object, otherwise Creates one
            if (GetProperty("navMeshAgentComponent")==null)
            {
                SetProperty("navMeshAgentComponent", gameObject.transform.Find(template.agentPath).gameObject.GetComponent<UnityEngine.CharacterController>());

                if (GetProperty("navMeshAgentComponent") == null)
                    SetProperty("navMeshAgentComponent", gameObject.transform.Find(template.agentPath).gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>());
            }
            //Get a local reference
            agent = (UnityEngine.AI.NavMeshAgent)GetProperty("navMeshAgentComponent");
        }


        public virtual void Move()
        {
            //If the Target property exists, Move
            if (core.properties.ContainsKey("target"))
            {
                target = (Transform)core.properties["target"];

                agent.SetDestination(target.position);

                //If the Object contains a Character Controller, pass movement on to the Character Controller instead of the NavMeshAgent
                if (core.properties.ContainsKey("characterController"))
                {
                    GetProperty("characterController").CallMethod("Move", new object[] { (agent.desiredVelocity * Time.deltaTime) });
                }
                else
                {
                    agent.Move(agent.desiredVelocity * Time.deltaTime);
                }
            }
        }

        /// <summary>
        /// Follows the current Target
        /// </summary>
        public class Follow : State<NavMeshAgent>
        {
            public virtual void Update()
            {
                root.Move();
            }
        }

        /// <summary>
        /// Stops Following the Target
        /// </summary>
        public class StopFollow : State<NavMeshAgent>
        {

        }
    }
}
