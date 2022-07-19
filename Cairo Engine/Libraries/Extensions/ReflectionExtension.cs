using System;
using System.Reflection;

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

            if(child != "")
            {
                Type lastType;

                foreach (string childClass in child.TokenArray('.'))
                {

                }
            }

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
        /// Gets a Child Class from the Instance
        /// </summary>
        /// <param name="instance">Instance.</param>
        /// <param name="className">Class name.</param>
        public static void GetChildClass(this object instance, string className)
        {

        }

        public static void GenerateReflectionInstance(this object instance)
        {
            ReflectionModule.AddInstance(instance);
        }
    }
}
