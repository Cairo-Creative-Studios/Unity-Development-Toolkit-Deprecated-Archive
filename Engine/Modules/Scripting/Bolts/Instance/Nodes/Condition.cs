using System;
using System.Collections.Generic;

namespace CairoEngine.Scripting
{
    [Serializable]
    public class Condition
    {
        /// <summary>
        /// Whether this is a Not block
        /// </summary>
        public bool not = false;
        public bool value = true;

        public Script script;
        public List<Field> fields = new List<Field>();

        /// <summary>
        /// Returns the Value of this Condition
        /// </summary>
        /// <returns>The Value of the Condition</returns>
        public virtual bool Check()
        {
            //Toggle the Result if this is a Not block
            if (not)
                return !value;
            else
                return value;
        }

        /// <summary>
        /// Renders the Condition in the Editor
        /// </summary>
        public virtual void Render()
        {

        }
    }
}
