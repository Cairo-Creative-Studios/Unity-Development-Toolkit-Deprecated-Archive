using System;
using UnityEngine;
using CairoEngine;
using Cinemachine;

namespace CairoEngine.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Cameras/Third Person", fileName = "[DRIVER] Third Person Camera")]
    public class DriverTemplate_ThirdPersonCamera : DriverTemplate
    {
        //[Header("")]
        //[Header(" -- Third Person Camera -- ")]
        //[Tooltip("Time it takes for the Camera to recenter on the X axis (Recenters behind the Root Object)")]
        //public float resetXTime = 2f;

        //Initialize the driver Class for this driver
        private void OnEnable()
        {
            this.driverProperties.main.driverClass = "CairoEngine.Drivers.ThirdPersonCamera";

            SetInputTranslation(new string[] { "XAxis", "YAxis" }, new string[] { "LookHorizontal", "LookVertical" });

        }


    }
}
