using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace UDT.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Properties/Inventory", fileName = "[DRIVER] Inventory")]
    public class DriverTemplate_Inventory : DriverTemplate
    {
        [Header("")]
        [Header(" -- Inventory -- ")]
        [Foldout("Properties")]
        public List<string> pickupTags = new List<string>();

        //Initialize the driver Class for this driver
        private void OnEnable()
        {
            this.driverProperties.main.driverClass = "UDT.Drivers.Inventory";
            SetScriptingEvents("Pickup,Putdown".TokenArray());
        }
    }
}
