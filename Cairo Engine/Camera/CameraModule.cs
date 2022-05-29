using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// Handles the Camera in the Game
    /// </summary>
    public class CameraModule
    {
        /// <summary>
        /// The current Camera's Transform
        /// </summary>
        private static Transform cameraTransform;
        /// <summary>
        /// The current Camera Target (The Object that the Camera should follow)
        /// </summary>
        private static Transform cameraTarget;

        public static void Init()
        {

        }

        public static void Update()
        {
            if (cameraTarget == null)
                cameraTarget = Camera.main.transform;

            Camera.main.transform.position = cameraTarget.position;
            Camera.main.transform.rotation = cameraTarget.rotation;
        }

        /// <summary>
        /// Sets the camera target.
        /// </summary>
        /// <param name="target">Target.</param>
        public static void SetCameraTarget(Transform target)
        {
            cameraTarget = target;
        }
    }
}
