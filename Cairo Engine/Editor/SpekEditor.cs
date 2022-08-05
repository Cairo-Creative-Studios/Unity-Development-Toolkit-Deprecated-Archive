using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace CairoEngine
{
    [InitializeOnLoad]
    public class Editor
    {
        public static List<LevelTemplate> levelInfos = new List<LevelTemplate>();
        public static List<GameModeTemplate> gameModeInfos = new List<GameModeTemplate>();

        [InitializeOnLoadMethod]
        public static async Task Init()
        {
            //Load Data files
            levelInfos.AddRange(Resources.LoadAll<LevelTemplate>(""));
            gameModeInfos.AddRange(Resources.LoadAll<GameModeTemplate>(""));

            while (true)
            {
                Update();
                await Task.Delay(8);
            }
        }

        public static void Update()
        {
        }

        #region Menu Items


        [MenuItem("Cairo/Level Tools/Create Spawn Point")]
        private static void CreateSpawnPoint()
        {
            GameObject spawnerObject = new GameObject();
            spawnerObject.name = "Spawner";
            spawnerObject.AddComponent<SpawnPoint>();
            spawnerObject.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
        }

        [MenuItem("Cairo/Level Tools/Create Kill Zone")]
        private static void CreateKillZone()
        {
            GameObject obj = new GameObject();
            obj.name = "Kill Zone";
            obj.AddComponent<BoxCollider>();
            Volume volume = obj.AddComponent<Volume>();
            volume.isGlobal = false;
            obj.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
        }

        #endregion
    }
}
