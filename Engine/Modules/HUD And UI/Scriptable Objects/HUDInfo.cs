using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName ="CairoGame/HUD Info")]
    public class HUDInfo : ScriptableObject
    {
        /// <summary>
        /// The name to use for this UI
        /// </summary>
        public string ID = "";
        /// <summary>
        /// The Displays to Instantiate when this HUD is Created
        /// </summary>
        public List<UIDisplay> UIDisplays;
        /// <summary>
        /// The Value Links of the HUD object
        /// </summary>
        public List<UIValueLink> valueLinks = new List<UIValueLink>();
    }
}
