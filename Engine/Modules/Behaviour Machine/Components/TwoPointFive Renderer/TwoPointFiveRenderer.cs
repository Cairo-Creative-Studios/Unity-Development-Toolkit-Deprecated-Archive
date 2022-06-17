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

        public override void BehaviourUpdate()
        {
            base.BehaviourUpdate();
        }
    }
}
