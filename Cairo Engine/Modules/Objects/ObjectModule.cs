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
        private static List<Object> objects = new List<Object>();
        /// <summary>
        /// The Game Objects that have been added to the Spawn Pool
        /// </summary>
        private static Dictionary<string,Queue<GameObject>> spawnPool = new Dictionary<string,Queue<GameObject>>();

        private static List<ObjectTemplate> templates = new List<ObjectTemplate>();

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
            templates.AddRange(Resources.LoadAll<ObjectTemplate>(""));
        }

        public static void Update()
        {

        }

        #region Objects
        ///<summary>
        /// Adds an Object to the Object list
        ///</summary>
        public static void AddObject(Object addedObject)
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
        public static Object CreateObject(string name)
        {
            GameObject spekObject = new GameObject();
            Object spekBehaviour = spekObject.AddComponent<Object>();
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
        public static GameObject Spawn(string ID)
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

            spawnedObject = new GameObject();
            CairoEngine.Object objectBehaviour = spawnedObject.AddComponent<CairoEngine.Object>();
            objectBehaviour.poolID = ID;

            AddBehaviours(spawnedObject, ID);

            return spawnedObject;
        }

        public static GameObject SpawnFromPrefab(GameObject prefab, string ID)
        {
            GameObject spawnedObject;
            ObjectTemplate objectTemplate = GetTemplate(ID);

            if (spawnPool.ContainsKey(objectTemplate.poolID + "_" + ID))
            {
                if (spawnPool[objectTemplate.poolID + "_" + ID].Count > 0)
                {
                    spawnedObject = spawnPool[objectTemplate.poolID + "_" + ID].Dequeue();
                    spawnedObject.SetActive(true);

                    AddBehaviours(spawnedObject, ID);

                    return spawnedObject;
                }
            }

            spawnedObject = Engine.CreatePrefabInstance(prefab);
            CairoEngine.Object objectBehaviour = spawnedObject.AddComponent<CairoEngine.Object>();

            objectBehaviour.template = objectTemplate;

            objectBehaviour.poolID = objectTemplate.poolID + "_" + ID;

            objectBehaviour.OnCreate();

            AddBehaviours(spawnedObject, ID);

            return spawnedObject;
        }

        /// <summary>
        /// Destroying the Object adds the Object to the Spawn Pool and Deactivates it.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        public static void Destroy(GameObject gameObject)
        {
            string ID = gameObject.GetComponent<Object>().poolID;

            if (!spawnPool.ContainsKey(ID))
                spawnPool.Add(ID,new Queue<GameObject>());

            spawnPool[ID].Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Add Behaviours to Spawned Objects from it's List of Behaviours in the Object Template
        /// </summary>
        private static void AddBehaviours(GameObject spawnedObject, string ID)
        {
            ObjectTemplate template = GetTemplate(ID);

            if (template != null)
            {
                if (template.behaviours.Count > 0)
                {
                    foreach (BehaviourTypeTemplate behaviour in template.behaviours)
                    {
                        Debug.Log("Added " + behaviour.ID);
                        BehaviourModule.AddBehaviour(spawnedObject, behaviour.ID);
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
        private static ObjectTemplate GetTemplate(string ID)
        {
            foreach(ObjectTemplate template in templates)
            {
                if (template.ID == ID)
                    return template;
            }
            return null;
        }
    }
}
