using System;
using System.Collections.Generic;
using UnityEngine;
using CairoData;

namespace CairoEngine.Scripting
{
    /// <summary>
    /// An Event Sheet is a Visual Scripting Interface that contains code in a Block List sort of structure.
    /// </summary>
    [CreateAssetMenu(menuName = "Event Sheet")]
    public class EventSheet : ScriptableObject
    {
        [HideInInspector] public Tree<Block> script = new Tree<Block>(new Block());
    }
}
