using System;
using System.Collections.Generic;

namespace CairoEngine.Reflection
{
    /// <summary>
    /// An Instance Container is an Instance of a Class that contains a List of Child Instances in a Tree
    /// </summary>
    public class ReflectionInstance
    {
        private object parent;
        private Dictionary<string, object> children = new Dictionary<string, object>();

        public object this[string className]
        {
            /// <summary>
            /// Gets the Instance within this Reflection Instance's Children with the given Class Name
            /// </summary>
            /// <returns>The item.</returns>
            /// <param name="className">Class name.</param>
            get
            {
                if(parent.GetType().Name == className)
                {
                    return parent;
                }
                else
                {
                    foreach(string child in children.Keys)
                    {
                        if(child == className)
                        {
                            return children[child];
                        }
                    }
                }
                return null;
            }
        }

        public ReflectionInstance(object parent)
        {
            this.parent = parent;

            AddChildren(parent);
        }

        /// <summary>
        /// Searches through the Tree of Nested Classes within the Created Reflection Instance, and Creates Instances of them, adding them to the Initial Instance (Parent)
        /// </summary>
        /// <param name="parent">Parent.</param>
        private void AddChildren(object initalInstance)
        {
            Type[] nestedClasses = initalInstance.GetType().GetNestedTypes();

            //Loop through all the nested classes in the Instance
            foreach(Type nestedClass in nestedClasses)
            {
                //Add the Nested Class to the Children of the initial Parent Instance
                children.Add(nestedClass.FullName, Activator.CreateInstance(nestedClass));

                //Call Add Children for each current Child in the Tree
                AddChildren(nestedClass);
            }
        }

        /// <summary>
        /// Returns an Array of all the Child Instances belonging to this Instance
        /// </summary>
        /// <returns>The children.</returns>
        public object[] GetChildren()
        {
            List<object> childInstances = new List<object>();

            foreach(object child in children)
            {
                childInstances.Add(child);
            }

            return childInstances.ToArray();
        }

        /// <summary>
        /// Call a Method on all the Child Instances of this Parent
        /// </summary>
        /// <param name="methodName">Method name.</param>
        /// <param name="parameters">Parameters.</param>
        public void CallMethodOnChildren(string methodName, object[] parameters)
        {
            foreach(object child in children)
            {
                child.CallMethod(methodName, parameters);
            }
        }
    }
}
