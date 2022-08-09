using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Behaviour
{
    [Serializable]
    public class Weapon : CairoBehaviour<BehaviourTemplate_Weapon>
    {
        /// <summary>
        /// Resets to 0, and increments by Delta Time, to keep track of the Fire Rate Cooldown
        /// </summary>
        float cooldown;
        public Transform muzzle;
        public ParticleSystem muzzleFlash;

        public virtual void Init()
        {
            //Get the Muzzle Transform
            muzzle = rootTransform.Find(template.muzzlePath);
            //Instantiate the Muzzle Flash Object
            GameObject muzzleFlashObject = GameObject.Instantiate(template.muzzleFlash);
            muzzleFlashObject.transform.position = muzzle.position;
            muzzleFlashObject.transform.eulerAngles = muzzle.eulerAngles;
            muzzleFlashObject.transform.parent = muzzle;
            //Get the Particle System of the Muzzle Flash Prefab
            muzzleFlash = muzzleFlashObject.GetComponent<ParticleSystem>();
        }

        public virtual void Update()
        {
            cooldown += Time.deltaTime;

            if (inputs["Shoot"] > 0)
            {
                if (cooldown > template.fireRate * 60)
                {
                    ObjectModule.Spawn(template.projectile, muzzle);
                    muzzleFlash.Play(true);
                    cooldown = 0;
                    PlayAudioClip("Fire");
                }
            }
        }
    }
}
