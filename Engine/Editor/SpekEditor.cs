using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CairoEngine
{
    [InitializeOnLoad]
    public class Editor
    {
        public static List<LevelInfo> levelInfos = new List<LevelInfo>();
        public static List<PawnInfo> pawnInfos = new List<PawnInfo>();
        public static List<GameModeInfo> gameModeInfos = new List<GameModeInfo>();
        public static List<InventoryItemInfo> itemInfos = new List<InventoryItemInfo>();

        [InitializeOnLoadMethod]
        public static async Task Init()
        {
            //Load Data files
            levelInfos.AddRange(Resources.LoadAll<LevelInfo>(""));
            pawnInfos.AddRange(Resources.LoadAll<PawnInfo>(""));
            gameModeInfos.AddRange(Resources.LoadAll<GameModeInfo>(""));
            itemInfos.AddRange(Resources.LoadAll<InventoryItemInfo>(""));

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
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PawnInfo>(), "Assets/Resources/" + folderName + "/" + folderName + " Info.asset");
            PawnInfo pawnInfo = Resources.Load<PawnInfo>(folderName + " Info");
            //Create the Pawn's Game Object.
            GameObject pawnObject = new GameObject();
            pawnObject.name = folderName;
            //Add the Pawn Component to the new Game Object and set default values.
            Pawn pawn = pawnObject.AddComponent<Pawn>();
            pawn.pawnInfo = pawnInfo;
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
            UnityEngine.Rendering.Volume volume = obj.AddComponent<UnityEngine.Rendering.Volume>();
            volume.isGlobal = false;
            obj.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
        }

        #endregion
    }
}
