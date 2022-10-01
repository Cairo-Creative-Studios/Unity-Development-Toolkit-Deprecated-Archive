using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Drivers
{
	[CreateAssetMenu(menuName = "Cairo Game/Behaviours/Inventory", fileName = "[BEHAVIOUR] Inventory")]
	public class DriverTemplate_Inventory : DriverTemplate
	{
		public List<string> pickupTags = new List<string>();

		//Initialize the Behaviour Class for this Behaviour
		private void OnEnable()
		{
			this.behaviourClass = "CairoEngine.Drivers.Inventory";

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
