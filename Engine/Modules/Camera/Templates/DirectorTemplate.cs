//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine.CameraTools;
using B83.Unity.Attributes;

namespace CairoEngine
{
    public class DirectorTemplate : Resource
    {
        [ReadOnly][MonoScript] public string directorClass = ""; 

        [Tooltip("When the Director is created, it searches the Root for the Aim Transform")]
        public string aimTargetName = "";
        [Tooltip("When the Director is created, it searched the Root for the Target Transform")]
        public string followtargetName = "";
        [Tooltip("Interpolates Between Follow Targets when they're changed")]
        public float followLerp = 0.9f;
        [Tooltip("Interpolates Between Aim Targets when they're changed")]
        public float aimLerp = 0.9f;

        public class Dampening
        {
            new Vector3 position = new Vector3(0.1f, 0.1f, 0.1f);
            new Vector3 rotation = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
