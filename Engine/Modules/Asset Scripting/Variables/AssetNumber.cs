using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDT.AssetScripting
{
    [CreateAssetMenu(menuName = "Asset Scripting/Variables/Number")]
    public class AssetNumber : AssetVariableBase
    {
        public AssetVariable<float> variable;
    }
}
