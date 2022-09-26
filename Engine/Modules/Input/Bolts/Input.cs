//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


#if ENABLE_INPUT_SYSTEM
namespace CairoEngine
{
    [Serializable]
    public class Input
    {
        public InputAction inputAction;

        public float Get()
        {
            float returnValue = 0;

            inputAction.Enable();

            returnValue = inputAction.ReadValue<float>();

            return returnValue;
        }
    }
}
#elif ENABLE_LEGACY_INPUT_MANAGER
namespace CairoEngine
{
    public class Input
    {
        public enum Device
        {
            Keyboard,
            Mouse,
            Gamepad,
            Simulated
        }
        public Device device; 

        public T Get<T>()
        {
            T returnValue = default(T);


            return returnValue;
        }

        /// <summary>
        /// Sets the Input Value Manually
        /// </summary>
        public void Simulate()
        {

        }

        public void Enable() 
        {
        
}
    }
}
#endif