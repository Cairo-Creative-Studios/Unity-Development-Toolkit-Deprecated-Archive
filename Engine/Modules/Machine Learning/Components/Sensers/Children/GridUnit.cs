//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using UDT;
using UDT.MachineLearning;

namespace UDT.MachineLearning.Sensers.Children
{
    public class GridUnit : MonoBehaviour
    {
        public GameObject collidingObject = null;

        void OnCollisionEnter(Collision collision)
        {
            collidingObject = collision.gameObject;
        }
    }
}
