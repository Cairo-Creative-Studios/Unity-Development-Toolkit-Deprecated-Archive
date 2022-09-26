using System;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;

namespace CairoEngine.SaveSystem
{
	public class SaveSystemModule : MonoBehaviour
	{
		public static SaveSystemModule singleton;
		[Tooltip("If set, a profile for this Template will be created when the game Starts")]
		public ProfileTemplate defualtTemplate;
		[Tooltip("The Profile that is currently loaded into the Game")]
		public SaveProfile currentProfile = new SaveProfile(null);
		[Tooltip("If True, the System will allow new Profiles to be Created ID's that already exist, otherwise the System will automatically add a numerically ordered suffix to seperate the Profiles.")]
		public bool allowOverwriting = false;
		[Tooltip("Whether to delete the Current Save or not, will set to False after deletion")]
		public bool deleteSave = false;
		/// <summary>
		/// The Binary Serializer for Saving the Game
		/// </summary>
		SaveGameBinarySerializer serializer = new SaveGameBinarySerializer();

		public static bool ready = false;

		int tick = 0;

		public delegate void LoadAction();
		public static event LoadAction OnLoad;

		[RuntimeInitializeOnLoadMethod]
		public static void Init()
		{
			GameObject singletonObject = new GameObject();
			singleton = singletonObject.AddComponent<SaveSystemModule>();
			singletonObject.name = "Cairo Save System Module";
			GameObject.DontDestroyOnLoad(singletonObject);
			LoadProfile("Default");
			singleton.currentProfile.ID = "Default";
		}

		void Start()
		{

			//If the Default Template is set, create a Save Profile from it.
			if (singleton.defualtTemplate != null)
				currentProfile = new SaveProfile(singleton.defualtTemplate);

			ready = true;
		}

		void Update()
		{
			if (deleteSave)
			{
				currentProfile = new SaveProfile(null);
				deleteSave = false;
			}
			tick++;
		}

		/// <summary>
		/// Sets a Property in the current Save Profile
		/// </summary>
		/// <returns>The property.</returns>
		/// <param name="propertyName">Property name.</param>
		/// <param name="value">Value.</param>
		public static object SetProperty(string propertyName, object value)
		{
			singleton.currentProfile.SetProperty(propertyName, value);
			SaveProfile();
			return value;
		}

		/// <summary>
		/// Returns the Property with the given Name, from the current Save Profile
		/// </summary>
		/// <returns>The property.</returns>
		/// <param name="propertyName">Property name.</param>
		public static object GetProperty(string propertyName)
		{
			return singleton.currentProfile.GetProperty(propertyName);
		}

		/// <summary>
		/// Creates a profile from the Given Template
		/// </summary>
		public static SaveProfile CreateProfile(string ID)
		{
			singleton.currentProfile = new SaveProfile(null);
			if (GetProfileExists(ID))
			{
				int i = 0;
				while (GetProfileExists(ID + "_" + i))
				{
					i++;
				}
				singleton.currentProfile.ID = ID + "_" + i;
			}
			else
				singleton.currentProfile.ID = ID;

			return singleton.currentProfile;
		}

		/// <summary>
		/// Creates a profile from the Given Template
		/// </summary>
		public static SaveProfile CreateProfile(ProfileTemplate template, string ID = "Default")
		{
			singleton.currentProfile = new SaveProfile(template);
			if (GetProfileExists(template.ID, ID))
			{
				int i = 0;
				while (GetProfileExists(template.ID, ID + "_" + i))
				{
					i++;
				}
				singleton.currentProfile.ID = ID + "_" + i;
			}
			else
				singleton.currentProfile.ID = ID;

			return singleton.currentProfile;
		}

		/// <summary>
		/// Returns True if the Given Profile Exists
		/// </summary>
		/// <returns><c>true</c>, if profile exists was gotten, <c>false</c> otherwise.</returns>
		/// <param name="profileID">Profile identifier.</param>
		public static bool GetProfileExists(string profileID)
		{
			return SaveGame.Load<SaveProfile>("Profile_" + profileID) != null;
		}

		/// <summary>
		/// Returns True if the Given Profile Exists
		/// </summary>
		/// <returns><c>true</c>, if profile exists was gotten, <c>false</c> otherwise.</returns>
		/// <param name="templateID">Template identifier.</param>
		/// <param name="profileID">Profile identifier.</param>
		public static bool GetProfileExists(string templateID, string profileID)
		{
			return SaveGame.Load<SaveProfile>("ProfileTemplate_" + templateID + "_Profile_" + profileID) != null;
		}

		/// <summary>
		/// Saves the Current Profile
		/// </summary>
		public static void SaveProfile()
		{
			string savePath;
			if(singleton.currentProfile.template != null)
			{
				//If the Profile was Created with a Template, Add the Profile to the Template's List in Save Data
				List<SaveProfile> profileList = new List<SaveProfile>();
				profileList.AddRange(SaveGame.Load<SaveProfile[]>("TemplateSaveList_" + singleton.currentProfile.template.ID));
				profileList.Add(singleton.currentProfile);
				SaveGame.Save("TemplateSaveList_" + singleton.currentProfile.template.ID, profileList.ToArray(), singleton.serializer);

				//Then Save the Profile
				savePath = "ProfileTemplate_" + singleton.currentProfile.template.ID + "_Profile_" + singleton.currentProfile.ID;
			}
			else
			{
				//Save the Profile
				savePath = "Profile_" + singleton.currentProfile.ID;
				SaveGame.Save(savePath, singleton.currentProfile, singleton.serializer);
			}
		}

