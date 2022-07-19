//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;
using CairoEngine.Behaviour.TwoPointFive;

namespace CairoEngine.Behaviour
{
    public class TwoPointFiveRenderer : BehaviourType
    {
        private List<TwoPointFive.Joint> joints = new List<TwoPointFive.Joint>();
        private TwoPointFive.Renderer renderObject;

        public void Update()
        {
        }
    }
}
