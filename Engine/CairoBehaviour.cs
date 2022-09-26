using System;
using CairoEngine.Reflection;
using UnityEngine;

namespace CairoEngine
{
    public class CairoBehaviour : MonoBehaviour
    {
        void Awake()
        {
            Engine.EngineInitialized += EngineInitialized;
        }

        public virtual void EngineInitialized()
        {

        }
    }
}
