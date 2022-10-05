//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using UDT;
using UDT.MachineLearning;
using System.Collections.Generic;

namespace UDT.MachineLearning.Sensers
{
    [CreateAssetMenu(menuName = "Cairo Game/Machine Learning/Sensers/Shift Grid")]
    public class SenserTemplate_ShiftGrid : SenserTemplate
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

        [HideInInspector] public GameObject generatedGrid;

        public override List<string> GetInputs(List<string> curInputs)
        {
            List<string> result = new List<string>();
            result.AddRange(curInputs.GetRange(0, inputs.Count));
            result.AddRange(inputs.GetRange(0, inputs.Count));

            if (!result.Contains("Shift"))
                result.Add("Shift");
            int curGridSize = gridsize * gridsize * gridsize;
            for (int i = 0; i < curGridSize; i++)
            {
                if (!result.Contains("" + Mathf.Round(i / 100) + "," + Mathf.Round((i % 100) / 10) + "," + (i % 10)))
                    result.Add("" + Mathf.Round(i / 100) + "," + Mathf.Round((i % 100) / 10) + "," + (i % 10));
            }

            return result;
        }
    }
}
