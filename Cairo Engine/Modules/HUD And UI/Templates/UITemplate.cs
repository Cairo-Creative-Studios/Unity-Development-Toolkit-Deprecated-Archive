//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CairoEngine.UI;

namespace CairoEngine
{
    [CreateAssetMenu(menuName ="Cairo Game/UI Template")]
    public class UITemplate : Resource
    {

        /// <summary>
        /// The UXML files to use for rendering the UI.
        /// </summary>
        [Tooltip("The UXML Files to render with the UI")]
        public SDictionary<VisualTreeAsset, StyleSheet> UXMLFiles = new SDictionary<VisualTreeAsset, StyleSheet>();

        /// <summary>
        /// The Camera Overlays to use for rendering the UI
        /// </summary>
        [Tooltip("The Camera OVerlays to render with the UI")]
        public List<CameraOverlay> cameraOverlays;

        /// <summary>
        /// Default Game Object Binds to use for the UI
        /// </summary>
        [Tooltip("Default Game Object Binds to use for the UI")]
        public List<GameObjectBind> gameObjectBinds = new List<GameObjectBind>();
    }
}
