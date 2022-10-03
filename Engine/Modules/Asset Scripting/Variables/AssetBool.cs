using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.AssetScripting
{
    [CreateAssetMenu(menuName = "Asset Scripting/Variables/Bool")]
    public class AssetBool : AssetVariableBase
    {
        public AssetVariable<bool> variable;
    }
}
