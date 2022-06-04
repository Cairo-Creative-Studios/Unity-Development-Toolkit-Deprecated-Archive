using System;
using UnityEngine;
using CairoEngine;
using CairoEngine.MachineLearning;
using System.Collections.Generic;

namespace CairoEngine.MachineLearning.Sensers
{
    [CreateAssetMenu(menuName = "CairoGame/Machine Learning/Sensers/Shift Grid")]
    public class SenserTemplate_ShiftGrid : SenserInfo
    {
        /// <summary>
        /// The size of the block.
        /// </summary>
        public int blockSize = 32;
        /// <summary>
        /// The Size of the Grid
        /// </summary>
        public int gridsize = 10;
        /// <summary>
        /// The tags to look out for with the Shift Grid
        /// </summary>
        public List<string> tags = new List<string>();
        /// <summary>
        /// The Game Object used for Grid Units
        /// </summary>
        public GameObject GridUnitPrefab;

        public override Dictionary<int, List<String>> GetInputs(int curInputs)
        {
            Dictionary<int, List<String>> result = new Dictionary<int, List<String>>();
            result.Add(curInputs, inputs);

            if (!result[curInputs].Contains("Shift"))
                result[curInputs].Add("Shift");
            int curGridSize = gridsize * gridsize * gridsize;
            for (int i = 0; i < curGridSize; i++)
            {
                if(!result[curInputs].Contains("" + Mathf.Round(i / 100) + "," + Mathf.Round((i % 100) / 10) + "," + (i % 10)))
                    result[curInputs].Add("" + Mathf.Round(i / 100) + "," + Mathf.Round((i % 100)/10) + "," + (i % 10));
            }

            return result;
        }
    }
}
