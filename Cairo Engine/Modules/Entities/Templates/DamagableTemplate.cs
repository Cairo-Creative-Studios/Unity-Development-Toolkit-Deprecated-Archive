using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    public class StatBasedTemplate : ObjectTemplate
    {
        [Tooltip("The Object's Default Stats")]
        [ReadOnly] public SDictionary<string, float> defaultStats = new SDictionary<string, float>();

        void OnEnabled()
        {
            //Add default Stats
            if (!defaultStats.ContainsKey("Health"))
            {
                defaultStats.Add("Health", 100f);
            }
        }
    }
}
