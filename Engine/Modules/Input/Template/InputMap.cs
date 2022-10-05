//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDT
{
    [CreateAssetMenu(menuName = "Cairo Game/Input Map")]
    public class InputMap : ScriptableObject
    {
        public string mapID = "Default Input";
        public SDictionary<string, Input> inputs = new SDictionary<string, Input>();
    }
}
