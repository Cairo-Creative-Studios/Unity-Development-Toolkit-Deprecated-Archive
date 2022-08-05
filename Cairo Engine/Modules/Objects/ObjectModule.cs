//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using B83.Unity.Attributes;

namespace CairoEngine
{
    /// <summary>
    /// The Object Manager lets you create and interact with Objects easily in the Game.
    /// </summary>
    public class ObjectModule
    {

        /// <summary>
        /// The objects that the Engine is controlling.
        /// </summary>
        private static List<CObject> objects = new List<CObject>();
        /// <summary>
        /// The Game Objects that have been added to the Spawn Pool
        /// </summary>
        private static Dictionary<string,Queue<GameObject>> spawnPool = new Dictionary<string,Queue<GameObject>>();

        private static List<CObjectTemplate> templates = new List<CObjectTemplate>();

        private static Dictionary<string, List<CObject>> activeTags = new Dictionary<string, List<CObject>>();

        /// <summary>
        /// The current Spawn Pool
        /// </summary>
        private static GameObject spawnPoolContainer;

        /// <summary>
        /// The current Moving Speed of the Object
        /// </summary>
        [Tooltip("The current Moving Speed of the Object")]
        public Vector3 Velocity;

        public static void Init()
        {
            templates.AddRange(Resources.LoadAll<CObjectTemplate>(""));
        }

        public static void Update()
        {

        }

        #region Objects
        ///<summary>
        /// Adds an Object to the Object list
        ///</summary>
        public static void AddObject(CObject addedObject)
        {
            objects.Add(addedObject);
        }

        /// <summary>
        /// Checks a Game Object into the Object Module
        /// </summary>
        /// <param name="registeredObject">Registered object.</param>
        //public static Object CheckIn(GameObject registeredObject)
        //{
        //    //Add the Object Component and Enable the State Machine on it.
        //    Object createdObject = registeredObject.AddComponent<Object>();
        //    StateMachineModule.EnableStateMachine(registeredObject, createdObject);
        //    return createdObject;
        //}

        /// <summary>
        /// Creates a new Object and Returns it.
        /// </summary>
        /// <returns>The object.</returns>
        public static CObject CreateObject(string name)
        {
            GameObject spekObject = new GameObject();
            CObject spekBehaviour = spekObject.AddComponent<CObject>();
            objects.Add(spekBehaviour);
            spekBehaviour.OID = objects.Count - 1;
            return spekBehaviour;
        }
        #endregion

        /// <summary>
        /// Called when a Level is created, creates a Spawn Pool object alongside the Level
        /// </summary>
        public static void CreateSpawnPool()
        {
            spawnPoolContainer = new GameObject();
            spawnPoolContainer.name = "Spawn Pool";
        }

        /// <summary>
        /// Spawns the Game Object and adds it to the Spawn Pool.
        /// </summary>
        /// <returns>The spawn.</returns>
        public static GameObject Spawn(string ID, Transform spawner)
        {
            //The Object that is Spawned
            GameObject spawnedObject;

            if (spawnPool.ContainsKey(ID))
            {
                if (spawnPool[ID].Count > 0)
                {
                    spawnedObject = spawnPool[ID].Dequeue();
                    spawnedObject.SetActive(true);

                    AddBehaviours(spawnedObject, ID);

                    return spawnedObject;
                }
            }


            CObjectTemplate template = GetTemplate(ID);

            if (template != null)
            {
                if (template.prefab != null)
                {
                    spawnedObject = Engine.CreatePrefabInstance(template.prefab);
                    spawnedObject.transform.position = spawner.position;
                    spawnedObject.transform.eulerAngles = spawner.eulerAngles;
                    CObject objectBehaviour = spawnedObject.AddComponent<CObject>();
                    objectBehaviour.template = template;

                    objectBehaviour.poolID = ID;

                    AddBehaviours(spawnedObject, ID);

                    return spawnedObject;
                }
            }

            return null;
        }

        /// <summary>
        /// Spawns the Game Object and adds it to the Spawn Pool.
        /// </summary>
        /// <returns>The spawn.</returns>
        public static GameObject Spawn(CObjectTemplate template, Transform spawner)
        {
            //The Object that is Spawned
            GameObject spawnedObject;

            if (template != null)
            {
                if (template.prefab != null)
                {
                    spawnedObject = Engine.CreatePrefabInstance(template.prefab);
                    spawnedObject.transform.position = spawner.position;
                    spawnedObject.transform.eulerAngles = spawner.eulerAngles;
                    CObject objectBehaviour = spawnedObject.AddComponent<CObject>();
                    objectBehaviour.template = template;

                    objectBehaviour.poolID = template.ID;

                    AddBehaviours(spawnedObject, template.ID);

                    return spawnedObject;
                }
            }

            return null;
        }

        /// <summary>
        /// Destroying the Object adds the Object to the Spawn Pool and Deactivates it.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        public static void Destroy(GameObject gameObject)
        {
            string ID = gameObject.GetComponent<CObject>().poolID;

            if (!spawnPool.ContainsKey(ID))
                spawnPool.Add(ID,new Queue<GameObject>());

            spawnPool[ID].Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Activates the tags.
        /// </summary>
        /// <param name="instance">Instance.</param>
        public static void ActivateTags(CObject instance)
        {
            foreach(string tag in instance.template.tags)
            {
                if (activeTags.ContainsKey(tag))
                {
                    if (!activeTags[tag].Contains(instance))
                        activeTags[tag].Add(instance);
                }
                else
                {
                    activeTags.Add(tag, new List<CObject>());
                    activeTags[tag].Add(instance);
                }
            }
        }

        /// <summary>
        /// Removes the Object from Active Tags
        /// </summary>
        /// <param name="instance">Instance.</param>
        public static void RemoveTags(CObject instance)
        {
            foreach(string tag in instance.template.tags)
            {
                activeTags[tag].Remove(instance);
            }
        }

        /// <summary>
        /// Add Behaviours to Spawned Objects from it's List of Behaviours in the Object Template
        /// </summary>
        private static void AddBehaviours(GameObject spawnedObject, string ID)
        {
            CObjectTemplate template = GetTemplate(ID);

            if (template != null)
            {
                if (template.behaviours.Count > 0)
                {
                    foreach (BehaviourTemplate behaviour in template.behaviours)
                    {
                        BehaviourModule.AddBehaviour(spawnedObject, behaviour);
                    }
                }
            }
            else
                Debug.LogWarning("Can't add nonexistent "+ID+" Behaviour to " + spawnedObject);
        }

        /// <summary>
        /// Get an Object Template by it's ID
        /// </summary>
        /// <returns>The template.</returns>
        /// <param name="ID">Identifier.</param>
        private static CObjectTemplate GetTemplate(string ID)
        {
            foreach(CObjectTemplate template in templates)
            {
                if (template.ID == ID)
                    return template;
            }
            return null;
        }
    }
}
