using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDT.AssetScripting
{
    [CreateAssetMenu(menuName = "Asset Scripting/Variables/Object")]
    public class AssetObject : AssetVariableBase
    {
        public AssetVariable<object> variable;
    }
}
