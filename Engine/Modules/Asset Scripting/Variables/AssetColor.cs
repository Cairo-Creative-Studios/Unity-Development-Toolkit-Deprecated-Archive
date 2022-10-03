using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.AssetScripting
{
    [CreateAssetMenu(menuName = "Asset Scripting/Variables/Color")]
    public class AssetColor : AssetVariableBase
    {
        public AssetVariable<Color> variable;
    }
}
