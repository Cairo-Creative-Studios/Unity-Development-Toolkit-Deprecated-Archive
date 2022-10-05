/*! \addtogroup objectmodule Object Module
 *  Additional documentation for group 'Object Module'
 *  @{
 */

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections.Generic;
using UnityEngine;

namespace UDT.Objects
{
    /// <summary>
    /// The Object Module interfaces with Cairo Objects, and enables simple creation of complex Objects within the game, with very little input from the user. Using Scriptable Objects, the Toolkit can be used to create pretty much any kind of Gameplay scenario without any code at all.
    /// </summary>
    public class ObjectModule : MonoBehaviour
    {
        public bool initialize = true;
        /// <summary>
        /// The Object Module Singleton
        /// </summary>
        public static ObjectModule singleton;
        /// <summary>
        /// When true, all Game Objects implement Object Extension Components when they're created
        /// </summary>
        [Tooltip("When true, all Game Objects implement Object Extension Components when they're created")]
        public bool implementGlobally = true;

        /// <summary>
        /// The objects that the Engine is controlling.
        /// </summary>
        private static List<ObjectExtension> objects = new List<ObjectExtension>();
        /// <summary>
        /// The Game Objects that have been added to the Spawn Pool
        /// </summary>
        [Tooltip("The Spawn Pools controlled by the Object Module")]
        [SerializeField] private Dictionary<string, Queue<GameObject>> spawnPool = new Dictionary<string, Queue<GameObject>>();
        /// <summary>
        /// Tags for Objects that have been created with Object Extensions
        /// </summary>
        [SerializeField] private Dictionary<string, List<ObjectExtension>> activeTags = new Dictionary<string, List<ObjectExtension>>();

        /// <summary>
        /// The current Spawn Pool
        /// </summary>
        private static GameObject spawnPoolContainer;

        /// <summary>
        /// The current Moving Speed of the Object
        /// </summary>
        [Tooltip("The current Moving Speed of the Object")]
        public Vector3 Velocity;

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            GameObject singletonObject = new GameObject();
            singleton = singletonObject.AddComponent<ObjectModule>();
            singletonObject.name = "Cairo Object Module";
            DontDestroyOnLoad(singletonObject);
            if (!singleton.initialize)
                Object.Destroy(singletonObject);
        }

        #region Objects
        ///<summary>
        /// Adds an Object to the Object list
        ///</summary>
        public static void AddObject(ObjectExtension addedObject)
        {
            objects.Add(addedObject);
        }

        /// <summary>
        /// Creates a new Object and Returns it.
        /// </summary>
        /// <returns>The object.</returns>
        public static ObjectExtension CreateObject()
        {
            GameObject createdGameObject = new GameObject();
            ObjectExtension createdObject = createdGameObject.AddComponent<ObjectExtension>();
            objects.Add(createdObject);
            createdObject.OID = objects.Count - 1;
            return createdObject;
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

            if (singleton.spawnPool.ContainsKey(ID))
            {
                if (singleton.spawnPool[ID].Count > 0)
                {
                    spawnedObject = singleton.spawnPool[ID].Dequeue();
                    spawnedObject.SetActive(true);

                    //AddBehaviours(spawnedObject, ID);

                    return spawnedObject;
                }
            }


            //ObjectTemplate template = GetTemplate(ID);

            //if (template != null)
            //{
            //    if (template.prefab != null)
            //    {
            //        spawnedObject = new GameObject();
            //        spawnedObject.transform.position = spawner.position;
            //        spawnedObject.transform.eulerAngles = spawner.eulerAngles;
            //        ObjectExtension objectBehaviour = spawnedObject.AddComponent<ObjectExtension>();
            //        objectBehaviour.template = template;

            //        objectBehaviour.poolID = ID;

            //        AddBehaviours(spawnedObject, ID);

            //        return spawnedObject;
            //    }
            //}

            return null;
        }

        /// <summary>
        /// Spawns the Game Object and adds it to the Spawn Pool.
        /// </summary>
        /// <returns>The spawn.</returns>
        public static GameObject Spawn(ObjectTemplate template, Transform spawner)
        {
            //The Object that is Spawned
            GameObject spawnedObject;

            if (template != null)
            {
                if (template.prefab != null)
                {
                    spawnedObject = new GameObject();
                    spawnedObject.transform.position = spawner.position;
                    spawnedObject.transform.eulerAngles = spawner.eulerAngles;
                    ObjectExtension objectBehaviour = spawnedObject.AddComponent<ObjectExtension>();
                    objectBehaviour.poolID = template.poolID;
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
            string ID = gameObject.GetComponent<ObjectExtension>().poolID;

            if (!singleton.spawnPool.ContainsKey(ID))
                singleton.spawnPool.Add(ID, new Queue<GameObject>());

            singleton.spawnPool[ID].Enqueue(gameObject);
            gameObject.SetActive(false);
        }
    }
}
