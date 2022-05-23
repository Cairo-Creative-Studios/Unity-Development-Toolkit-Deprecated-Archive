using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

//TODO: Implement the UI Manager.
namespace CairoEngine
{
    public class UIManager
    {
        /// <summary>
        /// The data to use for the UI that should be Displayed.
        /// </summary>
        private static HUDInfo info;
        /// <summary>
        /// The HUD infos that have are present in the Game.
        /// </summary>
        private static List<HUDInfo> hudInfos = new List<HUDInfo>();
        /// <summary>
        /// The Values given to the UI. Values should be passed through the SetValue() method.
        /// </summary>
        private static Dictionary<string, object> UIValues = new Dictionary<string, object>();
        /// <summary>
        /// A List of Text to Value Links. When the UIManager is Updated, the text displayed in these TextContainers is also Updated to the values assigned to them.
        /// </summary>
        private static Dictionary<string,TextContainer> valueLinks = new Dictionary<string,TextContainer>();

        public static void Init()
        {
            hudInfos.AddRange(Resources.LoadAll<HUDInfo>(""));
        }

        public static void Update()
        {

        }

        /// <summary>
        /// Resets the User Interface. Usually you'll want to do this at the beginning of Each Runtime State, and then Create/Link new UI Values.
        /// </summary>
        public static void ResetUI()
        {

        }

        /// <summary>
        /// Creates a link between a Value and a UI Object. The Type of the Object passed determines how the UI Displays the Value.
        /// To customize how the Value is displayed, you must pass options when invoking the Method.
        /// </summary>
        /// <param name="UIObject">UIO bject.</param>
        /// <param name="valueName">Value name.</param>
        public static void CreateValueLink(object UIObject, string valueName, object[] options = null)
        {

        }

        /// <summary>
        /// Sets a UI Value, so that it can be used by UI Objects
        /// </summary>
        public static void SetValue()
        {

        }

        /// <summary>
        /// Sets the user interface to the Specified UI Object
        /// </summary>
        /// <param name="ID">Identifier.</param>
        public static void SetUI(string ID)
        {
            HUDInfo hud = GetHUD(ID);
            if (hud != null)
                info = hud;
            else
                Debug.LogWarning("Failed to set HUD to nonexistent " + ID);

            foreach(UIDisplay display in hud.UIDisplays)
            {
                GameObject displayObject = UnityEngine.Object.Instantiate(display.rootObject);
                Camera displayCamera = displayObject.transform.Find("Camera").GetComponent<Camera>();
                Camera.main.GetComponent<UniversalAdditionalCameraData>().cameraStack.Add(displayCamera);
            }
        }

        /// <summary>
        /// Finds and returns the requested HUD
        /// </summary>
        /// <returns>The hud.</returns>
        /// <param name="ID">The HUD's ID</param>
        public static HUDInfo GetHUD(string ID)
        {
            foreach(HUDInfo hud in hudInfos)
            {
                if (hud.ID == ID)
                    return hud;
            }
            return null;
        }
    }
}
