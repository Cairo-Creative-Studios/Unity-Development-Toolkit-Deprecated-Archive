//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine.Drivers
{
    [Serializable]
    public class Weapon : Driver<DriverTemplate_Weapon>
    {
        /// <summary>
        /// Resets to 0, and increments by Delta Time, to keep track of the Fire Rate Cooldown
        /// </summary>
        float cooldown;
        public Transform muzzle;
        public ParticleSystem muzzleFlash;
        public bool canShoot = false;

        public Transform attachedProjectile;
        public Transform attachedProjectileParent;

        public int ammo = 0;

        void Start()
        {
            //Get the Muzzle Transform
            muzzle = rootTransform.Find(template.muzzlePath);

            //Instantiate the Muzzle Flash Object
            if (template.muzzleFlash != null)
            {
                GameObject muzzleFlashObject = GameObject.Instantiate(template.muzzleFlash);
                muzzleFlashObject.transform.position = muzzle.position;
                muzzleFlashObject.transform.eulerAngles = muzzle.eulerAngles;
                muzzleFlashObject.transform.parent = muzzle;
                //Get the Particle System of the Muzzle Flash Prefab
                muzzleFlash = muzzleFlashObject.GetComponent<ParticleSystem>();
            }

            if (template.attached)
            {
                attachedProjectile = transform.Find(template.attachedPath);
                attachedProjectileParent = attachedProjectile.parent;

                SphereCollider collider = attachedProjectileParent.gameObject.AddComponent<SphereCollider>();
                collider.isTrigger = true;
            }

            ammo = template.startingAmmo;
        }

        void Update()
        {
            cooldown += Time.deltaTime;

            if (!template.customFireState)
                canShoot = true;

            if (inputs["Shoot"] > 0&&canShoot&&ammo>0)
            {
                if (cooldown > template.fireRate * 60)
                {
                    GameObject projectile = null;
                    if (template.attached)
                    {
                        projectile = attachedProjectile.gameObject;
                    }
                    else
                    {
                        projectile = GameObject.Instantiate(template.projectile);
                    }

                    if(projectile != null)
                    {
                        if(!template.attached)
                            projectile.transform.position = muzzle.position;

                        projectile.transform.rotation = muzzle.rotation;
                        if (muzzleFlash != null)
                            muzzleFlash.Play(true);
                        cooldown = 0;
                        PlayAudioClip("Fire");

                        Bullet bullet = projectile.GetComponent<Bullet>();
                        if (bullet != null)
                        {
                            bullet.fired = true;
                            bullet.originalParent = attachedProjectileParent;
                            bullet.direction = muzzle.forward;
                            bullet.weapon = this;
                        }

                        ammo--;
                    }
                }
            }
        }
    }
}
