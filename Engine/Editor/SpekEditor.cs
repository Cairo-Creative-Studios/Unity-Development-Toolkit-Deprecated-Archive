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
        public static List<PawnTemplate> pawnInfos = new List<PawnTemplate>();
        public static List<GameModeTemplate> gameModeInfos = new List<GameModeTemplate>();
        public static List<InventoryItemTemplate> itemInfos = new List<InventoryItemTemplate>();

        [InitializeOnLoadMethod]
        public static async Task Init()
        {
            //Load Data files
            levelInfos.AddRange(Resources.LoadAll<LevelTemplate>(""));
            pawnInfos.AddRange(Resources.LoadAll<PawnTemplate>(""));
            gameModeInfos.AddRange(Resources.LoadAll<GameModeTemplate>(""));
            itemInfos.AddRange(Resources.LoadAll<InventoryItemTemplate>(""));

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
        [MenuItem("Cairo/Data/New Pawn")]
        private static void CreateLevelData()
        {
            string folderName = "Pawn";

            AssetDatabase.CreateFolder("Assets/Resources", "Pawn");
            //Create the Pawn Info Asset.
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PawnTemplate>(), "Assets/Resources/" + folderName + "/" + folderName + " Info.asset");
            PawnTemplate pawnInfo = Resources.Load<PawnTemplate>(folderName + " Info");
            //Create the Pawn's Game Object.
            GameObject pawnObject = new GameObject();
            pawnObject.name = folderName;
            //Add the Pawn Component to the new Game Object and set default values.
            Pawn pawn = pawnObject.AddComponent<Pawn>();
            pawn.pawnTemplate = pawnInfo;
            pawn.entityInfo = pawnInfo;
            //Create the prefab for the Game OBject and give it to the Pawn Info.

            GameObject pawnPrefab = PrefabUtility.SaveAsPrefabAsset(pawnObject, AssetDatabase.GetAssetPath(pawnInfo));

            AssetDatabase.CreateAsset(pawnObject, "Assets/Resources/" + folderName + "/Pawn.asset");
            //pawnInfo.prefab = pawnPrefab;
            //Destroy the Game Object from the Hierarchy.
            UnityEngine.Object.Destroy(pawnObject);
        }

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
