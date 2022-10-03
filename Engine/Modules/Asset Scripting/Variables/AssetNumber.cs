using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.AssetScripting
{
    [CreateAssetMenu(menuName = "Asset Scripting/Variables/Number")]
    public class AssetNumber : AssetVariableBase
    {
        public AssetVariable<float> variable;
    }
}
