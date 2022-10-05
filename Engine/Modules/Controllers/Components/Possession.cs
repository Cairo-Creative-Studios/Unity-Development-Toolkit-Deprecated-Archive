using System.Collections.Generic;
using UnityEngine;
using UDT.Drivers;

namespace UDT.Controllers
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
