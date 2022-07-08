using System;
using UnityEngine;

namespace CairoEngine.Scripting.Conditions
{
    public class Condition_CompareTransformDistance : Condition
    {
        /// <summary>
        /// Gets the Distance between a Transform's position and a Vector3 (Which could also be the position of another Transform)
        /// </summary>
        public override bool Check()
        {
            Transform transform = fields[0].Get<Transform>();

            switch (fields[1].Get<string>())
            {
                case ">":
                    value = fields[3].Get<float>() > fields[0].Get<Transform>().position.Distance(fields[2].Get<Vector3>());
                    break;
                case ">=":
                    value = fields[3].Get<float>() >= fields[0].Get<Transform>().position.Distance(fields[2].Get<Vector3>()); ;
                    break;
                case "=":
                    value = fields[3].Get<float>() == fields[0].Get<Transform>().position.Distance(fields[2].Get<Vector3>());
                    break;
                case "<":
                    value = fields[3].Get<float>() < fields[0].Get<Transform>().position.Distance(fields[2].Get<Vector3>());
                    break;
                case "<=":
                    value = fields[3].Get<float>() <= fields[0].Get<Transform>().position.Distance(fields[2].Get<Vector3>());
                    break;
            }


            return base.Check();
        }
    }
}
