using System;
using NaughtyAttributes;
using UnityEngine;

namespace UDT.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Action/ANav Mesh Agent", fileName = "[DRIVER] Nav Mesh Agent")]
    public class DriverTemplate_NavMeshAgent : DriverTemplate
    {
        [Header("")]
        [Header(" -- Nav Mesh Agent -- ")]
        /// <summary>
        /// The desired Range from the Target that the Agent wants to be in, if it's not in range it will keep following the Target
        /// </summary>
        [Tooltip("The desired Range from the Target that the Agent wants to be in, if it's not in range it will keep following the Target")]
        [Foldout("Properties")]
        public float range = 100f;
        /// <summary>
        /// The Path to the Nav Mesh Agent within the Object's Prefab
        /// </summary>
        [Tooltip("The Path to the Nav Mesh Agent within the Object's Prefab")]
        [Foldout("Component Paths")]
        public string agentPath = "";


        //Initialize the driver Class for this driver
        private void OnEnable()
        {
            this.driverProperties.main.driverClass = "UDT.Drivers.NavMeshAgent";
            SetScriptingEvents("Following, Stopped, Arrived".TokenArray());
        }
    }
}
