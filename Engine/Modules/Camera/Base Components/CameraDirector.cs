//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using Cinemachine;

namespace CairoEngine.CameraTools
{
    public class CameraDirector : MonoBehaviour
    {
        /// <summary>
        /// The Camera Director's Template for Data
        /// </summary>
        public DirectorTemplate template;
        /// <summary>
        /// The Cinemachine Camera Base for the Director
        /// </summary>
        public CinemachineVirtualCameraBase cameraBase;

        /// <summary>
        /// The Root Transform of the Cinemation Object
        /// </summary>
        public Transform Root;

        /// <summary>
        /// The current Follow Target
        /// </summary>
        public Transform followTarget;
        /// <summary>
        /// The current Aim Target
        /// </summary>
        public Transform aimTarget;

        /// <summary>
        /// The Last Follow Target, used for Interpolation.
        /// </summary>
        [HideInInspector] public Transform lastFollowTarget;
        /// <summary>
        /// The Last Aim Target, used for Interpolation.
        /// </summary>
        [HideInInspector] public Transform lastAimTarget;

        /// <summary>
        /// Whether to Interpolate to the Follow and Aim Target (Disabled once the Target has been reached)
        /// </summary>
        [HideInInspector] public bool interpolate = false;
        /// <summary>
        /// The Interpolation Rate
        /// </summary>
        [HideInInspector] public float lerpRate = 0.1f;
        /// <summary>
        /// Whether to Interpolate smoothly
        /// </summary>
        [HideInInspector] public float lerpDampen = 0.75f;
        /// <summary>
        /// The Alpha of the Interpolation (set to 0 at the beginning of the Interpolation)
        /// </summary>
        [HideInInspector] public float lerpAlpha = 0.0f;
        /// <summary>
        /// The Alpha for the Lerp Alpha
        /// </summary>
        [HideInInspector] public float lerpAmount = 0.0f;

        public virtual void Init()
        {

        }
    }
}
