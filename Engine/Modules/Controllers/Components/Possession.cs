using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Drivers;

namespace CairoEngine.Controllers
{
	public class Possession : MonoBehaviour
	{
		public ControllerTemplate controllerTemplate;
		public Controller controller;

		void Start()
		{
			controller = ControllerModule.CreatePlayerController(0, controllerTemplate);
			ControllerModule.Possess(controller, gameObject);
		}
	}
}
