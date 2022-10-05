using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDT.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Properties/Saveable", fileName = "[DRIVER] Saveable")]
    public class DriverTemplate_Saveable : DriverTemplate
    {
        [Header("")]
        [Header(" -- SaveAble -- ")]
        /// <summary>
        /// Lists of Fields within Monobehaviours that are to be Saved and Loaded
        /// </summary>
        [Tooltip("Lists of Fields within Monobehaviours that are to be Saved and Loaded")]
        public SDictionary<string, List<string>> fields = new SDictionary<string, List<string>>();

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.driverProperties.main.driverClass = "UDT.Drivers.Saveable";
        }
    }
}
