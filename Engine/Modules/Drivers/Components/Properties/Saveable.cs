using System.Collections.Generic;
using UnityEngine;
using UDT.SaveSystem;
using UDT.Reflection;
using System;
using Homebrew;

namespace UDT.Drivers
{
    public class Saveable : Driver<DriverTemplate_Saveable>
    {
        /// <summary>
        /// Lists of Fields within Monodrivers that are to be Saved and Loaded
        /// </summary>
        [Tooltip("Lists of Fields within Monodrivers that are to be Saved and Loaded")]
        [Foldout("Properties")]
        public SDictionary<string, List<string>> fields = new SDictionary<string, List<string>>();
        /// <summary>
        /// If true, the Component will automatically handle Saving and Load Game Object information
        /// </summary>
        [Tooltip("If true, the Component will automatically handle Saving and Load Game Object information")]
        [Foldout("Properties")]
        public bool automatic = true;

        bool autoLoaded = false;

        void Start()
        {
            fields = template.fields;

            if (automatic)
            {
                SaveSystemModule.OnLoad += OnLoad;
                OnLoad();
            }
        }

        void OnLoad()
        {
            Load();
        }

        void Update()
        {
            if (automatic && SaveSystemModule.ready)
            {
                if (autoLoaded)
                    Save();
                else
                    Load();
            }
        }

        /// <summary>
        /// Saves all the Fields that are to be Tracked by the Save System
        /// </summary>
        public void Save()
        {
            foreach (string driver in fields.Keys)
            {
                var component = gameObject.GetComponent(Type.GetType(driver));
                foreach (string field in fields[driver])
                {
                    SaveSystemModule.SetProperty(gameObject.name + "_" + gameObject.GetInstanceID() + "_" + field, component.GetField(field));
                }
            }
        }

        /// <summary>
        /// Loads all the Fields that are to be Tracked by the Save System
        /// </summary>
        public void Load()
        {
            autoLoaded = true;
            foreach (string driver in fields.Keys)
            {
                var component = gameObject.GetComponent(Type.GetType(driver));
                foreach (string field in fields[driver])
                {
                    component.SetField(field, SaveSystemModule.GetProperty(gameObject.name + "_" + gameObject.GetInstanceID() + "_" + field));
                }
            }
        }
    }
}
