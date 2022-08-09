using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Scripting/Asset Variables/String")]
    public class AssetString : AssetVariableBase
    {
        public AssetVariable<string> value;
    }
}
