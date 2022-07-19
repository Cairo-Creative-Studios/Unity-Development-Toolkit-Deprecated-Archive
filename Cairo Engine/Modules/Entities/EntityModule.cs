//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

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
        /// The Vehicles currently in the Game
        /// </summary>
        private static List<Vehicle> vehicles = new List<Vehicle>();

        /// <summary>
        /// All the Entity Templates in the Project
        /// </summary>
        private static List<EntityTemplate> entityTemplates = new List<EntityTemplate>();
        /// <summary>
        /// All the Pawn Templates in the Project
        /// </summary>
        private static List<PawnTemplate> pawnTemplates = new List<PawnTemplate>();
        /// <summary>
        /// All the Vehicle Templates in the Project
        /// </summary>
        private static List<VehicleTemplate> vehicleTemplates = new List<VehicleTemplate>();

        public static void Init()
        {
            entityTemplates.AddRange(Resources.LoadAll<EntityTemplate>(""));
            pawnTemplates.AddRange(Resources.LoadAll<PawnTemplate>(""));
            vehicleTemplates.AddRange(Resources.LoadAll<VehicleTemplate>(""));
        }

        public static void Update()
        {
            UpdateVehicles();
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
        /// Spawns a Pawn in the Game
        /// </summary>
        /// <returns>The pawn.</returns>
        /// <param name="ID">The ID of the kind of Pawn to Spawn</param>
        /// <param name="spawn">The Transform of the Spawn Point to spawn at</param>
        /// <param name="controller">The Controller we're spawning a Pawn for</param>
        public static Pawn SpawnPawn(string ID, Transform spawn, Controller controller = null)
        {
            return SpawnPawn(ID, spawn.position, spawn.eulerAngles, controller);
        }

        /// <summary>
        /// Spawns a Pawn into the Game.
        /// </summary>
        /// <returns>The pawn.</returns>
        /// <param name="ID">Identifier.</param>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        public static Pawn SpawnPawn(string ID, Vector3 position, Vector3 rotation = default(Vector3), Controller controller = null)
        {
            //Get the Pawn Info by ID
            PawnTemplate pawnTemplate = GetPawnTemplate(ID);

            if (pawnTemplate != null)
            {
                //Spawn a new Pawn Object
                GameObject pawnObject = ObjectModule.SpawnFromPrefab(pawnTemplate.prefab, pawnTemplate.ID);
                pawnObject.name = pawnTemplate.ID;
                pawnObject.transform.position = position;
                pawnObject.transform.eulerAngles = rotation;

                Pawn newPawn = (Pawn)pawnObject.AddComponent(Type.GetType(pawnTemplate.Class));

                if(controller != null)
                    newPawn.controller = controller;

                newPawn.pawnTemplate = pawnTemplate;
                newPawn.entityInfo = pawnTemplate;

                //Enable the State Machine on the Pawn
                StateMachineModule.EnableStateMachine(pawnObject, newPawn);

                //Log Spawn Success
                Debug.Log("Spawned Pawn, " + pawnObject.name + " with Pawn Info " + pawnTemplate);

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
        public static Pawn SpawnPawn(PawnTemplate pawnTemplate,Vector3 position, Vector3 rotation = default(Vector3), Controller controller = null)
        {
            //Spawn a new Pawn Object
            GameObject pawnObject = ObjectModule.SpawnFromPrefab(pawnTemplate.prefab, pawnTemplate.ID);
            pawnObject.name = pawnTemplate.ID;
            pawnObject.transform.position = position;
            pawnObject.transform.eulerAngles = rotation;

            Pawn newPawn = pawnObject.AddComponent<Pawn>();

            //Set the New Pawn's Values
            if (controller != null)
                newPawn.controller = controller;
            newPawn.pawnTemplate = pawnTemplate;
            newPawn.entityInfo = pawnTemplate;

            //Enable the State Machine on the Pawn
            StateMachineModule.EnableStateMachine(pawnObject, newPawn);

            //Log Spawn success
            Debug.Log("Spawned Pawn, " + pawnObject.name + " with Pawn Info " + pawnTemplate);

            return newPawn;
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
            foreach (PawnTemplate curPawn in pawnTemplates)
            {
                if (curPawn.ID == ID)
                {
                    return curPawn.prefab;
                }
            }
            return null;
        }

        private static PawnTemplate GetPawnTemplate(string ID)
        {
            foreach (PawnTemplate pawnTemplate in pawnTemplates)
            {
                if (pawnTemplate.ID == ID)
                {
                    return pawnTemplate;
                }
            }
            return null;
        }
        #endregion

        #region Vehicles
        /// <summary>
        /// Updates the Vehicles in the Game
        /// </summary>
        private static void UpdateVehicles()
        {
            foreach(Vehicle vehicle in vehicles)
            {
                Object vehicleObject = vehicle.GetComponent<Object>();

                //If the Vehicle is currently spawned in the game
                if (vehicleObject.active)
                {
                    //Update the Inputs given to each Seat
                    if (vehicle.seats.Count > 0)
                    {
                        foreach(Seat seat in vehicle.seats.Keys)
                        {
                            if (vehicle.seats[seat] != null)
                                seat.inputs = vehicle.seats[seat].inputs;
                            else
                                seat.inputs.Clear();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Spawns a Vehicle into the Game
        /// </summary>
        /// <returns>The vehicle.</returns>
        /// <param name="ID">Identifier.</param>
        /// <param name="spawn">Spawn.</param>
        public static Vehicle SpawnVehicle(string ID, Transform spawn)
        {
            VehicleTemplate template = GetVehicleTemplate(ID);

            if (template != null)
            {
                if (template.prefab == null)
                    Debug.LogWarning("Vehicle " + ID + " has no Prefab to Spawn, and will not be created!");
                else
                {
                    //Add the Vehicle
                    GameObject vehicleObject = ObjectModule.SpawnFromPrefab(template.prefab, ID);
                    Vehicle vehicle = (Vehicle)vehicleObject.AddComponent(Type.GetType(template.Class));
                    vehicles.Add(vehicle);

                    //Set it's Properties
                    vehicleObject.name = ID;
                    vehicleObject.transform.position = spawn.position;
                    vehicleObject.transform.eulerAngles = spawn.eulerAngles;
                    vehicle.template = template;

                    //Loads it's Seats
                    if (template.seats.Count > 0)
                    {
                        foreach (Seat seat in template.seats)
                        {
                            vehicle.seats.Add(seat, null);
                        }
                    }
                    else
                        Debug.LogWarning("Vehicle " + ID + " has no seats, Pawns and Entitys will not be able to sit in it!");

                    return vehicle;
                }
            }
            else
                Debug.LogWarning("Vehicle " + ID + " not found!");

            return null;
        }

        /// <summary>
        /// Gets the Vehicle Template by name
        /// </summary>
        /// <returns>The vehicle template.</returns>
        /// <param name="ID">Identifier.</param>
        private static VehicleTemplate GetVehicleTemplate(string ID)
        {
            foreach(VehicleTemplate vehicleTemplate in vehicleTemplates)
            {
                if(vehicleTemplate.ID == ID)
                {
                    return vehicleTemplate;
                }
            }
            return null;
        }
        #endregion
    }
}