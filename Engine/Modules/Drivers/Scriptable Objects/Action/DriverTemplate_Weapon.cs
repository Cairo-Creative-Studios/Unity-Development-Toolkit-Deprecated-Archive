using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Drivers
{
    [CreateAssetMenu(menuName = "Cairo Game/Behaviours/Weapon", fileName = "[BEHAVIOUR] Weapon")]
    public class DriverTemplate_Weapon : DriverTemplate
    {
        [Header("")]
        [Header(" -- Weapon -- ")]
        [Header(" - Stats - ")]
        [Tooltip("Whether to block the Player from being able to weild this weapon.")]
        public bool blockPlayer = false;
        [Tooltip("What Level the Player must be at to weild the Weapon.")]
        public int weildLevel = 0;
        [Tooltip("The amount of Ammo in the weapon when it's created")]
        public int startingAmmo = 1;
        [Tooltip("The Max Amount of Ammo in a Clip")]
        public int ammoMax = 1;
        [Tooltip("The Amount of Ammo Clips")]
        public int clipCount = 0;

        [Header(" - Projectile - ")]
        [Tooltip("The Projectile that is fired by the weapon, ignored if the Attached")]
        public GameObject projectile;
        [Tooltip("Whether the Projectile Game Object is attached to the Weapon")]
        public bool attached = false;
        [Tooltip("The Path to the attached Projectile.")]
        public string attachedPath = "";
        [Tooltip("The Amount of Seconds between each shot")]
        public float fireRate = 0.5f;
        [Tooltip("Only allows fire if an outside Script sets canFire to true")]
        public bool customFireState = false;

        [Header(" - Effects - ")]
        [Tooltip("The Muzzle Flash Particle System Prefab to Play when the Weapon shoots")]
        public GameObject muzzleFlash;
        [Tooltip("The Path in the Root Object to the Transform where the Muzzle Flash should be Created")]
        public string muzzlePath;

        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            //Set Default Class
            this.behaviourClass = "CairoEngine.Drivers.Weapon";

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
                if (!scriptContainer.output.ContainsKey(defaultEvent))
                    scriptContainer.output.Add(defaultEvent, null);
                if (!scriptContainer.events.ContainsKey(defaultEvent))
                    scriptContainer.events.Add(defaultEvent, null);
            }
        }
    }
}