		/// <summary>
		/// Attempts to Load a profile with the given ID
		/// </summary>
		/// <param name="ID">Identifier.</param>
		public static SaveProfile LoadProfile(string ID)
		{
			SaveProfile profile = SaveGame.Load<SaveProfile>("Profile_" + ID);
			if (profile != null)
			{
				singleton.currentProfile = profile;
				if (OnLoad != null)
					OnLoad();
				return profile;
			}
			return null;
		}

		/// <summary>
		/// Attempts to Load a profile with the given ID and Template ID
		/// </summary>
		/// <returns>The profile.</returns>
		/// <param name="profileID">Profile identifier.</param>
		/// <param name="templateID">Template identifier.</param>
		public static SaveProfile LoadProfile(string profileID, string templateID)
		{
			SaveProfile profile = SaveGame.Load<SaveProfile>("ProfileTemplate_"+templateID+"_Profile_" + profileID);
			if (profile != null)
			{
				singleton.currentProfile = profile;
				return profile;
			}
			return null;
		}

		/// <summary>
		/// Loads the Profile's Last Autosave
		/// </summary>
		/// <param name="ID">Identifier.</param>
		public static SaveProfile LoadAutosave(string ID)
		{
			SaveProfile profile = SaveGame.Load<SaveProfile>("ProfileAutosave_" + ID);
			if (profile != null)
			{
				singleton.currentProfile = profile;
				return profile;
			}
			return null;
		}

		/// <summary>
		/// Attempts to Load an Autosave with the given ID and Template ID
		/// </summary>
		/// <returns>The profile.</returns>
		/// <param name="profileID">Profile identifier.</param>
		/// <param name="templateID">Template identifier.</param>
		public static SaveProfile LoadAutosave(string profileID, string templateID)
		{
			SaveProfile profile = SaveGame.Load<SaveProfile>("ProfileTemplate_" + templateID + "_ProfileAutosave_" + profileID);
			if (profile != null)
			{
				singleton.currentProfile = profile;
				return profile;
			}
			return null;
		}

		/// <summary>
		/// Gets the profile.
		/// </summary>
		/// <returns>The profile.</returns>
		/// <param name="ID">Identifier.</param>
		public static SaveProfile GetProfile(string ID)
		{
			return SaveGame.Load<SaveProfile>("Profile_" + ID);
		}

		/// <summary>
		/// Gets the profile.
		/// </summary>
		/// <returns>The profile.</returns>
		/// <param name="ID">Identifier.</param>
		public static SaveProfile GetProfile(string profileID, string templateID)
		{
			return SaveGame.Load<SaveProfile>("ProfileTemplate_" + templateID + "_Profile_" + profileID);
		}


		/// <summary>
		/// Returns an Array of Save Profiles that utilize the Template with the given ID
		/// </summary>
		/// <returns>The profiles from template.</returns>
		/// <param name="ID">Identifier.</param>
		public static SaveProfile[] GetProfilesFromTemplate(string templateID)
		{
			return SaveGame.Load<SaveProfile[]>("TemplateSaveList_" + templateID);
		}

		/// <summary>
		/// Returns an Array of Save Profiles that utilize the given Template
		/// </summary>
		/// <returns>The profiles from template.</returns>
		/// <param name="template">Template.</param>
		public static SaveProfile[] GetProfilesFromTemplate(ProfileTemplate template)
		{
			return SaveGame.Load<SaveProfile[]>("TemplateSaveList_" + template.ID);
		}
	}

	[Serializable]
	public class SaveProfile
	{
		/// <summary>
		/// The Path in the Save System of the Profile
		/// </summary>
		[Tooltip("The Path in the Save System of the Profile")]
		public string SavePath = "";
		/// <summary>
		/// The Name of the Profile
		/// </summary>
		[Tooltip("The Name of the Profile")]
		public string ID = "";
		/// <summary>
		/// The template to use for Profile Information
		/// </summary>
		[Tooltip("The template to use for Profile Information")]
		[NonSerialized]
		public ProfileTemplate template;
		public Dictionary<string, bool> boolProperties = new Dictionary<string, bool>();
		public SDictionary<string, string> displayedProperties = new SDictionary<string, string>();
		public SDictionary<string, object> properties = new SDictionary<string, object>();


		public void Display()
		{
			displayedProperties.Clear();
			foreach (string item in boolProperties.Keys)
			{
				displayedProperties.Add(item, boolProperties[item].ToString());
			}
		}

		public SaveProfile(ProfileTemplate template)
		{
			this.template = template;
		}

		/// <summary>
		/// Set the Property with the given Name
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		public void SetProperty(string name, object value)
		{
			//if (boolProperties == null)
				//boolProperties = new SDictionary<string, bool>();

			if (properties.ContainsKey(name))
				properties[name] = value;
			else
			{
				properties.Add(name,value);
			}

		}

		/// <summary>
		/// Gets the Property with the given Name
		/// </summary>
		/// <returns>The property.</returns>
		/// <param name="name">Name.</param>
		public object GetProperty(string name)
		{
			if (properties.ContainsKey(name))
				return properties[name];
			return null;
		}

		public SaveProfile()
		{
			Display();
		}
	}
}
