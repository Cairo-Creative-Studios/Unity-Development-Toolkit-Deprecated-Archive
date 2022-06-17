using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Inventory/Weapon")]
    public class WeaponTemplate : ObjectTemplate
    {
        /// <summary>
        /// The Projectiles that the Weapon can shoot. When the Weapon is told to shoot, the name of the Type of Projectile to shoot needs to be given. The Name is identified with Projectile Info Object.
        /// </summary>
        public List<ProjectileTemplate> projectiles = new List<ProjectileTemplate>();

        /// <summary>
        /// Whether to block the Player from being able to weild this weapon.
        /// </summary>
        public bool blockPlayer = false;

        /// <summary>
        /// What Level the Player must be at to weild the Weapon.
        /// </summary>
        public int weildLevel = 0;
    }
}