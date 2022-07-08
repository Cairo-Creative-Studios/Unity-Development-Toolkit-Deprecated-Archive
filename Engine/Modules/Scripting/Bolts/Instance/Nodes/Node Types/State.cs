using System;
using System.Collections.Generic;

namespace CairoEngine.Scripting
{
    [Serializable]
    public class State : Node
    {
        public string name = "";

        public override void Run()
        {
            if(script.CheckState(name))
                base.Run();
        }
    }
}
