//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CairoEngine
{
    public class PlayerController : Controller
    {
        public int player = -1;
        public SDictionary<string, InputAction> inputActions = new SDictionary<string, InputAction>();
    
        void Update()
        {
            foreach(string input in inputActions.Keys)
            {
                inputs[input] = inputActions[input].ReadValue<float>();
            }
        }
    }
}
