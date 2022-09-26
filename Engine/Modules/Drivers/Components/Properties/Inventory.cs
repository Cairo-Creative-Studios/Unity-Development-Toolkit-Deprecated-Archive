using System;
using System.Collections.Generic;
using CairoEngine.InventoryManagement;
using UnityEngine;

namespace CairoEngine.Drivers
{
	public class Inventory : Driver<DriverTemplate_Inventory>
	{
		public List<DriverTemplate_InventoryItem> items = new List<DriverTemplate_InventoryItem>();

		void Start()
		{
			InventoryModule.AddInventory(this);
		}

		public void Pickup(InventoryItem item)
		{
			items.Add(item.template);
			item.Pickup(core);
		}
	}
}
