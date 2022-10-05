using System;
using System.Collections.Generic;
using UDT.Data;

namespace UDT.StateMachine
{
    [Serializable]
    public class Machine
    {
        public object root;
        /// <summary>
        /// The State Machine Tree contains all the State classes within the core Object's collection of States. 
        /// </summary>
        public Tree<object> states = null;

        public Machine(Tree<object> states)
        {
            this.states = states;

            //Step the Tree Forward into the First State
            states.StepForward(0);
        }
    }
}
