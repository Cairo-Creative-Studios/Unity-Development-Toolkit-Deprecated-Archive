using System;
using System.Collections.Generic;

namespace CairoEngine.Scripting
{
    [Serializable]
    public class Node
    {
        public Script script;
        public List<Node> children = new List<Node>();

        public virtual void Init(Script script)
        {
            foreach(Node node in children)
            {
                node.Init(script);
            }
        }

        public virtual void Run()
        {
            foreach (Node child in children)
            {
                Variable asVariable = (Variable)child;

                if (asVariable != null)
                {
                    script.variables[asVariable.name] = asVariable.value;
                    script.activeVariables.Add(asVariable.name);
                }

                child.Run();
            }
        }

        /// <summary>
        /// Renders the Node in the Editor
        /// </summary>
        public virtual void Render()
        {

        }
    }
}
