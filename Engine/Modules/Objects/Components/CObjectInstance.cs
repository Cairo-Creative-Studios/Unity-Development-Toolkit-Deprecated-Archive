using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// Object Instances allow Level Designers to add Instances of Objects directly into the Scene, rather than taking the time to create Object Templates.
    /// </summary>
    [AddComponentMenu(menuName: "Cairo Game/Object Instance")]
    public class CObjectInstance : MonoBehaviour
    {
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
        /// Optional Template to add to the Object upon Creation
        /// </summary>
        [Tooltip("Optional Template to add to the Object upon Creation")]
        public CObjectTemplate template;
        /// <summary>
        /// The Status of Possession for the Instance Upon Creation
        /// </summary>
        [Tooltip("The Status of Possession for the Instance Upon Creation")]
        public PossessionStatus possessionStatus = 0;

        [Header(" - Without Template - ")]
        /// <summary>
        /// The ID of the Object
        /// </summary>
        [Tooltip("The ID of the Object")]
        public string ID = "Untitled";
        /// <summary>
        /// Tags to give to the Object when it is Spawned.
        /// </summary>
        [Tooltip("Tags to give to the Object when it is Spawned")]
        public List<string> tags = new List<string>();
        /// <summary>
        /// The Behaviour Templates to add on Start
        /// </summary>
        [Tooltip("The Behaviour Templates to add on Start")]
        public List<BehaviourTemplate> behaviourTemplates = new List<BehaviourTemplate>();

        /// <summary>
        /// <see langword="true"/> if waiting for the Engine to Start before Inializing the Instance
        /// </summary>
        private bool waitForEngine = false;

        void Start()
        {
            if (Engine.started)
                Init();
            else
                waitForEngine = true;
        }

        void Update()
        {
            if(waitForEngine)
                if (Engine.started)
                {
                    Init();
                    waitForEngine = false;
                }
        }

        void Init()
        {
            //Add the CObject Behaviour
            CObject cObject = gameObject.AddComponent<CObject>();

            //If template has been left blank, we must make a new one to interface with the Object System
            if (template == null)
            {
                //Create a new Object Template
                template = ScriptableObject.CreateInstance<CObjectTemplate>();

                //Set up Template Properties
                template.ID = ID;
                template.tags = tags;
                template.behaviours = behaviourTemplates;
            }

            //Set the Created Object's Template
            cObject.template = template;

            //Adds the Behaviours to the Object
            ObjectModule.AddBehaviours(gameObject, template);

            //Possess the Object based on the Possession Status of the Object
            switch (possessionStatus)
            {
                case PossessionStatus.automatic:
                    ControllerModule.Possess(ControllerModule.GetFreeController(), cObject);
                    break;
                case PossessionStatus.firstPlayer:
                    ControllerModule.Possess(0, cObject);
                    break;
                case PossessionStatus.secondPlayer:
                    ControllerModule.Possess(1, cObject);
                    break;
                case PossessionStatus.thirdPlayer:
                    ControllerModule.Possess(2, cObject);
                    break;
                case PossessionStatus.fourthPlayer:
                    ControllerModule.Possess(3, cObject);
                    break;
            }
        }
    }
}
