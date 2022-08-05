//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class ResourceModule
    {
        /// <summary>
        /// The Resource Packages that exist in the game.
        /// </summary>
        private static List<ResourcePackage> resourcePackages = new List<ResourcePackage>();
        /// <summary>
        /// The Resource Package currently used for the game.
        /// </summary>
        private static ResourcePackage resourcePackage;
        /// <summary>
        /// The Engine's Core Resource Package
        /// </summary>
        private static ResourcePackage enginePackage;

        public static void Init()
        {
            resourcePackages.AddRange(Resources.LoadAll<ResourcePackage>(""));
        }

        /// <summary>
        /// Loads all Resources of the Specified Type in the current Resource Package
        /// </summary>
        /// <returns>The all.</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T[] LoadAll<T>(string path) where T : ScriptableObject
        {
            List<T> resources = new List<T>();

            foreach(object resource in resourcePackage.resources.Values)
            {
                if(resource.GetType() == typeof(T))
                {
                    resources.Add((T)resource);
                }
            }

            return resources.ToArray();
        }

        /// <summary>
        /// Loads the Resource Package into Memory
        /// </summary>
        /// <returns>The package.</returns>
        /// <param name="ID">Identifier.</param>
        private static ResourcePackage LoadPackage(string ID)
        {
            foreach(ResourcePackage package in resourcePackages)
            {
                if(package.ID == ID)
                {
                    return package;
                }
            }

            Debug.LogWarning("Resource Package "+ID+" wasn't found in Resources.");

            return null;
        }
    }
}
