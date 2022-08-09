using System;
using UnityEngine;

namespace CairoEngine.StateMachine
{
    public class StateMachineComponent : MonoBehaviour
    {
        [Tooltip("The Machine attached to this Object")]
        [HideInInspector] public Machine machine;
        [Tooltip("The Class for which this State Machine Viewer is Monitoring")]
        [ReadOnly] public string watching = "";
        [Tooltip("The current State as a Comma Seperated List")]
        public string currentState = "";
    }
}
