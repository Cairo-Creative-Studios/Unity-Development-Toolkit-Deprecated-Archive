//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Controllers
{
    public class Controller : MonoBehaviour
    {
        /// <summary>
        /// The Controller's Template
        /// </summary>
        public ControllerTemplate template;

        public List<object> possessedObjects = new List<object>();

        /// <summary>
        /// The Controller's Inputs and their current State.
        /// </summary>
        public SDictionary<string, float> inputs = new SDictionary<string, float>();

        /// <summary>
        /// A string representation of all the Values of the Inputs in the Controller
        /// </summary>
        public string inputString = "";

        /// <summary>
        /// Whether the Controller has already been Checked in or not
        /// </summary>
        public bool checkedIn = false;

        void Start()
        {
            if (!checkedIn)
                ControllerModule.CheckIn(this);
        }
    }
}
