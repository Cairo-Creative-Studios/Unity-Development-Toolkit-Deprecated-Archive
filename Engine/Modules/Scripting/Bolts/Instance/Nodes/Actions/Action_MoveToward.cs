using System;
using UnityEngine;

namespace CairoEngine.Scripting.Actions
{
    public class Action_MoveToward : Action
    {
        /// <summary>
        /// Moves the Transform toward a Position by an Amount
        /// </summary>
        public override void Perform()
        {
            fields[0].Get<Transform>().position = fields[0].Get<Transform>().position.Translate(fields[1].Get<Vector3>(), fields[2].Get<float>());
        }
    }
}
