//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

/**Base Class for all Spektor engine Objects**/
using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Behaviour;

namespace CairoEngine
{
    public class Object : MonoBehaviour
    {
        /// <summary>
        /// Whether the Object is active ingame, for Spawn Pool behaviour
        /// </summary>
        public bool active = true;
        /// <summary>
        /// The ID of this Object
        /// </summary>
        public int OID = -1;
        /// <summary>
        /// The Behaviours that have been added to the Object.
        /// </summary>
        public List<BehaviourType> behaviours = new List<BehaviourType>();
        /// <summary>
        /// The ID of the Pool this Object belongs to
        /// </summary>
        public string poolID = "";
        /// <summary>
        /// The Level this Object is a part of.
        /// </summary>
        public Level level;
        /// <summary>
        /// The children of this Object.
        /// </summary>
        public List<GameObject> children = new List<GameObject>();

        /// <summary>
        /// The Object's Rigid Body, if it currently has one
        /// </summary>
        [HideInInspector] public Rigidbody rigidBody;
        public Vector3 velocity = new Vector3();

        /// <summary>
        /// Enables the behaviour.
        /// </summary>
        /// <param name="behaviour">Behaviour.</param>
        public void EnableBehaviour(string behaviour)
        {
            GetBehaviour(behaviour).Enable();
        }

        /// <summary>
        /// Disables the behaviour.
        /// </summary>
        public void DisableBehaviour(string behaviour)
        {
            GetBehaviour(behaviour).Disable();
        }

        /// <summary>
        /// Enable this instance.
        /// </summary>
        public void Enable()
        {
            foreach(BehaviourType behaviourType in behaviours)
            {
                behaviourType.Enable();
            }
        }

        /// <summary>
        /// Disable this instance.
        /// </summary>
        public void Disable()
        {
            foreach (BehaviourType behaviourType in behaviours)
            {
                behaviourType.Disable();
            }
        }

        /// <summary>
        /// Specifies what happens to the Object upon Creation
        /// </summary>
        public virtual void OnCreate()
        {

        }

        /// <summary>
        /// Specifies what happens to the Object upon Deletion
        /// </summary>
        public virtual void OnDelete()
        {

        }

        /// <summary>
        /// Get a Behaviour attached to the Object
        /// </summary>
        /// <returns>The behaviour.</returns>
        /// <param name="ID">Identifier.</param>
        private BehaviourType GetBehaviour(string ID)
        {
            foreach(BehaviourType behaviourType in behaviours)
            {
                if(behaviourType.template.ID == ID)
                {
                    return behaviourType;
                }
            }

            return null;
        }
    }
}
