using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Scripting/Asset Variables/Object")]
    public class AssetObject : AssetVariableBase
    {
        public AssetVariable<object> value;
    }
}
