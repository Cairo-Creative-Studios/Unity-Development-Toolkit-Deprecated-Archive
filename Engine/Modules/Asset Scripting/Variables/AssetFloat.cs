using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Scripting/Asset Variables/Float")]
    public class AssetFloat : AssetVariableBase
    {
        public AssetVariable<float> variable;
    }
}
