//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;

namespace CairoEngine.Behaviour
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Character Controller")]
    public class BehaviourTemplate_CharacterController : BehaviourTypeTemplate
    {
        [Tooltip("Smooths out the change in Movement Input")]
        public float movementInterpolation = 0.15f;

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Behaviour.CharacterControllerBehaviour";

            SetInputMap(new string[]{"Horizontal", "Vertical", "Jump"}, new string[]{"Horizontal", "Vertical", "Jump"});
        }
    }
}
