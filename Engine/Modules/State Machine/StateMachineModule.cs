/*! \addtogroup statemachinemodule State Machine Module
 *  Additional documentation for group 'State Machine Module'
 *  @{
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoData;
using CairoEngine.StateMachine;
using CairoEngine.Reflection;

namespace CairoEngine
{
    /// <summary>
    /// The State Machine Modules allows Hierarchical, Class Based State Machines to be constructed within any class. The Module handles Method calls within the Nested State Class Instances that are generated for Scripts that are State Machine Enabled.
    /// </summary>
    public static class StateMachineModule
    {
        /// <summary>
        /// All the Machines in the State Machine Module
        /// </summary>
        private static List<Machine> machines = new List<Machine>();
        private static List<StateMachineComponent> components = new List<StateMachineComponent>();

        /// <summary>
        /// Update the State Machine Module
        /// </summary>
        public static void Update()
        {
            foreach(StateMachineComponent component in components)
            {
                //Get the Current State Hiearchy as a String
                string asString = component.machine.states.currentNode.GetValuesInHiearchy().ConvertToString('/');
                //Set the Current State Value as the Hiearchy with the Root Class cut out
                component.currentState = asString.PopAtIndex(0,'/');
                //Set the Watching value (The Class the Component is monitoring) as the Root of the Hiearchy, which is the Root Class for the State Machine
                component.watching = asString.TokenAt(0, '/');
            }

            foreach(Machine machine in machines)
            {
                machine.states.currentNode.value.CallMethod("Update");
            }
        }

        /// <summary>
        /// Creates a State Machine for the Given Instance
        /// </summary>
        /// <param name="root">Root.</param>
        public static Machine CreateStateMachine(object root)
        {
            //Create the new State Machine
            Machine createdMachine = new Machine(root.GetNestedClassesAsTree("State`1", true));

            //Add a reference to the Tree Node in the Created State
            foreach (Node<object> node in createdMachine.states.ToArray())
            {
                node.value.SetField("node", node);
                node.value.SetField("root", root);
            }

            //Call the Enter Method on the active State
            createdMachine.states.currentNode.value.CallMethod("Enter");

            //Add the created State Machine to the List of Machines
            machines.Add(createdMachine);

            return createdMachine;
        }

        /// <summary>
        /// Attaches a Component to the Specified Game Object that can track the status of a State Machine
        /// </summary>
        /// <returns>The component.</returns>
        /// <param name="gameObject">Game object.</param>
        /// <param name="machine">Machine.</param>
        public static StateMachineComponent AddComponent(GameObject gameObject, Machine machine)
        {
            StateMachineComponent component = gameObject.AddComponent<StateMachineComponent>();
            component.machine = machine;
            components.Add(component);
            return component;
        }

        /// <summary>
        /// Gets the State of the Object
        /// </summary>
        /// <returns>The state.</returns>
        /// <param name="root">Root.</param>
        public static object GetState(object root)
        {
            Machine machine = GetMachine(root);
            Tree<object> states = machine.states;

            return states[states.cursor];
        }

        /// <summary>
        /// Gets a Machine that has been enabled through the State Machine Module
        /// </summary>
        /// <returns>The machine.</returns>
        /// <param name="root">Root.</param>
        private static Machine GetMachine(object root)
        {
            foreach(Machine machine in machines)
            {
                if (machine.root == root)
                    return machine;
            }
            return null;
        }
    }
}
