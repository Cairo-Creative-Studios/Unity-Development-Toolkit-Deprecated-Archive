using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class InputModule
    {
        /// <summary>
        /// All the Input Maps preloaded for the Project
        /// </summary>
        private List<InputMap> inputMaps = new List<InputMap>();

        public void Init()
        {
            //Preload all Input Maps in the Project
            inputMaps.AddRange(Resources.LoadAll<InputMap>(""));
        }

        /// <summary>
        /// Gets the Value of an Input in an Input Map
        /// </summary>
        /// <returns>The input value.</returns>
        /// <param name="map">Map.</param>
        /// <param name="input">Input.</param>
        public object GetInputValue(string map, string input)
        {
            InputMap inputMap = GetInputMap(map);

            if (inputMap != null)
                return inputMap.inputs[input];

            return null;
        }

        /// <summary>
        /// Gets the specified Input Map.
        /// </summary>
        /// <returns>The input map.</returns>
        /// <param name="ID">Identifier.</param>
        private InputMap GetInputMap(string ID)
        {
            foreach(InputMap inputMap in inputMaps)
            {
                if (inputMap.ID == ID)
                    return inputMap;
            }
            return null;
        }
    }
}
