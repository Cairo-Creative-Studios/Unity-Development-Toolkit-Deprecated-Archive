using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// Controls Entities (Including Objects that extend from the base Entity class), and their presence in the Game.
    /// </summary>
    public class EntityModule
    {
        /// <summary>
        /// The entities currently in the Game
        /// </summary>
        private static List<Entity> entities = new List<Entity>();
        /// <summary>
        /// The pawns currently in the Game
        /// </summary>
        private static List<Pawn> pawns = new List<Pawn>();


        /// <summary>
        /// The info files of all the Pawns that can be loaded into the game.
        /// </summary>
        private static List<PawnInfo> pawnInfos = new List<PawnInfo>();


        public static void Init()
        {
            pawnInfos.AddRange(Resources.LoadAll<PawnInfo>(""));
        }

        public static void Update()
        {

        }

        #region Entities
        /// <summary>
        /// Adds a Character Controller to an Entity
        /// </summary>
        /// <param name="entity">The Entity to add a Character Controller to</param>
        /// <param name="hierarchy"></param>
        public static void EntityAddCharacterController(Entity entity, string hierarchy = "")
        {

        }
        /// <summary>
        /// Adds a First Person Camera to the Entity
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="hierarchy">Hierarchy.</param>
        public static void EntityAddFPSCamera(Entity entity, string hierarchy = "")
        {

        }
        /// <summary>
        /// Adds a Third PErson Camera to the Entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="hierarchy">Hierarchy.</param>
        public static void EntityAddThirdPersonCamera(Entity entity, string hierarchy = "")
        {

        }
        #endregion

        #region Pawns
        /// <summary>
        /// Spawns a Pawn into the Game.
        /// </summary>
        /// <returns>The pawn.</returns>
        /// <param name="ID">Identifier.</param>
        /// <param name="type">The name of the Pawn class to give to the Spawned Pawn</param>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        public static Pawn SpawnPawn(string ID, string type, Vector3 position, Vector3 rotation = default(Vector3), object controller = null)
        {
            PawnInfo pawnInfo = GetPawnInfo(ID);

            if (pawnInfo != null)
            {
                GameObject newPawnObject = pawnInfo.prefab;
                newPawnObject.transform.position = position;
                newPawnObject.transform.eulerAngles = rotation;
                Pawn newPawn = (Pawn)newPawnObject.AddComponent(Type.GetType(type));
                newPawn.controller = controller;
                newPawn.pawnInfo = pawnInfo;
                newPawn.entityInfo = pawnInfo;

                return newPawn;
            }

            return null;
        }
        /// <summary>
        /// Spawns a Pawn into the Game.
        /// </summary>
        /// <returns>The pawn.</returns>
        /// <param name="ID">Identifier.</param>
        /// <param name="type">The name of the Pawn class to give to the Spawned Pawn</param>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        public static void SpawnPawn(PawnInfo pawnInfo,Vector3 position, Vector3 rotation = default(Vector3), object controller = null)
        {
           GameObject pawnObject = UnityEngine.Object.Instantiate(pawnInfo.prefab);
            pawnObject.name = "Foo";
            pawnObject.transform.position = position;
            pawnObject.transform.eulerAngles = rotation;
            
            Debug.Log("Spawned Pawn, " + pawnObject.name + " with Pawn Info " + pawnInfo);

            Pawn newPawn = (Pawn)pawnObject.AddComponent(Type.GetType("Pawn"));
            newPawn.controller = controller;
            newPawn.pawnInfo = pawnInfo;
            newPawn.entityInfo = pawnInfo;
            return ;
        }

        /// <summary>
        /// Kills the pawn
        /// </summary>
        /// <param name="pawn">Pawn.</param>
        /// <param name="damageInstegator">Damage instegator, holds information about how the Pawn died.</param>
        public static void KillPawn(Pawn pawn, DamageInstegator damageInstegator)
        {

        }

        /// <summary>
        /// Gets the Pawn's prefab after finding it by it's ID.
        /// </summary>
        /// <param name="ID">Identifier.</param>
        private static GameObject GetPawnPrefab(string ID)
        {
            foreach (PawnInfo curPawn in pawnInfos)
            {
                if (curPawn.ID == ID)
                {
                    return curPawn.prefab;
                }
            }
            return null;
        }

        private static PawnInfo GetPawnInfo(string ID)
        {
            foreach (PawnInfo pawnInfo in pawnInfos)
            {
                if (pawnInfo.ID == ID)
                {
                    return pawnInfo;
                }
            }
            return null;
        }
        #endregion
    }
}