using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Drivers
{
	[CreateAssetMenu(menuName = "Cairo Game/Behaviours/Saveable", fileName = "[BEHAVIOUR] Saveable")]
	public class DriverTemplate_Saveable : DriverTemplate
	{
		/// <summary>
		/// Lists of Fields within Monobehaviours that are to be Saved and Loaded
		/// </summary>
		[Tooltip("Lists of Fields within Monobehaviours that are to be Saved and Loaded")]
		public SDictionary<string, List<string>> fields = new SDictionary<string, List<string>>();

		//Initialize the Behaviour Class for this Behaviour
		private void OnEnable()
		{
			this.behaviourClass = "CairoEngine.Drivers.Saveable";
		}
	}
}
