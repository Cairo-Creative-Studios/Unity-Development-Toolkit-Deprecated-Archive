﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.AssetScripting
{
    [CreateAssetMenu(menuName = "Asset Scripting/Variables/String")]
    public class AssetString : AssetVariableBase
    {
        public AssetVariable<string> variable;
    }
}
