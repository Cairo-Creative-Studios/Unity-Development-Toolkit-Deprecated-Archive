//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UDT.Controllers;
using NaughtyAttributes;

namespace UDT.Objects
{
    /// <summary>
    /// An Extension to Game 
    /// </summary>
    public class ObjectExtension : UDTBehaviour
    {
        /// <summary>
        /// The ID of this Object
        /// </summary>
        [Tooltip("Cairo Engine's Unique Identifier for this Object")]
        [BoxGroup("Default Properties")]
        [ReadOnly] public int OID = -1;
        /// <summary>
        /// The Pool ID for the Object, used for Object Pooling
        /// </summary>
        [Tooltip("The ID of the Object Pool this belongs to")]
        [BoxGroup("Default Properties")]
        public string poolID = "Default";
        /// <summary>
        /// The tags assigned to the Object
        /// </summary>
        [Tooltip("The tags assigned to the Object, for picking")]
        [BoxGroup("Default Properties")]
        public List<string> tags = new List<string>();

        public enum PossessionStatus
        {
            dontPossess,
            automatic,
            firstPlayer,
            secondPlayer,
            thirdPlayer,
            fourthPlayer
        }
        /// <summary>
        /// The Status of Possession for the Instance Upon Creation
        /// </summary>
        [Tooltip("The Status of Possession for the Instance Upon Creation")]
        [BoxGroup("Possession")]
        public PossessionStatus initialPossessionStatus = 0;
        /// <summary>
        /// If the Controller Template is set, it will force the creation of a Controller Object using the given Template, and possess this Object with it.
        /// </summary>
        [Tooltip("If the Controller Template is set, it will force the creation of a Controller Object using the given Template, and possess this Object with it.")]
        [BoxGroup("Possession")]
        public ControllerTemplate controllerTemplate;

        /// <summary>
        /// The Inputs recieved from the Controller of the Entity.
        /// </summary>
        [BoxGroup("Possession")]
        public SDictionary<string, float> inputs = new SDictionary<string, float>();

        public enum SaveSettings
        {
            Everything,
            Transform,
            Behaviours
        }

        public override void EngineInitialized()
        {
            if (initialPossessionStatus == PossessionStatus.automatic)
            {
                if (controllerTemplate != null)
                {
                    if (controllerTemplate.isPlayer)
                    {
                        PlayerController playerController = ControllerModule.CreatePlayerController(0, controllerTemplate);
                        ControllerModule.Possess(playerController, gameObject);
                    }
                }
            }
        }
    }
}
