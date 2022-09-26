using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Drivers
{
	[CreateAssetMenu(menuName = "Cairo Game/Driver Core")]
    public class DriverCoreTemplate : ScriptableObject
    {
        /// <summary>
        /// The Behaviours to give to the Object when it's spawned
        /// </summary>
        [Tooltip("The Behaviours to give to the Object when it's Spawned.")]
        public SDictionary<string, List<DriverTemplate>> states = new SDictionary<string, List<DriverTemplate>>();
        /// <summary>
        /// The Root Transform, used for Movement related behaviour if you want to seperate it from other objects within the Prefab.
        /// </summary>
        [Tooltip("The Root Transform, used for Movement related behaviour if you want to seperate it from other objects within the Prefab.")]
        public string rootPath = "";
        /// <summary>
        /// The Animator to use for Communications between the Object's Animations and Behaviours
        /// </summary>
        [Tooltip("The Animator to use for Communications between the Object's Animations and Behaviours")]
        public string animatorPath = "";

    }
}
