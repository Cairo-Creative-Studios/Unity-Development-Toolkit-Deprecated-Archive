//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Gameplay/Team Template")]
    public class TeamTemplate : Resource
    {
        public string teamName = "Red";
        public Color teamColor = new Color(100.0f, 0.0f, 0.0f);
    }
}
