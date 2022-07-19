using System;
using System.Collections.Generic;

namespace CairoEngine.Scripting
{
    [Serializable]
    public class Group : Node
    {
        public bool enabled = true;

        public override void Run()
        {
            if(enabled)
                base.Run();
        }
    }
}
