//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using UnityEngine;

namespace UDT.Scripting
{
    /// <summary>
    /// Moves the Selected Objects forward by the given Amount
    /// </summary>
    public class Action_MoveForward : ACES_Base
    {
        public override void Init()
        {
            types.Add(typeof(GameObject));
            types.Add(typeof(Transform));

            parameters.Add("Amount", 0);

            base.Init();
        }

        public void Perform()
        {
            foreach (GameObject instance in block.selectedObjects)
            {
                instance.transform.position += instance.transform.forward * (float)parameters["Amount"];
            }
        }
    }
}
