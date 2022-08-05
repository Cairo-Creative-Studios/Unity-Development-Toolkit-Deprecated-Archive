using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using CairoData;

namespace CairoEngine.Reflection
{
    public static class ReflectionExtension
    {
        /// <summary>
        /// Calls a Method on the Instance
        /// </summary>
        /// <returns>The method.</returns>
        /// <param name="type">Type.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="parameters">Parameters.</param>
        public static object CallMethod(this object instance, string methodName, object[] parameters = null, string child = "")
        {
            object returnValue = null;

            MethodInfo method = instance.GetType().GetMethod(methodName);
            if (method != null)
            {
                returnValue = method.Invoke(instance, parameters);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the Field from the Instance
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="instance">Instance.</param>
        /// <param name="fieldName">Field name.</param>
        public static object GetField(this object instance, string fieldName, string child = "")
        {
            object returnValue = null;

            FieldInfo field = instance.GetType().GetField(fieldName);
            if(field != null)
            {
                returnValue = field.GetValue(instance);
            }

            return returnValue;
        }

        /// <summary>
        /// Set a Value of a Field from the Instance
        /// </summary>
        /// <param name="instance">Instance.</param>
        /// <param name="fieldName">Field name.</param>
        /// <param name="value">Value.</param>
        public static void SetField(this object instance, string fieldName, object value, string child = "")
        {
            FieldInfo field = instance.GetType().GetField(fieldName);
            if (field != null)
            {
                field.SetValue(instance, value);
            }
        }

        /// <summary>
        /// Gets nested Classes within the given Instance
        /// </summary>
        /// <returns>The nested classes as tree.</returns>
        /// <param name="instance">The Instance to get the Nested Classes from</param>
        /// <param name="baseClassName">The name of the Base Class to use for Class Selection</param>
        /// <param name="instantiate">Whether to create Instances for the found Classes and pass those</param>
        /// <typeparam name="T">The Type of Tree to Generate</typeparam>
        public static Tree<object> GetNestedClassesAsTree(this object instance, string baseClassName = "", bool instantiate = false)
        {
            //Create the Classes Tree
            Tree<object> classes = new Tree<object>(instance);
            //Call the Generation Method for the Tree, passing this method's Parameters
            NestSearchMethod(classes, classes.rootNode,baseClassName, instantiate, 0);
            //Return the Tree
            return classes;
        }

        /// <summary>
        /// Search for Nested Classes to construct the given Tree
        /// </summary>
        /// <param name="tree">Tree.</param>
        /// <param name="node">Node.</param>
        /// <param name="baseClassName">Base class name.</param>
        /// <param name="instantiate">If set to <c>true</c> instantiate.</param>
        private static void NestSearchMethod(Tree<object> tree, Node<object> node, string baseClassName = "", bool instantiate = false, int count = 0)
        {

            List<Node<object>> nestedNodes = new List<Node<object>>();

            int index = 0;

            foreach (Type nestedType in node.value.GetType().GetNestedTypes())
            {

                //Rebuild the Index list for each Nested Type in the passed Node
                List<int> curIndex = new List<int>();
                curIndex.AddRange(node.index);
                curIndex.Add(index);

                if(nestedType.BaseType.Name == baseClassName)
                {
                    if(instantiate)
                        nestedNodes.Add(new Node<object>(tree,Activator.CreateInstance(nestedType), node, curIndex.ToArray()));
                    else
                        nestedNodes.Add(new Node<object>(tree,nestedType, node, curIndex.ToArray()));
                }

                index++;
            }

            //Add the Node to the Tree, and calls this recursive Function again for the next Nodes
            foreach(Node<object> child in nestedNodes)
            {
                tree.Add(child, node.index);
                NestSearchMethod(tree, child, baseClassName, instantiate,count);
            }
        }
    }
}
