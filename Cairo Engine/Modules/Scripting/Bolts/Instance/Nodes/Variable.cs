using System;
using System.Collections.Generic;

namespace CairoEngine.Scripting
{
    [Serializable]
    public class Variable : Node
    {
        public string name = "";
        public object value;
    }
}
