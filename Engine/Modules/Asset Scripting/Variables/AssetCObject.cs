using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Scripting/Asset Variables/Cairo Object")]
    public class AssetCObject : AssetVariableBase
    {
        public AssetVariable<CObject> value;
    }
}
