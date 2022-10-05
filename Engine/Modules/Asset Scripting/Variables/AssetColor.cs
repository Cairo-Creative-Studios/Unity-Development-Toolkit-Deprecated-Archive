using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDT.AssetScripting
{
    [CreateAssetMenu(menuName = "Asset Scripting/Variables/Color")]
    public class AssetColor : AssetVariableBase
    {
        public AssetVariable<Color> variable;
    }
}
