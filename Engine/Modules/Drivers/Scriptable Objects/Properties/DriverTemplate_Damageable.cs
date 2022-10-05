using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace UDT.Drivers
{
    [CreateAssetMenu(menuName = "Drivers/Properties/Damageable", fileName = "[DRIVER] Damageable")]
    public class DriverTemplate_Damageable : DriverTemplate
    {
        [Header("")]
        [Header(" -- Damageable -- ")]
        /// <summary>
        /// The Default Health to give to the Object
        /// </summary>
        [Tooltip("The Default Health to give to the Object")]
        [Foldout("Properties")]
        public float health = 100f;
        /// <summary>
        /// When True, Objects with this driver will be damaged by all Damage Instegators it collides with.
        /// </summary>
        [Tooltip("When True, Objects with this driver will be damaged by all Damage Instegators it collides with.")]
        [Foldout("Properties")]
        public bool damagedByAll = true;
        /// <summary>
        /// Only Damage Instegators with these tags can deal Damage to this Damageable Object
        /// </summary>
        [Tooltip("Only Damage Instegators with these tags can deal Damage to this Damageable Object")]
        [Foldout("Properties")]
        public List<string> damageTags = new List<string>();

        //Initialize the driver Class for this driver
        private void OnEnable()
        {
            this.driverProperties.main.driverClass = "UDT.Drivers.Damageable";

            SetScriptingEvents("Hit,HealthDepleted".TokenArray());
        }
    }
}
