using System;
using CairoEngine.Reflection;
using System.Collections.Generic;

namespace CairoEngine.Reflection
{
    public class ReflectionModule
    {
        /// <summary>
        /// All the Reflection Instances that have been created.
        /// </summary>
        private static Dictionary<object, ReflectionInstance> instances = new Dictionary<object, ReflectionInstance>();

        public static void AddInstance(object instance)
        {
            instances.Add(instance, new ReflectionInstance(instance));
        }

        public static object GetInstance(string className)
        {
            foreach(object instance in instances.Keys)
            {
                if (instance.GetType().Name == className)
                {
                    return instances[instance];
                }
            }
            return null;
        }

    }
}
