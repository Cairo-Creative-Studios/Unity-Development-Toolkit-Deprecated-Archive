using System;
using UnityEngine;
using B83.Unity.Attributes;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Runtime")]
    public class RuntimeTemplate : ScriptableObject
    {
        [MonoScript(type = typeof(Runtime))] public string runtimeClass = "Runtime";
    }
}
