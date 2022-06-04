using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;
using CairoEngine.MachineLearning;
using CairoEngine.MachineLearning.Sensers.Children;

namespace CairoEngine.MachineLearning.Sensers
{
    public class Senser_ShiftGrid : Senser
    {
        SenserTemplate_ShiftGrid shiftGridInfo;
        List<GridUnit> gridUnits = new List<GridUnit>();

        int shift = 0;

        public override void Init()
        {
            shiftGridInfo = (SenserTemplate_ShiftGrid)info;

            int gridSize = shiftGridInfo.gridsize* shiftGridInfo.gridsize* shiftGridInfo.gridsize;

            //Build Grid Unit Dictionary
            for (int i = 0; i < gridSize; i++)
            {
                //Instantiate the default Grid Unit Object
                GameObject gridUnitObject = Object.Instantiate(shiftGridInfo.GridUnitPrefab);
                //Set the Parent of the Grid Unit Object to the Senser Object
                gridUnitObject.transform.parent = senserObject.transform;

                //Set the Position of the Grid Unit relative to the Senser Object's Transform
                Vector3 gridUnitPosition = new Vector3(Mathf.Round(i / 100)*shiftGridInfo.blockSize, Mathf.Round((i % 100) / 10) * shiftGridInfo.blockSize, Mathf.Round((i % 10) / 10) * shiftGridInfo.blockSize);
                gridUnitObject.transform.position = gridUnitPosition;

                //Add the Grid Unit Behaviour to the Grid Unit Object and add it to the List of Grid Units
                GridUnit gridUnit = gridUnitObject.AddComponent<GridUnit>();
                gridUnits.Add(gridUnit);
            }
        }

        public override void Sense()
        {
            inputs[inputNames["Shift"]] = Shift();
            Check();
        }

        /// <summary>
        /// Shifts the Grid
        /// </summary>
        /// <returns>The shift.</returns>
        double Shift()
        {
            shift++;
            shift = shift % shiftGridInfo.blockSize;

            Vector3 shiftPosition = new Vector3();
            shiftPosition.x = agentObject.transform.position.x - shiftGridInfo.blockSize/2 + shift/shiftGridInfo.blockSize;
            shiftPosition.y = agentObject.transform.position.y - shiftGridInfo.blockSize/2 + shift/shiftGridInfo.blockSize;
            shiftPosition.z = agentObject.transform.position.z - shiftGridInfo.blockSize/2 + shift/shiftGridInfo.blockSize;

            return (double)shift;
        }

        void Check()
        {
            int gridSize = shiftGridInfo.gridsize * shiftGridInfo.gridsize* shiftGridInfo.gridsize;
            for (int i = 0; i < gridSize; i++)
            {
                int collider = 0;
                foreach(string colliderTag in shiftGridInfo.tags)
                {
                    if(colliderTag == gridUnits[i].collidingObject.tag)
                    {
                        inputs[inputNames[GetGridInputName(i)]] = collider / shiftGridInfo.tags.Count;
                    }
                    collider++;
                }
            }
        }

        string GetGridInputName(int i)
        {
            return "" + Mathf.Round(i / 100) + "," + Mathf.Round((i % 100) / 10) + "," + Mathf.Round((i % 10) / 10);
        }
    }
}
