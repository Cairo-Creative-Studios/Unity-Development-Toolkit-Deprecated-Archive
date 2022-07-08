//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Behaviour.CarController
{
    public class Tire : MonoBehaviour
    {
        public int axle = 0;

        private void OnEnable()
        {
            Transform lastCheckedParent = transform;
            for(int i = 0; i < 5; i++)
            {
                lastCheckedParent = lastCheckedParent.parent;

                CarControllerBehaviour car = lastCheckedParent.GetComponent<CarControllerBehaviour>();
                if (car != null)
                {
                    car.AddTire(this);
                }
            }
        }
    }
}
