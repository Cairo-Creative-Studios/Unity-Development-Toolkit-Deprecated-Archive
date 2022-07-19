using System;
using System.Collections.Generic;
using CairoEngine.Scripting;

namespace CairoEngine
{
    public class ScriptTemplate
    {
        public List<Node> nodes = new List<Node>();

        void OnEnable()
        {
            nodes.Add(new Event());
        }
    }
}
