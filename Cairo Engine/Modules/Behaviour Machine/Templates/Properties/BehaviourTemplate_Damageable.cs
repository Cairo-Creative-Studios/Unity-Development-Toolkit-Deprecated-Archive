using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Behaviour
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Damageable", fileName = "[BEHAVIOUR] Damageable")]
    public class BehaviourTemplate_Damageable : BehaviourTemplate
    {
        [Tooltip("The Default Health to give to the Object")]
        public float health = 100f;
        [Tooltip("When True, Objects with this Behaviour will be damaged by all Damage Instegators it collides with.")]
        public bool damagedByAll = true;
        [Tooltip("Only Damage Instegators with these tags can deal Damage to this Damageable Object")]
        public List<string> damageTags = new List<string>();

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            this.behaviourClass = "CairoEngine.Behaviour.Damageable";
        }
    }
}
