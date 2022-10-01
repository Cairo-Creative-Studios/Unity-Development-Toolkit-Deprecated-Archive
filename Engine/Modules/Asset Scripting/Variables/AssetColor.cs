using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Scripting/Asset Variables/Color")]
    public class AssetColor : AssetVariableBase
    {
        public AssetVariable<Color> variable;
    }
}
