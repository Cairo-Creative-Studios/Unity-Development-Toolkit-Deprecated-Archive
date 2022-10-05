//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using UDT.Data;

namespace UDT.Scripting
{
    /// <summary>
    /// An Event Sheet is a Visual Scripting Interface that contains code in a Block List sort of structure.
    /// </summary>
    [CreateAssetMenu(menuName = "Event Sheet")]
    public class EventSheet : ScriptableObject
    {
        /// <summary>
        /// The actual Structure of the Event Sheet file
        /// </summary>
        [HideInInspector] public Tree<Block> script = new Tree<Block>(new Block());
        /// <summary>
        /// All the Groups in the Event sheet, and coupled with their Enabled State
        /// </summary>
        [HideInInspector] public SDictionary<string, bool> groups = new SDictionary<string, bool>();
    }
}
