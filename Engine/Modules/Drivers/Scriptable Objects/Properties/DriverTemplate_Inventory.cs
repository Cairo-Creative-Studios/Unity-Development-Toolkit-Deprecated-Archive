using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace CairoEngine.Drivers
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
			this.driverClass = "CairoEngine.Drivers.Inventory";

			foreach (string defaultEvent in "Pickup,Putdown".TokenArray())
            {
                if (!scriptContainer.output.ContainsKey(defaultEvent))
                    scriptContainer.output.Add(defaultEvent, null);
                if (!scriptContainer.events.ContainsKey(defaultEvent))
                    scriptContainer.events.Add(defaultEvent, null);
            }
		}
	}
}
