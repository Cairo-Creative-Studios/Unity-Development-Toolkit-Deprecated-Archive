//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

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

        private static List<ControllerTemplate> templates = new List<ControllerTemplate>();

        public static void Init()
        {
            //Create the first Player 
            //CreatePlayerController(-1);
            templates.AddRange(Resources.LoadAll<ControllerTemplate>(""));
        }

        /// <summary>
        /// Updates the controllers and their Controlled Entities
        /// </summary>
        public static void Update()
        {
            foreach(Controller controller in controllers)
            {
                foreach(Entity entity in controller.possessedEntities)
                {
                    entity.inputs = controller.inputs;
                }
            }
        }

        /// <summary>
        /// Creates an AI Controller in the Game.
        /// </summary>
        public static void CreateAIController(string ID = "")
        {
            GameObject AIControllerObject = new GameObject();
            AIController aIController = AIControllerObject.AddComponent<AIController>();
            ControllerTemplate template = GetTemplate(ID);

            if (template != null)
                aIController.template = template;

            foreach (string inputName in template.inputMap.inputs.Keys)
            {
                aIController.inputs.Add(inputName, 0);
            }
        }

        ///<summary>
        /// Adds a Player Controller to game for the given Device
        ///</summary>
        public static PlayerController CreatePlayerController(int device, string ID = "")
        {
            ControllerTemplate template = GetTemplate(ID);

            GameObject gameObject = new GameObject
            {
                name = "Player Controller " + controllers.Count
            };
            PlayerController playerController = gameObject.AddComponent<PlayerController>();
            gameObject.transform.parent = Engine.singleton.transform;
            controllers.Add(playerController);
            playerController.checkedIn = true;

            if (template != null)
                playerController.template = template;

            foreach(string inputName in template.inputMap.inputs.Keys)
            {
                playerController.inputs.Add(inputName,0);
            }

            playerController.inputActions = template.inputMap.inputs;

            foreach(string key in playerController.inputActions.Keys)
            {
                playerController.inputActions[key].Enable();
            }

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
            {
                entity.controller = controller;
                controller.possessedEntities.Add(entity);
            }
        }

        /// <summary>
        /// Gets a Template by ID
        /// </summary>
        /// <returns>The template.</returns>
        /// <param name="ID">Identifier.</param>
        private static ControllerTemplate GetTemplate(string ID)
        {
            foreach(ControllerTemplate template in templates)
            {
                if (template.ID == ID)
                    return template;
            }
            return null;
        }
    }
}
