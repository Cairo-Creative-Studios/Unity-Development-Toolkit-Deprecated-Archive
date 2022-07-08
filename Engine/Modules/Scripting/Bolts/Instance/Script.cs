using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Scripting
{
    [Serializable]
    public class Script
    {
        /// <summary>
        /// The root Script Node
        /// </summary>
        public Node root;

        /// <summary>
        /// The current State Branch
        /// </summary>
        public List<string> stateBranch = new List<string>();

        /// <summary>
        /// The current lifespan of the Script
        /// </summary>
        public int tick = 0;

        /// <summary>
        /// The index of the last checked State
        /// </summary>
        [HideInInspector] public int lastCheckedState = 0;
        /// <summary>
        /// The total levels of States within the State Tree
        /// </summary>
        private int stateLevel = 0;

        /// <summary>
        /// The Triggers that are in the Script
        /// </summary>
        public SDictionary<string, Trigger> triggers = new SDictionary<string, Trigger>();
        /// <summary>
        /// The functions that are in the Script
        /// </summary>
        public SDictionary<string, Function> functions = new SDictionary<string, Function>();

        /// <summary>
        /// All the Variables in the Node
        /// </summary>
        public SDictionary<string, object> variables = new SDictionary<string, object>();

        /// <summary>
        /// The Variables that have updated this Tick. If there are Variables in the Variables Dictionary that haven't been updated, they are no longer in Scope and get removed.
        /// </summary>
        public List<string> activeVariables = new List<string>();

        /// <summary>
        /// Initialize the Script
        /// </summary>
        public void Init()
        {
            root.Init(this);

            //Get the Default State Branch from the top-most State in the Script
            if(stateBranch.Count == 0)
            {
                foreach (Node node in root.children)
                {
                    State asState = (State)node;

                    if (asState != null)
                    {
                        stateBranch.Add(asState.name);
                    }
                }
            }

            //Call the On Start Trigger 
            Trigger("OnStart");
        }

        /// <summary>
        /// Update the Script
        /// </summary>
        public void Tick()
        {
            //Clear the Active Variables List
            activeVariables.Clear();

            //Increase the Tick
            tick++;
            //Clear the Check State Index
            lastCheckedState = 0;
            //Run the Root node code
            root.Run();

            //If the Variable isn't in scope, remove it from the Variables Dictionary
            foreach(string variable in variables)
            {
                if (!activeVariables.Contains(variable))
                    variables.Remove(variable);
            }
        }

        /// <summary>
        /// Checks if the passed State is the one that is currently active
        /// </summary>
        /// <returns><c>true</c>, if state is active, <c>false</c> otherwise.</returns>
        /// <param name="stateName">State name.</param>
        public bool CheckState(string stateName)
        {
            if(stateBranch[lastCheckedState] == stateName)
            {
                lastCheckedState++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the current State. If a Level is not Identified, the State will be changed between the second to last layer of State Nodes.
        /// </summary>
        /// <param name="state">The Name of the State to change to</param>
        /// <param name="level">Level of the State to go to</param>
        public void SetState(string state, int level = -1)
        {
            stateBranch[Mathf.Clamp(stateBranch.Count - 2,0,100)] = state;
        }

        /// <summary>
        /// Sets the State by replacing the Entire active branch with the given Link
        /// </summary>
        /// <param name="link">The Link to the State to go to.</param>
        public void SetStateToLink(string link)
        {
            //Clear the State Branch
            stateBranch.Clear();

            //Loop through all the States in the branch Link
            for(int i = 0; i < link.TokenCount('/'); i++)
            {
                //Add the State to the active State Branch
                stateBranch.Add(link.TokenAt(i, '/'));
            }
        }

        /// <summary>
        /// Activates a Trigger that's included in the Script
        /// </summary>
        /// <param name="trigger">The name of the Trigger</param>
        public void Trigger(string trigger)
        {
            triggers[trigger].Run();
        }
    }
}
