//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Gameplay/Level")]
    public class LevelTemplate : ScriptableObject
    {
		public string ID = "Default";
        public string sceneName = "Test Scene";
    }
}
