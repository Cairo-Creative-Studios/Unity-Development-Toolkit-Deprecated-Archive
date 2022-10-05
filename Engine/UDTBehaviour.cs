using System;
using UDT.Reflection;
using UnityEngine;

namespace UDT
{
    /// <summary>
    /// Cairo Behaviours implement the Engine Initilized Event so they can schedule certain Start Up Tasks to call when the Engine has been Initialized
    /// </summary>
    public class UDTBehaviour : MonoBehaviour
    {
        void Awake()
        {
            Engine.EngineInitialized += EngineInitialized;
        }

        public virtual void EngineInitialized()
        {

        }
    }
}
