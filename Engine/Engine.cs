/*! \mainpage Cairo Gameplay Toolkit
 *
 * \section intro_sec Introduction
 *
 * Welcome to the Cairo Creative Studios Gameplay Toolkit Documentation! Here you will find all the information on elements of the Toolkit, as well as examples on how to use it! 
 *
 * \section install_sec Installation
 * Download the most recent released Unity Package from here: https://github.com/Cairo-Creative-Studios/Cairo-Gameplay-Toolkit/releases
 * \n 
 * You can also purchase it from Itch.io if you're interested in donating to the Toolkit's development: https://cairocreative.itch.io/cairo-engine
 */

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

        public static bool started = false;
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
            started = true;
        }
        #endregion
    }
}