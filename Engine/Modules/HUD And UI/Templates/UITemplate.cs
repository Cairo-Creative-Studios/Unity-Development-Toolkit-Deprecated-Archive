using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CairoEngine.UI;

namespace CairoEngine
{
    [CreateAssetMenu(menuName ="Cairo Game/UI Template")]
    public class UITemplate : ScriptableObject
    {
        /// <summary>
        /// The name to use for this UI
        /// </summary>
        [Tooltip("This HUD's identifier")]
        public string ID = "DefaultHUD";

        /// <summary>
        /// The UXML files to use for rendering the UI.
        /// </summary>
        [Tooltip("The UXML Files to render with the UI")]
        public SerializableDictionary<VisualTreeAsset, StyleSheet> UXMLFiles = new SerializableDictionary<VisualTreeAsset, StyleSheet>();

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
