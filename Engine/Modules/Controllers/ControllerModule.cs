using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// Manages Controllers in the game, their ownership, the types of Controllers to create, Entity Possesion and more.
    /// </summary>
    public class ControllerModule
    {
        /// <summary>
        /// The currently active Controllers in the game
        /// </summary>
        private static List<Controller> controllers = new List<Controller>();

        public static void Init()
        {
            //Create the first Player 
            CreatePlayerController(-1);
        }

        /// <summary>
        /// Updates the controllers and their Controlled Entities
        /// </summary>
        public static void Update()
        {

        }

        /// <summary>
        /// Creates an AI Controller in the Game.
        /// </summary>
        public static void CreateAIController()
        {
            GameObject newAIControllerObject = new GameObject();

        }

        ///<summary>
        /// Adds a Player Controller to game for the given Device
        ///</summary>
        public static PlayerController CreatePlayerController(int device)
        {
            GameObject gameObject = new GameObject
            {
                name = "Player Controller " + controllers.Count
            };
            PlayerController playerController = gameObject.AddComponent<PlayerController>();
            gameObject.transform.parent = Engine.singleton.transform;
            controllers.Add(playerController);
            playerController.checkedIn = true;

            return playerController;
        }

        /// <summary>
        /// Checks in a Controller if it hasn't already been added in the Controller Module
        /// </summary>
        /// <param name="controller">The Controller to add</param>
        public static void CheckIn(Controller controller)
        {
            controllers.Add(controller);
            controller.checkedIn = true;
        }

        /// <summary>
        /// Possess the specified Entity with a Controller
        /// </summary>
        /// <param name="controller">The Controller that will possess the Entity.</param>
        /// <param name="entity">The Entity to possess</param>
        public static void Possess(Controller controller, Entity entity)
        {
            if (entity.controller == null)
                controller.possessedPawn = entity;
        }
    }
}
