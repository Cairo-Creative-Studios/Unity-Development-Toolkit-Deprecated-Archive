using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Scripting;

namespace CairoEngine
{
    public class ScriptInstance : MonoBehaviour
    {
        /// <summary>
        /// The Scripts attached to this Object
        /// </summary>
        public List<Script> scripts = new List<Script>();

        /// <summary>
        /// The Properties of this Object
        /// </summary>
        public List<object> properties = new List<object>();

        void Start()
        {
            foreach(Script script in scripts)
            {
                script.Init();
            }
        }

        void Update()
        {
            foreach(Script script in scripts)
            {
                script.Tick();
            }
        }
    }
}
