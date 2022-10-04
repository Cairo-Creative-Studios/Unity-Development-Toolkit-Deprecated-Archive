using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using CairoEngine.Reflection;

namespace CairoEngine.Drivers
{
    [CreateAssetMenu(menuName = "Cairo Game/Driver Core")]
    public class DriverCoreTemplate : ScriptableObject
    {
        /// <summary>
        /// The Drivers to give to the Object when it's spawned
        /// </summary>
        [Tooltip("The Drivers to give to the Object when it's Spawned.")]
        public SDictionary<string, DriverTemplateContainer> states = new SDictionary<string, DriverTemplateContainer>();


        /// <summary>
        /// Holds all the Paths to Components used by the Driver Core and it's Drivers
        /// </summary>
        public class PathsContainer
        {
            /// <summary>
            /// The Root Transform, used for Movement related driver if you want to seperate it from other objects within the Prefab.
            /// </summary>
            [Tooltip("The Root Transform, used for Movement related driver if you want to seperate it from other objects within the Prefab.")]
            public string rootPath = "";
            /// <summary>
            /// The Animator to use for Communications between the Object's Animations and Drivers
            /// </summary>
            [Tooltip("The Animator to use for Communications between the Object's Animations and Drivers")]
            public string animatorPath = "";
        }
        /// <summary>
        /// The Paths to the Components used by the driver Core and it's Driver's
        /// </summary>
        [Tooltip("The Paths to the Components used by the Driver Core and it's Driver's")]
        [HorizontalLine]
        public PathsContainer paths = new PathsContainer();
    }

    [Serializable]
    public class DriverTemplateContainer
    {
        /// <summary>
        /// The List of Drivers within the Project's Scope
        /// </summary>
        [Dropdown("GetDriverTypes")]
        public string newDriver;

        /// <summary>
        /// The Drivers in the Container
        /// </summary>
        public List<ExpandableDriverTemplate> drivers = new List<ExpandableDriverTemplate>();

        /// <summary>
        /// Makes a new Driver Template and adds it to the Selected State
        /// </summary>
        [EasyButtons.Button]
        public void CreateNewDriver()
        {
            DriverTemplate templateToAdd = (DriverTemplate)Activator.CreateInstance(Type.GetType(newDriver));
            drivers.Add(new ExpandableDriverTemplate(templateToAdd));
        }

        /// <summary>
        /// Gets all the Types of Drivers in the Project, so they can be Added
        /// </summary>
        /// <returns>The driver types.</returns>
        public DropdownList<string> GetDriverTypes()
        {
            return DriverModule.ValidateDriverTypes();
        }
    }

    [Serializable]
    public class ExpandableDriverTemplate
    {
        [Expandable]
        public DriverTemplate template;

        public ExpandableDriverTemplate(DriverTemplate template)
        {
            this.template = template;
        }
    }
}
