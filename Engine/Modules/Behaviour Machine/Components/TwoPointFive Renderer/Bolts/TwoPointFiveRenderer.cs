//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Behaviour.TwoPointFive
{
    /// <summary>
    /// Renders complex 2.5D Sprites
    /// </summary>
    public class Renderer : MonoBehaviour
    {
        /// <summary>
        /// All the Sprites used in the Renderer, along with the Joints used to Calculate Angles for Rendering
        /// </summary>
        public Dictionary<Sprite, List<Joint>> sprites = new Dictionary<Sprite, List<Joint>>();
    }
}