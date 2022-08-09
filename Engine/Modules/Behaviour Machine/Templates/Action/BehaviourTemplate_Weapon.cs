using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Behaviour
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Weapon", fileName = "[BEHAVIOUR] Weapon")]
    public class BehaviourTemplate_Weapon : BehaviourTemplate
    {
        [Header("")]
        [Header(" -- Weapon -- ")]
        [Header(" - Stats - ")]
        [Tooltip("Whether to block the Player from being able to weild this weapon.")]
        public bool blockPlayer = false;
        [Tooltip("What Level the Player must be at to weild the Weapon.")]
        public int weildLevel = 0;

        [Header(" - Projectile - ")]
        [Tooltip("The Projectile that is fired by the weapon")]
        public CObjectTemplate projectile;
        [Tooltip("The Amount of Seconds between each shot")]
        public float fireRate = 0.5f;

        [Header(" - Effects - ")]
        [Tooltip("The Muzzle Flash Particle System Prefab to Play when the Weapon shoots")]
        public GameObject muzzleFlash;
        [Tooltip("The Path in the Root Object to the Transform where the Muzzle Flash should be Created")]
        public string muzzlePath;

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            //Set Default Class
            this.behaviourClass = "CairoEngine.Behaviour.Weapon";

            //Set Default Input
            SetInputMap(new string[] { "Shoot" }, new string[] { "Shoot"});

            //Set Default Audio Clips
            foreach(string clipName in "Equip, Charge, Fire, PutAway, Melee".TokenArray())
            {
                if (!audioTable.ContainsKey(clipName))
                {
                    audioTable.Add(clipName, new List<AudioClip>());
                }
                else if(audioTable[clipName] == null)
                {
                    audioTable[clipName] = new List<AudioClip>();
                }
            }

            foreach (string defaultEvent in "Shot,Pickup,Putdown".TokenArray())
            {
                if (!scriptContainer.Output.ContainsKey(defaultEvent))
                    scriptContainer.Output.Add(defaultEvent, null);
            }
        }
    }
}
