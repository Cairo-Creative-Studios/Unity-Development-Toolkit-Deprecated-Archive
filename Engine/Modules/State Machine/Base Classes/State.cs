using System;
using System.Collections.Generic;

namespace CairoEngine
{
    public class State
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

        /// <summary>
        /// Sets a variable value in the Parent Class.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="value">Value.</param>
        public void SetGlobalVariable(string variableName, object value)
        {

        }

        public object GetGlobalVariable(string variableName)
        {

            return null;
        }
    }
}
