using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.AssetScripting
{
    [CreateAssetMenu(menuName = "Asset Scripting/Variables/Object")]
    public class AssetObject : AssetVariableBase
    {
        public AssetVariable<object> variable;
    }
}
