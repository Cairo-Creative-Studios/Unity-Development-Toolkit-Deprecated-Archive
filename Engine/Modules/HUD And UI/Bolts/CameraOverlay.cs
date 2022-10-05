//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDT.UI
{
    [Serializable]
    public class CameraOverlay
    {
        /// <summary>
        /// Optional Object to use to disaply the UI Overlay
        /// </summary>
        public GameObject rootObject;
        /// <summary>
        /// The Camera to use for the Overlay
        /// </summary>
        public Camera overlayCamera;
    }
}
