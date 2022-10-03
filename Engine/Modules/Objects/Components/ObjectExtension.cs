//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using CairoEngine.Controllers;

namespace CairoEngine.Objects
{
    /// <summary>
    /// An Extension to Game 
    /// </summary>
    public class ObjectExtension : CairoBehaviour
    {
        /// <summary>
        /// The Pool ID for the Object, used for Object Pooling
        /// </summary>
        [Tooltip("The ID of the Object Pool this belongs to")]
        public string poolID = "Default";
        /// <summary>
        /// The tags assigned to the Object
        /// </summary>
        [Tooltip("The tags assigned to the Object, for picking")]
        public List<string> tags = new List<string>();

        public enum PossessionStatus
        {
            dontPossess,
            automatic,
            firstPlayer,
            secondPlayer,
            thirdPlayer,
            fourthPlayer
        }
        /// <summary>
        /// The Status of Possession for the Instance Upon Creation
        /// </summary>
        [Tooltip("The Status of Possession for the Instance Upon Creation")]
        public PossessionStatus initialPossessionStatus = 0;
        /// <summary>
        /// If the Controller Template is set, it will force the creation of a Controller Object using the given Template, and possess this Object with it.
        /// </summary>
        [Tooltip("If the Controller Template is set, it will force the creation of a Controller Object using the given Template, and possess this Object with it.")]
        public ControllerTemplate controllerTemplate;

        /// <summary>
        /// The Inputs recieved from the Controller of the Entity.
        /// </summary>
        public SDictionary<string, float> inputs = new SDictionary<string, float>();
        /// <summary>
        /// Whether the Object is active ingame, for Spawn Pool behaviour
        /// </summary>
        [Tooltip("Whether the Object is Active and should be Updated")]
        public bool active = true;
        /// <summary>
        /// The ID of this Object
        /// </summary>
        [Tooltip("Cairo Engine's Unique Identifier for this Object")]
        [ReadOnly] public int OID = -1;
        /// <summary>
        /// The Template belonging to this Object
        /// </summary>
        [Tooltip("The Template Belonging to this Object")]
        public ObjectTemplate template;
        /// <summary>
        /// Properties that are Created/Utilized by the Object's Behaviours
        /// </summary>
        [Tooltip("Properties that are Created/Utilized by the Object's Behaviours")]
        public SDictionary<string, object> properties = new SDictionary<string, object>();
        /// <summary>
        /// Expressions that are Created/Utilized by the Object's Behaviours
        /// </summary>
        [Tooltip("Expressions that are Created/Utilized by the Object's Behaviours")]
        public SDictionary<string, MethodInfo> expressions = new SDictionary<string, MethodInfo>();
        /// <summary>
        /// Flags that have been attached to the Object
        /// </summary>
        [Tooltip("Flags that have been attached to the Object")]
        public List<string> flags = new List<string>();
        /// <summary>
        /// The Level this Object is a part of.
        /// </summary>
        [Tooltip("The Level this Object Belongs to")]
        public Level level;
        /// <summary>
        /// The children of this Object.
        /// </summary>
        [Tooltip("The Objects that have set this Object as their parent")]
        public List<GameObject> children = new List<GameObject>();

        public enum SaveSettings
        {
            Everything,
            Transform,
            Behaviours
        }
        /// <summary>
        /// <see langword="true"/> if waiting for the Engine to Start before Inializing the Instance
        /// </summary>
        //private bool waitForEngine = false;

        //Continuously Update the Values of Properties
        void Update()
        {
            if (active && Engine.started)
            {
                foreach (object property in properties.Values)
                {
                    SetChildProperties(property);
                }
            }
        }

        public override void EngineInitialized()
        {
            if (initialPossessionStatus == PossessionStatus.automatic)
            {
                if (controllerTemplate != null)
                {
                    if (controllerTemplate.isPlayer)
                    {
                        PlayerController playerController = ControllerModule.CreatePlayerController(0, controllerTemplate);
                        ControllerModule.Possess(playerController, gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the Object
        /// </summary>
        /// <param name="saveSettings">Save settings.</param>
        public void Save(SaveSettings saveSettings = 0)
        {
        }

        /// <summary>
        /// Gets the Value of a Property
        /// </summary>
        /// <returns>The property.</returns>
        /// <param name="name">Name.</param>
        public object GetProperty(string name)
        {
            if(properties.ContainsKey(name))
                return properties[name];
            return null;
        }

        /// <summary>
        /// Sets the Value of a Property
        /// </summary>
        /// <returns>The property.</returns>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public object SetProperty(string name, object value)
        {
            if (properties.ContainsKey(name))
                properties[name] = value;
            else
                properties.Add(name, value);
            return value;
        }

        /// <summary>
        /// Updates Property Values contained within the Selected Property
        /// </summary>
        /// <param name="from">From.</param>
        private void SetChildProperties(object from)
        {
            FieldInfo[] fields = from.GetType().GetFields();
            foreach(FieldInfo field in fields)
            {
                if (properties.ContainsKey(from.GetType().Name + "." + field.Name))
                    properties[from.GetType().Name + "." + field.Name] = field.GetValue(from);
                else
                    properties.Add(from.GetType().Name+"."+field.Name, field.GetValue(from));
            }
        }
    }
}
