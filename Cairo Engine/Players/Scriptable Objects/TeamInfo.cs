using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "CairoGame/Team Template")]
    public class TeamInfo : ScriptableObject
    {
        public string teamName = "Red";
        public Color teamColor = new Color(100.0f, 0.0f, 0.0f);
    }
}
