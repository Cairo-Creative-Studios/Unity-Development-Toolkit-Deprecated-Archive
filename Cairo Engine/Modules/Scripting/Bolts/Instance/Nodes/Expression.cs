using System;
using System.Collections.Generic;

namespace CairoEngine.Scripting
{
    [Serializable]
    public class Expression<T>
    {
        /// <summary>
        /// The Value of this Property. (This will be overwrote in extending Properties)
        /// </summary>
        private object value;
        public List<Field> fields = new List<Field>();

        /// <summary>
        /// Gets the Value of the Property 
        /// </summary>
        /// <returns>The get.</returns>
        public virtual T Get()
        {
            return (T)value;
        }
    }
}
