//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using UnityEngine.AI;
using CairoEngine.StateMachine;
using CairoEngine.Reflection;
using NaughtyAttributes;

namespace CairoEngine.Drivers
{
    [Serializable]
    public class NavMeshAgent : Driver<DriverTemplate_NavMeshAgent>
    {
        Transform target;

        void Start()
        {
            //Validate the Nav Mesh Agent and Set the Local Reference
            core.navMeshAgent = (UnityEngine.AI.NavMeshAgent)ValidateComponentProperty("navMeshAgentComponent", template.agentPath, typeof(UnityEngine.AI.NavMeshAgent));
        }

        public virtual void Move()
        {
            //Try to Get the Nav Mesh Agent's Target
            target = (Transform)GetProperty("target");
            if (target != null)
                core.navMeshAgent.SetDestination(target.position);

            //Move to the Target
            core.Move(core.navMeshAgent.desiredVelocity, DriverCore.MoveMethod.NavMeshAgent);
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
