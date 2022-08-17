//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class ResourcePackage : ScriptableObject
    {
        public string ID = "";

        public SDictionary<string, UnityEngine.Object> resources = new SDictionary<string, UnityEngine.Object>();
    }
}
