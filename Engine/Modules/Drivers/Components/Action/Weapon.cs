//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace UDT.Drivers
{
    [Serializable]
    public class Weapon : Driver<DriverTemplate_Weapon>
    {
        /// <summary>
        /// Resets to 0, and increments by Delta Time, to keep track of the Fire Rate Cooldown
        /// </summary>
        [Tooltip("Resets to 0, and increments by Delta Time, to keep track of the Fire Rate Cooldown")]
        [Foldout("Properties")]
        float cooldown;
        /// <summary>
        /// The Amount of Ammo currently in the Weapon
        /// </summary>
        [Tooltip("The Amount of Ammo currently in the Weapon")]
        [Foldout("Properties")]
        public int ammo = 0;

        /// <summary>
        /// The Transform of the Muzzle to use for Muzzle Flashes
        /// </summary>
        [Tooltip("The Transform of the Muzzle to use for Muzzle Flashes")]
        [Foldout("Components")]
        public Transform muzzle;
        /// <summary>
        /// The Particle System to use for the Muzzle Flash
        /// </summary>
        [Tooltip("The Particle System to use for the Muzzle Flash")]
        [Foldout("Components")]
        public ParticleSystem muzzleFlash;
        /// <summary>
        /// The Transform of the Projectile that is attached to the Weapon
        /// </summary>
        [Tooltip("The Transform of the Projectile that is attached to the Weapon")]
        [Foldout("Components")]
        public Transform attachedProjectile;
        /// <summary>
        /// The Transform of the Parent for the Projectile to attached to, if the Projectile is to be attached
        /// </summary>
        [Tooltip("The Parent Transform of the Projectile that is attached to this Weapon")]
        [Foldout("Components")]
        public Transform attachedProjectileParent;
        /// <summary>
        /// Whether the Weapon can shoot at this Moment
        /// </summary>
        [Tooltip("Whether the Weapon can shoot at this Moment")]
        [Foldout("State")]
        public bool canShoot = false;

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

            if (inputs["Shoot"] > 0 && canShoot && ammo > 0)
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

                    if (projectile != null)
                    {
                        if (!template.attached)
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
