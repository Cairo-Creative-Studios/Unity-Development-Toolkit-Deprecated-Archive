//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

/**Base Class for all Spektor engine Objects**/
using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine.Behaviour;
using CairoEngine.Reflection;
using System.Reflection;
using ToolBox.Serialization;

namespace CairoEngine
{
    public class CObject : MonoBehaviour
    {
        /// <summary>
        /// The Controller of this Entity
        /// </summary>
        public Controller controller;
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
        public CObjectTemplate template;
        /// <summary>
        /// The Behaviours that have been added to the Object.
        /// </summary>
        [Tooltip("The Cairo Behaviours attached to the Object")]
        public List<object> behaviours = new List<object>();
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
        /// The ID of the Pool this Object belongs to
        /// </summary>
        [Tooltip("The ID of the Object Pool this belongs to")]
        public string poolID = "";
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
        /// <summary>
        /// The Moving Velocity of the Object
        /// </summary>
        [Tooltip("The Moving Velocity of the Object")]
        public Vector3 velocity = new Vector3();

        public enum SaveSettings
        {
            Everything,
            Transform,
            Behaviours
        }

        void Start()
        {
            Enable();
        }

        //Continuously Update the Values of Properties
        void Update()
        {
            if (active)
            {
                foreach (object property in properties.Values)
                {
                    SetChildProperties(property);
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
        /// Calls a method in the specified Behaviour that has been attached to the Cairo Object
        /// </summary>
        /// <returns>The behaviour method.</returns>
        /// <param name="methodName">Method name.</param>
        /// <param name="paramaters">Paramaters.</param>
        /// <typeparam name="BehaviourType">The 1st type parameter.</typeparam>
        public object CallBehaviourMethod<BehaviourType>(string methodName, object[] paramaters)
        {
            foreach(object behaviour in behaviours)
            {
                if(behaviour.GetType().Name == typeof(BehaviourType).Name)
                {
                    return behaviour.CallMethod(methodName, paramaters);
                }
            }

            return null;
        }

        /// <summary>
        /// Enables the behaviour.
        /// </summary>
        /// <param name="behaviour">Behaviour.</param>
        public void EnableBehaviour(string behaviour)
        {
            GetBehaviour(behaviour).Enable();
        }

        /// <summary>
        /// Disables the behaviour.
        /// </summary>
        public void DisableBehaviour(string behaviour)
        {
            GetBehaviour(behaviour).Disable();
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

            //SetChildProperties(value);
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

        /// <summary>
        /// Enable this instance.
        /// </summary>
        public void Enable()
        {
            active = false;

            foreach(object behaviourType in behaviours)
            {
                behaviourType.CallMethod("Enable");
            }

            ObjectModule.ActivateTags(this);
        }

        /// <summary>
        /// Disable this instance.
        /// </summary>
        public void Disable()
        {
            active = false;

            foreach (CairoBehaviour<object> behaviourType in behaviours)
            {
                behaviourType.Disable();
            }

            ObjectModule.RemoveTags(this);
        }

        void OnDestroy()
        {
            ObjectModule.RemoveTags(this);
            behaviours.Clear();
            BehaviourModule.RemoveBehaviourObject(gameObject);
        }

        /// <summary>
        /// Get a Behaviour attached to the Object
        /// </summary>
        /// <returns>The behaviour.</returns>
        /// <param name="ID">Identifier.</param>
        private CairoBehaviour<object> GetBehaviour(string ID)
        {
            foreach(CairoBehaviour<object> behaviourType in behaviours)
            {
                if(((BehaviourTemplate)(object)behaviourType.template).ID == ID)
                {
                    return behaviourType;
                }
            }

            return null;
        }
    }
}
