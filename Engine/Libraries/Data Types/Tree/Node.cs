using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoData
{
    [Serializable]
    public class Node<T>
    {
        public T value;
        [NonSerialized] public Tree<T> tree;
        [NonSerialized] public Node<T> parent;
        public int[] index;
        public List<Node<T>> children = new List<Node<T>>();

        public Node(Tree<T> tree, T value, Node<T> parent, int[] index)
        {
            this.tree = tree;
            this.value = value;
            this.parent = parent;
            this.index = index;
        }

        public Node<T>[] GetHiearchy()
        {
            List<Node<T>> hiearchy = new List<Node<T>>();
            Node<T> lastNode = this;

            foreach (int i in index)
            {
                hiearchy.Add(lastNode);
                if(lastNode.parent != null)
                    lastNode = lastNode.parent;
            }

            return hiearchy.ToArray();
        }

        public T[] GetValuesInHiearchy()
        {
            Node<T>[] hiearchy = GetHiearchy();
            List<T> values = new List<T>();

            for(int i = hiearchy.Length - 1; i > -1; i--)
            {
                values.Add(hiearchy[i].value);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns the Node within this Node at the given Inex
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="index">The index.</param>
        public Node<T> GetNode(int index)
        {
            return children[index];
        }

        /// <summary>
        /// Adds a Node as a Child to this Node
        /// </summary>
        /// <param name="node">Node.</param>
        public void Add(Node<T> node)
        {
            children.Add(node);
        }
    }
}