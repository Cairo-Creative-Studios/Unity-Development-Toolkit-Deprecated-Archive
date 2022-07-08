using System;
using System.Collections.Generic;

namespace CairoEngine.Scripting
{
    [Serializable]
    public class Action
    {
        public Script script;
        public List<Field> fields = new List<Field>();

        /// <summary>
        /// Perform the functionality of the Action
        /// </summary>
        public virtual void Perform()
        {

        }

        /// <summary>
        /// Renders the Action in the Editor
        /// </summary>
        public virtual void Render()
        {

        }
    }
}
