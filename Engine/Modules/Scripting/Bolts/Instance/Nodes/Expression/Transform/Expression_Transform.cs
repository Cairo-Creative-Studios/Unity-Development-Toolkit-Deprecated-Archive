using System;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Scripting.Properties
{
    public class Expression_Transform : Expression<Transform>
    {
        public GameObject gameObject;

        public override Transform Get()
        {
            return gameObject.transform;
        }
    }
}
