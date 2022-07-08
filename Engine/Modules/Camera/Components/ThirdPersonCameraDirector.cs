//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine.CameraTools;
using Cinemachine;

namespace CairoEngine
{
    public class ThirdPersonCameraDirector : CameraDirector
    {
        CinemachineFreeLook freeLook;

        public override void Init()
        {
            cameraBase = gameObject.AddComponent<CinemachineFreeLook>();
            freeLook = (CinemachineFreeLook) cameraBase;
        }
    }
}
