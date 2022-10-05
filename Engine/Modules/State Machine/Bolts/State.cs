using System;
using System.Collections.Generic;
using UnityEngine;
using UDT.Data;
using UDT.Reflection;

namespace UDT.StateMachine
{
    [Serializable]
    public class State<T>
    {
        public Node<object> node = null;
        public T root;

        public void CoreUpdate()
        {

        }

        /// <summary>
        /// Go to the State with the specified path relative to the current State (use '/' to move forward in the State Nest, and '../' to move Backward)
        /// </summary>
        public void GoToState(string statePath)
        {
            //Only search if a path is given, otherwise the State has been set to another in the same Nest as the current State
            if (statePath.TokenCount('/') > 0)
            {

            }
            else
            {
                //foreach(state)
            }
        }

        public string GetState()
        {
            return node.value.ToString();
        }

        /// <summary>
        /// Sets the State to the State with the given path
        /// </summary>
        /// <param name="statePath">State path.</param>
        public void SetState(string statePath)
        {
            Tree<object> tree = node.tree;
            tree.Reset();

            bool exists = true;

            List<object> activeStates = new List<object>();
            object lastState = tree.currentNode.value;

            for (int i = 0; i < statePath.TokenCount('/'); i++)
            {
                if (exists)
                {
                    //Step the Tree Forward into the Nest
                    exists = tree.StepForward(statePath.TokenAt(i, '/'));
                    activeStates.Add(tree.currentNode.value);
                }
                else
                {
                    //If the Requested State wasn't found, call an error set the Active State back to this one and end the Search
                    Debug.LogError("Given State Path is invalid, the given State Hiearchy does not exist: (" + statePath + ")");
                    tree.currentNode = node;
                    return;
                }
            }

            //Call Exit and Enter Methods of appropriate States
            lastState.CallMethod("Exit");
            foreach (object state in activeStates) state.CallMethod("Enter");
        }

        /// <summary>
        /// Gets the Value of a Field in the State Hiearchy. If two States contain the same Variable, the top-most value will be used.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="fieldName">Field name.</param>
        public object GetField(string fieldName)
        {
            Node<object>[] hiearchy = node.GetHiearchy();

            foreach (Node<object> parent in hiearchy)
            {
                return parent.value.GetField(fieldName);
            }

            return null;
        }

        /// <summary>
        /// Sets the value of a Field in the State Hiearchy. If two States contain the same Variable, the top-most value will be used
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="value">Value.</param>
        public void SetField(string fieldName, object value)
        {
            Node<object>[] hiearchy = node.GetHiearchy();

            foreach (Node<object> parent in hiearchy)
            {
                parent.value.SetField(fieldName, value);
                return;
            }
        }

        /// <summary>
        /// Calls a Method in the State hierarchy. If two States contain the same Method name, the top-most Method will be called.
        /// </summary>
        /// <returns>The method.</returns>
        /// <param name="methodName">Method name.</param>
        /// <param name="parameters">Parameters.</param>
        public object CallMethod(string methodName, object[] parameters)
        {
            Node<object>[] hiearchy = node.GetHiearchy();

            foreach (Node<object> parent in hiearchy)
            {
                return parent.value.CallMethod(methodName, parameters);
            }

            return null;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
