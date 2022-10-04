using System;
using System.Collections.Generic;
using CairoEngine.Reflection;
using CairoEngine.Drivers;
using NaughtyAttributes;
using UnityEngine;
using System.Reflection;

namespace CairoEngine.AssetScripting
{
    /// <summary>
    /// The Asset Script Linker accesses the Members of MonoBehaviours attached to the Game Object it is also attached to, in order to connect Asset Variables and Methods with the Members within the Behaviour
    /// </summary>
    [ExecuteAlways]
    public class AssetScriptLinker : MonoBehaviour
    {
        public List<AssetVariableLink> variables;
        public List<AssetMethod> methods;

        public void Update()
        {
            //Update all the Serialized Values of all the Variables in the Linker
            foreach (AssetVariableLink variable in variables)
            {
                variable.gameObject = gameObject;
                variable.Update();
            }
        }
    }

    [Serializable]
    public class AssetVariableLink
    {
        [ReadOnly]
        public GameObject gameObject;

        public AssetVariableBase asset;

        [Dropdown("GetMonoBehaviourList")]
        public string monoBehaviour = "";
        [Dropdown("GetFieldList")]
        public string field = "";

        /// <summary>
        /// Get's a List of MonoBehaviours attached to the Game Object, as well as a List of the Drivers that will be generated when Object's Driver Core is Initialized.
        /// </summary>
        /// <returns>The mono behaviour list.</returns>
        DropdownList<string> GetMonoBehaviourList()
        {
            DropdownList<string> monoBehaviourList = new DropdownList<string>();
            monoBehaviourList.Add("null", "null");

            try
            {
                if (gameObject != null)
                {
                    MonoBehaviour[] monoBehaviours = gameObject.GetComponents<MonoBehaviour>();
                    foreach (MonoBehaviour behaviour in monoBehaviours)
                    {
                        monoBehaviourList.Add(behaviour.GetType().FullName.TokenAt(behaviour.GetType().FullName.TokenCount('.') - 1, '.'), behaviour.GetType().FullName);
                    }

                    DriverCore driverCore = gameObject.GetComponent<DriverCore>();
                    DriverCoreTemplate template = driverCore?.properties.template;
                    if (template != null)
                    {
                        foreach (string state in template.states.Keys)
                        {
                            foreach (ExpandableDriverTemplate driver in template.states[state].drivers)
                            {
                                try
                                {
                                    if (driver.template != null)
                                        monoBehaviourList.Add("Driver Component, " + driver.template.driverProperties.main.driverClass.TokenAt(driver.template.driverProperties.main.driverClass.TokenCount('.') - 1, '.'), driver.template.driverProperties.main.driverClass);
                                }
                                catch
                                {
                                    //
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                //
            }

            return monoBehaviourList;
        }

        /// <summary>
        /// Gets a List of the Fields within the Selected Mono
        /// </summary>
        /// <returns>The field list.</returns>
        DropdownList<string> GetFieldList()
        {
            DropdownList<string> fieldList = new DropdownList<string>();
            fieldList.Add("null", null);

            if (monoBehaviour != "" && monoBehaviour != "null")
            {
                FieldInfo[] fieldInfos = Type.GetType(monoBehaviour)?.GetFields();

                if (fieldInfos != null && fieldInfos.Length > 0)
                {
                    foreach (FieldInfo field in fieldInfos)
                    {
                        fieldList.Add(field.Name, field.Name);
                    }
                }
            }

            return fieldList;
        }

        public void Update()
        {
            try
            {
                //Update the Value of the AssetVariable
                if (field != "null" && field != "" && gameObject != null && gameObject.GetComponent(Type.GetType(monoBehaviour)) != null)
                {
                    ((object)asset)?.GetField("variable")?.SetField("value", (object)gameObject.GetComponent(Type.GetType(monoBehaviour))?.GetField(field));
                }
            }
            catch
            {
                //
            }
        }
    }
}
