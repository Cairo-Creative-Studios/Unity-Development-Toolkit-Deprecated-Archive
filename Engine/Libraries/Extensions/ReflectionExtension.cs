//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

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
        /// Gets the nested types with the Base Type of the given name. 
        /// </summary>
        /// <returns>The nested types from base.</returns>
        /// <param name="type">Type to find Nested Types in</param>
        /// <param name="baseTypeName">Base type name.</param>
        public static Type[] GetNestedTypesOfBase(this Type type, string baseTypeName)
        {
            Type[] types = type.GetNestedTypes();
            List<Type> desiredTypes = new List<Type>();

            foreach (Type curType in types)
            {
                if (curType.BaseType.Name == baseTypeName)
                {
                    desiredTypes.Add(curType);
                }

                Debug.Log(curType);
            }

            return desiredTypes.ToArray();
        }

        /// <summary>
        /// Calls a Method on the Instance
        /// </summary>
        /// <returns>The method.</returns>
        /// <param name="instance">instance.</param>
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

        public static bool HasMethod(this Type type, string methodName)
        {
            return type.GetMethod(methodName) != null;
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

        //TODO: Fix to allow Uninstantiated Classes to be Tree Nodes
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

                if(nestedType.BaseType.Name == baseClassName||baseClassName == "")
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
