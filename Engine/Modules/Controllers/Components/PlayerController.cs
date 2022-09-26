//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Controllers
{
    public class PlayerController : Controller
    {
        public int player = -1;
        public SDictionary<string, Input> inputActions = new SDictionary<string, Input>();
    
        void Update()
        {
            foreach(string input in inputActions.Keys)
            {
                inputs[input] = inputActions[input].Get();
            }
        }
    }
}
