//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;

namespace CairoEngine
{
    public class State<T>
    {
        /// <summary>
        /// The methods that were found in the parent class.
        /// </summary>
        private List<string> parentMethods = new List<string>();
        /// <summary>
        /// The variables that were found in the parent class.
        /// </summary>
        private List<string> parentVariables = new List<string>();
        /// <summary>
        /// The Root Instance this State belongs to
        /// </summary>
        public T parent;

        /// <summary>
        /// Initialize this State.
        /// </summary>
        public void Init()
        {

        }

        /// <summary>
        /// Calls a method in the Parent Class.
        /// </summary>
        /// <param name="methodName">Method name.</param>
        public object CallGlobalMethod(string methodName)
        {

            return null;
        }
    }
}
