using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoData
{
    [Serializable]
    public class Tree<T>
    {
        /// <summary>
        /// The Root of the Tree
        /// </summary>
        public Node<T> rootNode;
        /// <summary>
        /// The Currently Selected Node of the Tree
        /// </summary>
        public Node<T> currentNode = null;
        /// <summary>
        /// The Cursor Placement in the Tree
        /// </summary>
        public int[] cursor;

        /// <summary>
        /// Gets and Sets the Value at the given Index
        /// </summary>
        /// <param name="index">Index.</param>
        public T this[int[] index]
        {
            get
            {
                bool searchedRoot = false;

                Node<T> curNode = null;

                foreach (int token in index)
                {
                    if (!searchedRoot)
                        curNode = rootNode.GetNode(token);
                    else
                        if (curNode != null)
                        curNode = curNode.GetNode(token);

                    searchedRoot = true;
                }

                if (curNode != null)
                {
                    return curNode.value;
                }

                return default(T);
            }
            set
            {
                bool searchedRoot = false;

                Node<T> curNode = null;

                foreach (int token in index)
                {
                    if (!searchedRoot)
                        curNode = rootNode.GetNode(token);
                    else
                        if (curNode != null)
                        curNode = curNode.GetNode(token);

                    searchedRoot = true;
                }

                if (curNode != null)
                {
                    curNode.value = value;
                }
            }
        }

        /// <summary>
        /// Gets the Node in the Tree with the given Value
        /// </summary>
        /// <param name="nodeValue">Node value.</param>
        public Node<T> this[T nodeValue]
        {
            get
            {
                Node<T>[] nodeArray = ToArray();

                foreach(Node<T> node in nodeArray)
                {
                    if((object)node.value == (object)nodeValue)
                    {
                        return node;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Trees must be Constructed with a Root Node
        /// </summary>
        /// <param name="root">Root.</param>
        public Tree(T root)
        {
            rootNode = new Node<T>(this, root, null, new int[]{0});
            currentNode = rootNode;
        }

        //todo: Finish Operation Methods
        public bool StepBack()
        {
            if(currentNode.parent != null)
            {
                currentNode = currentNode.parent;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Steps the Tree forward into the given Node
        /// </summary>
        public bool StepForward(int node)
        {
            if (currentNode.children.Count > node)
            {
                currentNode = currentNode.children[node];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Step Forward to the Node with the Given Value
        /// </summary>
        /// <param name="value">Value.</param>
        public bool StepForward(T value)
        {
            Node<T> lastNode = currentNode;

            foreach(Node<T> node in lastNode.children)
            {
                if ((object)node.value == (object)value)
                {
                    currentNode = node;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Step Forward to the Node with the Given String Value
        /// </summary>
        /// <param name="valueToString">Value to string.</param>
        public bool StepForward(string valueToString)
        {
            Node<T> lastNode = currentNode;

            foreach (Node<T>  node in lastNode.children)
            {
                if (node.value.ToString() == valueToString)
                {
                    currentNode = node;
                    return true;
                }
            }

            return false;
        }


        public void Reset()
        {
            currentNode = rootNode;
        }

        /// <summary>
        /// Attaches one Node to a Node at the given Index
        /// </summary>
        /// <param name="node">Node.</param>
        /// <param name="index">Index.</param>
        public void Add(Node<T> node, int[] index)
        {
            bool searchedRoot = false;

            Node<T> curNode = null;
            
            foreach (int token in index)
            {
                if (!searchedRoot)
                {
                    curNode = rootNode;
                }
                else
                    if(curNode.children.Count>token)
                        curNode = curNode.GetNode(token);

                searchedRoot = true;
            }

            curNode.Add(node);
        }

        /// <summary>
        /// Get all the Nodes in the Tree
        /// </summary>
        /// <returns>The nodes.</returns>
        public List<Node<T>> Nodes()
        {
            List<Node<T>> nodes = new List<Node<T>>();
            return null;
        }

        /// <summary>
        /// Gets all the Nodes in the Tree as an Array
        /// </summary>
        /// <returns>The array.</returns>
        public Node<T>[] ToArray()
        {
            List<Node<T>> asList = new List<Node<T>>();
            AddChildren(asList, rootNode);
            return asList.ToArray();
        }

        /// <summary>
        /// Returns the Tree as a JSON Object
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:CairoData.Tree`1"/>.</returns>
        public override string ToString()
        {
            string result = "";
            result = result + "{\n\t\"tree\": [\n\t\t";
            result = StringifyNode(result, 0, rootNode)+"\n\t]\n}";
            return result;

        }

        /// <summary>
        /// A Recursive Function that loops through all the Nodes in the Tree to Compile it into a JSON Object
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="result">Result.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="node">Node.</param>
        private string StringifyNode(string result, int depth, Node<T> node)
        {
            result = result + "\n"+"\t".MultiplyString(depth+2)+"{\n"+ "\t".MultiplyString(depth + 2)+node.value.ToString();

            foreach(Node<T> child in node.children)
            {
                result = result + ":";
                StringifyNode(result, depth++, child);
            }

            result = result + "\n" + "\t".MultiplyString(depth + 2) + "}";

            return result;
        }

        /// <summary>
        /// Adds the Children of the Node to the List
        /// </summary>
        /// <param name="list">List.</param>
        /// <param name="node">Node.</param>
        private void AddChildren(List<Node<T>> list, Node<T> node)
        {
            foreach(Node<T> child in node.children)
            {
                list.Add(child);
            }
            foreach(Node<T> child in node.children)
            {
                AddChildren(list, child);
            }
        }
    }
}
