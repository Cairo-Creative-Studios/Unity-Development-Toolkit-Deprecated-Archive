using System;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Behaviour
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Cameras/Third Person")]
    public class BehaviourTemplate_ThirdPersonCamera : BehaviourTypeTemplate
    {
        [Tooltip("Time it takes for the Camera to recenter on the X axis (Recenters behind the Root Object)")]
        public float resetXTime = 2f;

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Behaviour.ThirdPersonCameraBehaviour";

            SetInputMap(new string[] { "XAxis", "YAxis" }, new string[] { "LookHorizontal", "LookVertical" });
        }
    }
}
