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
        private static List<InputMap> inputMaps = new List<InputMap>();

        public static void Init()
        {
            //Preload all Input Maps in the Project
            inputMaps.AddRange(Resources.LoadAll<InputMap>(""));

            foreach(InputMap inputMap in inputMaps)
            {
                foreach (Input inputValue in inputMap.inputs.Values)
                {
                    inputValue.inputAction.Enable();
                }
            }
        }

        /// <summary>
        /// Gets the Value of an Input in an Input Map
        /// </summary>
        /// <returns>The input value.</returns>
        /// <param name="map">Map.</param>
        /// <param name="input">Input.</param>
        public static T GetInputValue<T>(string map, string input)
        {
            InputMap inputMap = GetInputMap(map);

            if (inputMap != null)
            {
                if (inputMap.inputs.ContainsKey(input))
                {
                    switch (typeof(T).Name)
                    {
                        case "Boolean":
                            return (T)(object)inputMap.inputs[input].inputAction.IsPressed();

                        case "String":
                            if (inputMap.inputs[input].inputAction.IsPressed())
                                return (T)(object)inputMap.inputs[input].inputAction.ReadValueAsObject().ToString();
                            else
                                return (T)(object)default(T).ToString();
                    }

                    if (inputMap.inputs[input].inputAction.IsPressed())
                    {
                        return (T)inputMap.inputs[input].inputAction.ReadValueAsObject();
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// Gets the specified Input Map.
        /// </summary>
        /// <returns>The input map.</returns>
        /// <param name="ID">Identifier.</param>
        private static InputMap GetInputMap(string ID)
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
