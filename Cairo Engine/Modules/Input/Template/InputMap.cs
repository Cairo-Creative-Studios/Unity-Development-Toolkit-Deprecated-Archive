using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Input Map")]
    public class InputMap : Resource
    {
        public SDictionary<string,Input> inputs = new SDictionary<string, Input>();
    }
}
