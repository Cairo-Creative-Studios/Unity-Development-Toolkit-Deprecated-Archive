using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Scripting/Asset Variables/Bool")]
    public class AssetBool : AssetVariableBase
    {
        public AssetVariable<bool> variable;
    }
}
