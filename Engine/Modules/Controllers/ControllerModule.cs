/*! \addtogroup controllermodule Controller Module
 *  Additional documentation for group 'Controller Module'
 *  @{
 */

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Reflection;

namespace CairoEngine.Controllers
{
    /// <summary>
    /// Manages Controllers in the game, their ownership, the types of Controllers to create, Entity Possesion and more.
    /// </summary>
    public class ControllerModule : MonoBehaviour
    {
		public bool initialize = true;

        /// <summary>
        /// The Controller Module Singleton
        /// </summary>
        public static ControllerModule singleton;

        /// <summary>
        /// Controller Templates loaded from the Resources folder
        /// </summary>
        private static List<ControllerTemplate> templates = new List<ControllerTemplate>();
        /// <summary>
        /// The currently active Controllers in the game
        /// </summary>
        [SerializeField] private List<Controller> controllers = new List<Controller>();

        private SDictionary<ControllerTemplate, List<Controller>> templateControllerGroups = new SDictionary<ControllerTemplate, List<Controller>>();
        /// <summary>
        /// Objects that have been possessed by a Controller
        /// </summary>
        private List<GameObject> possessedObjects = new List<GameObject>();

        private static int deviceCount = 0;

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            GameObject singletonObject = new GameObject();
            singleton = singletonObject.AddComponent<ControllerModule>();
            GameObject.DontDestroyOnLoad(singleton);
            singleton.name = "Cairo Controller Module";
            templates.AddRange(Resources.LoadAll<ControllerTemplate>(""));

			if (!singleton.initialize)
				GameObject.Destroy(singletonObject);
        }

        /// <summary>
        /// Updates the controllers and their Controlled Entities
        /// </summary>
        void Update()
        {
            foreach(Controller controller in controllers)
            {
                foreach(GameObject entity in controller.possessedObjects)
                {
                    MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
                    foreach(object component in components)
                    {
                        component.CallMethod("SetInputs",new object[] { controller.inputs });
                    }
                }
            }
        }

        public static void AddTemplate(ControllerTemplate template)
        {
            templates.Add(template);
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
                name = "Player Controller " + singleton.controllers.Count
            };
            PlayerController playerController = gameObject.AddComponent<PlayerController>();
            gameObject.transform.parent = singleton.transform;
            singleton.controllers.Add(playerController);
            playerController.checkedIn = true;

            if (template != null)
                playerController.template = template;

            if(template.inputMap!= null)
            {
                foreach (string inputName in template.inputMap.inputs.Keys)
                {
                    playerController.inputs.Add(inputName, 0);
                }
            }

            playerController.inputActions = template.inputMap.inputs;

            return playerController;
        }

        public static int CreateDevice()
        {
            return deviceCount++;
        }

        public static PlayerController CreatePlayerController(int device, ControllerTemplate template)
		{
			GameObject gameObject = new GameObject
			{
				name = "Player Controller " + singleton.controllers.Count
			};
			PlayerController playerController = gameObject.AddComponent<PlayerController>();
			//gameObject.transform.parent = Engine.singleton.transform;
			singleton.controllers.Add(playerController);
			playerController.checkedIn = true;

			if (template != null)
				playerController.template = template;

			if (template.inputMap != null)
			{
				foreach (string inputName in template.inputMap.inputs.Keys)
				{
					playerController.inputs.Add(inputName, 0);
				}
			}

			playerController.inputActions = template.inputMap.inputs;

            if (singleton.templateControllerGroups.ContainsKey(template))
            {
                singleton.templateControllerGroups[template].Add(playerController);
            }
            else
            {
                singleton.templateControllerGroups.Add(template, new List<Controller>());
                singleton.templateControllerGroups[template].Add(playerController);
            }

            return playerController;
		}

		/// <summary>
		/// Checks in a Controller if it hasn't already been added in the Controller Module
		/// </summary>
		/// <param name="controller">The Controller to add</param>
		public static void CheckIn(Controller controller)
        {
            singleton.controllers.Add(controller);
            controller.checkedIn = true;
        }

        /// <summary>
        /// Possess the specified Entity with a Controller
        /// </summary>
        /// <param name="controller">The Controller that will possess the Entity.</param>
        /// <param name="entity">The Entity to possess</param>
        public static void Possess(Controller controller, GameObject objectToPossess)
        {
            controller.possessedObjects.Add(objectToPossess);
        }

        /// <summary>
        /// Possess the specified Entity with a Controller
        /// </summary>
        /// <param name="controllerIndex">The Index of the Controller in the Controller Module</param>
        /// <param name="entity">The Entity to possess</param>
        public static void Possess(int controllerIndex, GameObject entity)
        {
            if (entity.GetField("controller") == null)
            {
                Controller curController = singleton.controllers[controllerIndex];
                curController.possessedObjects.Add(entity);
            }
        }

        /// <summary>
        /// Returns a controller that has yet to Possess any Objects
        /// </summary>
        /// <returns>The free controller.</returns>
        public static Controller GetFreeController()
        {
            foreach(Controller controller in singleton.controllers)
            {
                if (controller.possessedObjects.Count<1)
                {
                    return controller;
                }
            }
            return null;
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
                if (template.ID == ID||ID=="")
                    return template;
            }
            return null;
        }
    }
}
