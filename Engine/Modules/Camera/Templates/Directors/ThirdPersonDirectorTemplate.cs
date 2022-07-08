//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Camera/Director, Third Person")]
    public class ThirdPersonDirectorTemplate : DirectorTemplate
    {
        void OnEnable()
        {
            this.directorClass = "CairoEngine.ThirdPersonCameraDirector";
        }
    }
}
