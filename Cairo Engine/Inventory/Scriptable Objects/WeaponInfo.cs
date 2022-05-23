using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "CairoGame/Inventory/Weapon")]
    public class WeaponInfo : ScriptableObject
    {
        /// <summary>
        /// The Projectiles that the Weapon can shoot. When the Weapon is told to shoot, the name of the Type of Projectile to shoot needs to be given. The Name is identified with Projectile Info Object.
        /// </summary>
        public List<ProjectileInfo> projectiles = new List<ProjectileInfo>();

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