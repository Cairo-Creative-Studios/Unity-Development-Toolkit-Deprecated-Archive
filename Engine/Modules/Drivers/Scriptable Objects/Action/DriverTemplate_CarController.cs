//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Action/Car Controller", fileName = "[DRIVER] Car Controller")]
    public class DriverTemplate_CarController : DriverTemplate
    {
        [Header("")]
        [Header(" -- Car Controller -- ")]
        /// <summary>
        /// The Seats of the Vehicle
        /// </summary>
        [Tooltip("The Seats of the Vehicle")]
        public List<Seat> seats = new List<Seat>();

        //Initialize the driver Class for this driver
        private void OnEnable()
        {
            this.driverProperties.main.driverClass = "CairoEngine.Drivers.CarController";
            SetInputTranslation(new string[] { "Steer", "Accel", "Brake" }, new string[] { "Driver_MoveHorizontal", "Driver_LeftZAxis", "Driver_RightZAxis" });
            SetScriptingEvents("Accelerate, Brake, Stopped, Hit".TokenArray());
        }
    }
}
