using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace CairoEngine
{
    /// <summary>
    /// The Engine class acts as a hub for all Components, Modules, and Objects. 
    /// It should be accessed for control over all of the Game's Elements.
    /// </summary>
    public class Engine : MonoBehaviour
    {
        public static Engine singleton;

        /// <summary>
        /// The Runtime controls the overall flow of the Game by interacting with the Engine. 
        /// </summary>
        public Runtime runtime;

        /// <summary>
        /// Inialize the Game Engine.
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        public static void Inialize()
        {
            Scene previousScene = SceneManager.GetActiveScene();
            Scene engineScene = SceneManager.CreateScene("Engine");
            SceneManager.SetActiveScene(engineScene);
            GameObject engineObject = new GameObject
            {
                name = "Engine"
            };
            engineObject.AddComponent<Engine>();
            SceneManager.SetActiveScene(previousScene);
        }

        #region Engine Core
        void Start()
        {
            //Set the Singleton
            singleton = this;

            //Create the Game Runtime
            GameObject runtimeGameObject = new GameObject
            {
                name = "Runtime"
            };
            runtimeGameObject.transform.parent = singleton.transform;
            runtime = runtimeGameObject.AddComponent<Runtime>();

            //Initialize Managers
            BehaviourModule.Init();
            CameraModule.Init();
            ControllerModule.Init();
            EntityModule.Init();
            GameModeModule.Init();
            InventoryModule.Init();
            LevelModule.Init();
            MLModule.Init();
            ObjectModule.Init();
            PlayerModule.Init();
            StateMachineModule.Init();
            BehaviourModule.Init();
            UIModule.Init();
        }

        void Update()
        {
            //Update Managers
            BehaviourModule.Update();
            CameraModule.Update();
            ControllerModule.Update();
            EntityModule.Update();
            GameModeModule.Update();
            InventoryModule.Update();
            LevelModule.Update();
            MLModule.Update();
            ObjectModule.Update();
            PlayerModule.Update();
            StateMachineModule.Update();
            BehaviourModule.Update();
            UIModule.Update();
        }
        #endregion

        #region Global Methods
        /// <summary>
        /// Gets the nested types with the Base Type of the given name. 
        /// </summary>
        /// <returns>The nested types from base.</returns>
        /// <param name="type">Type to find Nested Types in</param>
        /// <param name="baseTypeName">Base type name.</param>
        public static Type[] GetNestedTypesOfBase(Type type, string baseTypeName)
        {
            Type[] types = type.GetNestedTypes();
            List<Type> desiredTypes = new List<Type>();

            foreach (Type curType in types)
            {
                if (curType.BaseType.Name == "State")
                {
                    desiredTypes.Add(curType);
                }
            }

            return desiredTypes.ToArray();
        }

        /// <summary>
        /// Creates an instance of a Prefab, running initialization code on it through the Engine's Modules afterward.
        /// </summary>
        /// <param name="prefab">The Prefab to create a new GameObject from</param>
        public static GameObject CreatePrefabInstance(GameObject prefab)
        {
            GameObject createdInstance = Object.Instantiate(prefab);
            UIModule.CheckIn(createdInstance, prefab);
            LevelModule.CheckIn(createdInstance);
            return createdInstance;
        }
        #endregion
    }
}