//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;
using B83.Unity.Attributes;
using CairoEngine.Reflection;
using CairoEngine.StateMachine;

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
        /// The Modules that are Available in the Engine
        /// </summary>
        public static List<string> modules = new List<string>();

        public static List<string> flags = new List<string>();


        public SDictionary<string, GameObject> enginePrefabs = new SDictionary<string, GameObject>();

        public static RuntimeTemplate runtimeTemplate;
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
            RuntimeTemplate[] runtimeTemplates = Resources.LoadAll<RuntimeTemplate>("");
            runtimeTemplate = runtimeTemplates[0];

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

            //Initialize Modules
            BehaviourModule.Init();
            ControllerModule.Init();
            GameModeModule.Init();
            InputModule.Init();
            LevelModule.Init();
            MLModule.Init();
            ObjectModule.Init();
            PlayerModule.Init();
            BehaviourModule.Init();
            UIModule.Init();

            //Create the Game Runtime
            GameObject runtimeGameObject = new GameObject
            {
                name = "Runtime"
            };
            runtimeGameObject.transform.parent = Engine.singleton.transform;

            var importedRuntime = runtimeGameObject.AddComponent(Type.GetType(runtimeTemplate.runtimeClass));
            Engine.singleton.runtime = (Runtime)importedRuntime;

            StateMachineModule.CreateStateMachine(importedRuntime);
        }

        void Update()
        {
            //Update Modules
            BehaviourModule.Update();
            ControllerModule.Update();
            GameModeModule.Update();
            LevelModule.Update();
            MLModule.Update();
            ObjectModule.Update();
            PlayerModule.Update();
            BehaviourModule.Update();
            UIModule.Update();
            StateMachineModule.Update();
        }

        void FixedUpdate()
        {
            //FixedUpdate Modules
            BehaviourModule.FixedUpdate();
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
                if (curType.BaseType.Name == baseTypeName)
                {
                    desiredTypes.Add(curType);
                }

                Debug.Log(curType);
            }

            return desiredTypes.ToArray();
        }

        /// <summary>
        /// Creates an instance of a Prefab, running initialization code on it through the Engine's Modules afterward.
        /// </summary>
        /// <param name="prefab">The Prefab to create a new GameObject from</param>
        public static GameObject CreatePrefabInstance(GameObject prefab)
        {
            GameObject createdInstance = GameObject.Instantiate(prefab);
            UIModule.CheckIn(createdInstance, prefab);
            LevelModule.CheckIn(createdInstance);
            return createdInstance;
        }


        #endregion
    }
}