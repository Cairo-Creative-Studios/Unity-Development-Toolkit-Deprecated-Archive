//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using CairoEngine.Reflection;
using CairoData;
using UnityEngine;

namespace CairoEngine.Scripting
{
    /// <summary>
    /// A Block is a Container of ACES or a Parent of other Blocks
    /// </summary>
    [Serializable]
    public class Block
    {
        /// <summary>
        /// The Tree Node of this Block
        /// </summary>
        [NonSerialized]
        public Node<Block> node;

        public enum BlockType
        {
            Event,
            Group,
            State
        }
        /// <summary>
        /// Determines how the Block functions. 
        /// Events perform Conditions and Actions
        /// Groups contain other Blocks and can be Toggled
        /// States contain other Blocks, and use the State Machine to determine which State Block's Code to perform
        /// </summary>
        public BlockType type = BlockType.Event;

        /// <summary>
        /// The text the Node should Display
        /// </summary>
        public string text = "Untitled";
        /// <summary>
        /// The Description of the Node 
        /// </summary>
        public string description = "";
        /// <summary>
        /// Whether the Node is enabled or not
        /// </summary>
        public bool enabled;
        /// <summary>
        /// Whether the Node is an Or Block or not
        /// </summary>
        public bool or;

        /// <summary>
        /// If the Node is an Event, it will contain a List of Condition Nodes
        /// </summary>
        public List<object> conditions = new List<object>();
        /// <summary>
        /// If the Node is an Event, it will contain a List of Action Nodes
        /// </summary>
        public List<object> actions = new List<object>();

        /// <summary>
        /// Objects that have been selected from Picking Conditions. Child Nodes inherit the List.
        /// </summary>
        [NonSerialized]
        public List<GameObject> selectedObjects;

        /// <summary>
        /// Initialized when the Event Sheet file is loaded, to be stored int he Script Module
        /// </summary>
        public void Init()
        {

        }

        /// <summary>
        /// Determines how to Perform the Block
        /// </summary>
        public void Perform()
        {
            switch (type)
            {
                case BlockType.Event:
                    PerformEvent();
                    break;
            }
        }

        /// <summary>
        /// Performs as an Event if this Block is of the Event Type
        /// </summary>
        void PerformEvent()
        {
            //Check all the Conditions in the Block
            int passes = 0;

            foreach (object condition in conditions)
            {
                if ((bool)condition.CallMethod("Perform"))
                    passes++;
            }

            //If the Conditions are met, perform all the Actions and Child Blocks of this Block
            if (passes == conditions.Count)
            {
                foreach (object action in actions)
                {
                    action.CallMethod("Perform");

                    foreach (Node<Block> child in node.children)
                    {
                        child.value.CallMethod("Perform");
                    }
                }
            }
            else
            {
                if (or && passes > 0)
                {
                    foreach (object action in actions)
                    {
                        action.CallMethod("Perform");

                        foreach (Node<Block> child in node.children)
                        {
                            child.value.CallMethod("Perform");
                        }
                    }
                }
            }
        }
    }
}
