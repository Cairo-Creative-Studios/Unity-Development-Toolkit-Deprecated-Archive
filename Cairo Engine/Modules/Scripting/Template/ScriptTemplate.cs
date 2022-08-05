using System;
using System.Collections.Generic;
using CairoEngine.Scripting;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Event Sheet")]
    public class ScriptTemplate : Resource
    {
        public List<Node> nodes = new List<Node>();

        void OnEnable()
        {
        }
    }
}
