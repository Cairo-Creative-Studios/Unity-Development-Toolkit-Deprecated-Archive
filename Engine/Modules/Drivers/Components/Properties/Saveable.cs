﻿using System.Collections.Generic;
using UnityEngine;
using CairoEngine.SaveSystem;
using CairoEngine.Reflection;
using System;

namespace CairoEngine.Drivers
{
	public class Saveable : Driver<DriverTemplate_Saveable>
	{
		/// <summary>
		/// Lists of Fields within Monobehaviours that are to be Saved and Loaded
		/// </summary>
		[Tooltip("Lists of Fields within Monobehaviours that are to be Saved and Loaded")]
		public SDictionary<string, List<string>> fields = new SDictionary<string, List<string>>();
		/// <summary>
		/// If true, the Component will automatically handle Saving and Load Game Object information
		/// </summary>
		[Tooltip("If true, the Component will automatically handle Saving and Load Game Object information")]
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
			foreach(string behaviour in fields.Keys)
			{
				var component = gameObject.GetComponent(Type.GetType(behaviour));
				foreach(string field in fields[behaviour])
				{
					SaveSystemModule.SetProperty(gameObject.name+"_"+gameObject.GetInstanceID()+"_"+field, component.GetField(field));
				}
			}
		}

		/// <summary>
		/// Loads all the Fields that are to be Tracked by the Save System
		/// </summary>
		public void Load()
		{
			autoLoaded = true;
			foreach (string behaviour in fields.Keys)
			{
				var component = gameObject.GetComponent(Type.GetType(behaviour));
				foreach (string field in fields[behaviour])
				{
					component.SetField(field, SaveSystemModule.GetProperty(gameObject.name + "_" + gameObject.GetInstanceID() + "_" + field));
				}
			}
		}
	}
}