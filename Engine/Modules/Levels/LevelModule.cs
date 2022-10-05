/*! \addtogroup levelmodule Level Module
 *  Additional documentation for group 'Level Module'
 *  @{
 */

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UDT
{
    /// <summary>
    /// The Level Manager is a layer on top of Scene Management that allows you to spawn and manipulate the Scene as a whole. Levels are also used for Level Streaming, Saving and Loading Level States, and Level specific objects like Spawners.
    /// </summary>
    public class LevelModule
    {

        // Abstract Objects
        /// <summary>
        /// The spawn points that currently exist in the Game.
        /// </summary>
        public static List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        /// <summary>
        /// The levels that are currently loaded into the game.
        /// </summary>
        private static List<Level> levels = new List<Level>();

        /// <summary>
        /// The levels that can be loaded into the Game
        /// </summary>
        private static List<LevelTemplate> levelInfos = new List<LevelTemplate>();

        public static Level currentLevel;

        public static void Init()
        {
            levelInfos.AddRange(Resources.LoadAll<LevelTemplate>(""));
            CreateLevelForScene(SceneManager.GetActiveScene());
        }

        public static void Update()
        {

        }

        /// <summary>
        /// Creates a Level for the given Scene.
        /// </summary>
        /// <param name="scene">Scene.</param>
        public static Level CreateLevelForScene(Scene scene)
        {
            GameObject levelObject = new GameObject
            {
                name = "Level_" + scene.name
            };
            Level level = levelObject.AddComponent<Level>();
            level.template = GetLevelInfoForScene(scene.name);

            GameObject[] gameObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in gameObjects)
            {
                rootObject.transform.parent = level.transform;
                SpawnPoint rootObjectAsSpawn = rootObject.GetComponent<SpawnPoint>();

                if (rootObjectAsSpawn != null)
                    spawnPoints.Add(rootObjectAsSpawn);

            }

            currentLevel = level;

            return level;
        }

        /// <summary>
        /// Loads the given level.
        /// </summary>
        /// <param name="ID">
        /// The ID of the level to load.
        /// </param>
        public static void LoadLevel(string ID)
        {
            foreach (LevelTemplate levelInfo in levelInfos)
            {
                if (levelInfo.ID == ID)
                {
                    GameObject levelObject = new GameObject
                    {
                        name = "Level_" + levelInfo.ID
                    };
                    Level level = levelObject.AddComponent<Level>();
                    level.template = levelInfo;
                    levels.Add(level);
                    return;
                }
            }
            Debug.LogWarning("Level " + ID + " not found when Load was requested.");
        }

        /// <summary>
        /// Draws the given level.
        /// </summary>
        /// <param name="ID">Identifier.</param>
        public static void DrawLevel(string ID)
        {
            int index = FindLevelIndex(ID);
            if (index != -1)
                levels[index].Draw();
        }

        /// <summary>
        /// Removes the level from the game.
        /// </summary>
        /// <param name="ID">Identifier.</param>
        public static void RemoveLevel(string ID)
        {
            int index = FindLevelIndex(ID);
            if (index != -1)
            {
                levels.RemoveAt(index);
                levels[index].Destroy();
            }
        }

        public static void MoveLevel(string ID, Vector3 moveAmount)
        {
            levels[FindLevelIndex(ID)].transform.position += moveAmount;
        }

        /// <summary>
        /// Gets the Transform of the Current Level
        /// </summary>
        /// <returns>The transform.</returns>
        public static Transform GetTransform()
        {
            return currentLevel.transform;
        }

        /// <summary>
        /// Gets the Transform of the specified Level
        /// </summary>
        /// <returns>The transform.</returns>
        /// <param name="ID">Identifier.</param>
        public static Transform GetTransform(string ID)
        {
            return GetLevel(ID).transform;
        }

        /// <summary>
        /// Checks in the passed Object, adding it to the Current Level.
        /// </summary>
        /// <param name="checkedObject">Checked object.</param>
        public static void CheckIn(GameObject checkedObject)
        {
            checkedObject.transform.parent = currentLevel.transform;
        }

        public static Transform GetSpawn()
        {
            Debug.Log(spawnPoints[0].transform.position);
            return spawnPoints[0].transform;
        }

        /// <summary>
        /// Finds the index of the level.
        /// </summary>
        /// <returns>The level index.</returns>
        /// <param name="ID">Identifier.</param>
        private static int FindLevelIndex(string ID)
        {
            int index = -1;
            foreach (Level level in levels)
            {
                index++;
                if (level.template.ID == ID)
                {
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// Gets a Level that has been Loaded with the specified ID
        /// </summary>
        /// <returns>The level.</returns>
        /// <param name="ID">Identifier.</param>
        private static Level GetLevel(string ID)
        {
            return levels[FindLevelIndex(ID)];
        }

        /// <summary>
        /// Searches for Level Info for the Given scene, or creates new Level Info.
        /// </summary>
        /// <returns>The level info for scene.</returns>
        /// <param name="sceneName">Scene name.</param>
        private static LevelTemplate GetLevelInfoForScene(string sceneName)
        {
            foreach (LevelTemplate levelInfo in levelInfos)
            {
                if (levelInfo.sceneName == sceneName)
                {
                    return levelInfo;
                }
            }

            LevelTemplate newLevelInfo = ScriptableObject.CreateInstance<LevelTemplate>();
            newLevelInfo.sceneName = sceneName;
            newLevelInfo.name = sceneName;

            return newLevelInfo;
        }
    }
}
