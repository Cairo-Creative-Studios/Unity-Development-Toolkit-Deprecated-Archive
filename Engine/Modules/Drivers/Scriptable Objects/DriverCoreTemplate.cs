using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace CairoEngine.Drivers
{
	[CreateAssetMenu(menuName = "Cairo Game/Driver Core")]
    public class DriverCoreTemplate : ScriptableObject
    {
        /// <summary>
        /// The Drivers to give to the Object when it's spawned
        /// </summary>
        [Tooltip("The Drivers to give to the Object when it's Spawned.")]
        [Foldout("Properties")]
        public SDictionary<string, List<DriverTemplate>> states = new SDictionary<string, List<DriverTemplate>>();

        /// <summary>
        /// The Root Transform, used for Movement related driver if you want to seperate it from other objects within the Prefab.
        /// </summary>
        [Tooltip("The Root Transform, used for Movement related driver if you want to seperate it from other objects within the Prefab.")]
        [Foldout("Component Paths")]
        public string rootPath = "";

        /// <summary>
        /// The Animator to use for Communications between the Object's Animations and Drivers
        /// </summary>
        [Tooltip("The Animator to use for Communications between the Object's Animations and Drivers")]
        [Foldout("Component Paths")]
        public string animatorPath = "";

    }
}
