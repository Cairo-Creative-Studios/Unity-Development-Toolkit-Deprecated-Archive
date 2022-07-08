//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Behaviour
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Car Controller")]
    public class BehaviourTemplate_CarController : BehaviourTypeTemplate
    {
        public float maxSpeed = 300;
        public float acceleration = 0.1f;
        public float decceleration = 0.2f;
        public int gears = 5;
        public List<int> axles;


        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Behaviour.CarControllerBehaviour";

            SetInputMap(new string[] { "Steer", "Accel", "Brake"}, new string[] { "Driver_MoveHorizontal", "Driver_LeftZAxis", "Driver_RightZAxis"});
        }
    }
}
