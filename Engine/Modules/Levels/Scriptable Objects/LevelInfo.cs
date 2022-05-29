using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "CairoGame/Level")]
    public class LevelInfo : ScriptableObject
    {
        public string ID = "New Level";
        public string sceneName = "Test Scene";
    }
}
