using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.UI
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
