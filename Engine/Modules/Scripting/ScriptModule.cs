/*! \addtogroup scriptmodule Script Module
 *  Additional documentation for group 'Script Module'
 *  @{
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Scripting;

namespace CairoEngine
{
    /// <summary>
    /// The Script Module uses an Event Sheet Scripting interface for constructing Gameplay Scenarios
    /// </summary>
    public class ScriptModule
    {
        /// <summary>
        /// All the active Root nodes in the Project
        /// </summary>
        public List<Node> rootNodes = new List<Node>();
    }
}
