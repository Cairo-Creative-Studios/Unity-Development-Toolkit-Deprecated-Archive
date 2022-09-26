/*! \addtogroup uimodule UI Module
 *  Additional documentation for group 'UI Module'
 *  @{
 */

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using System.Reflection;

namespace CairoEngine.UI
{
    /// <summary>
    /// Controls Display of User Interfaces, with many tools for simplifying display of Information. 
    /// </summary>
    public class UIModule
    {
        /// <summary>
        /// The data to use for the UI that should be Displayed.
        /// </summary>
        private static UITemplate currentTemplate;
        /// <summary>
        /// The HUD infos that have are present in the Game.
        /// </summary>
        private static List<UITemplate> uiTemplates = new List<UITemplate>();

        /// <summary>
        /// All the UI Binds in the Module, ordered by the Field Instances grouped with the Element that is to be modified
        /// </summary>
        private static List<ElementBind> binds = new List<ElementBind>();

        /// <summary>
        /// The current UI root, is recreated when the UI is Reset
        /// </summary>
        private static VisualElement root;

        /// <summary>
        /// All the current Overlays. Created when the UI is loaded, and deleted when reset.
        /// </summary>
        private static List<GameObject> overlays = new List<GameObject>();


        public enum PropertyType
        {
            Text,
            Width,
            Height,
            Color
        }

        public static void Init()
        {
            uiTemplates.AddRange(Resources.LoadAll<UITemplate>(""));
        }

        public static void Update()
        {
            //Update Element properties for Binds
            foreach (ElementBind bind in binds)
            {
                //Choose what to do depending on the Property that is intended to be modified by the Bind
                switch (bind.property)
                {
                    case PropertyType.Width:
                        bind.element.transform.scale = new Vector3(bind.element.transform.scale.x,(float)bind.field.GetValue(bind.instance),bind.element.transform.scale.z);
                        break;
                    case PropertyType.Height:
                        bind.element.transform.scale = new Vector3((float)bind.field.GetValue(bind.instance), bind.element.transform.scale.y, bind.element.transform.scale.z);
                        break;
                    case PropertyType.Text:
                        TextElement asText = (TextElement)bind.element;
                        asText.text = (string)bind.field.GetValue(bind.instance);
                        break;
                }
            }
        }

        /// <summary>
        /// Resets the User Interface. Usually you'll want to do this at the beginning of Each Runtime State, and then Create/Link new UI Values.
        /// </summary>
        public static void Clear()
        {
            //Destroy all the Camera Overlays
            foreach(GameObject overlay in overlays)
            {
                GameObject.Destroy(overlay);
            }
            //Clear the UXML Root
            root = new VisualElement();

            //Empty the current Binds list
            binds = new List<ElementBind>();
        }

        /// <summary>
        /// Binds a class's field to an Element's property
        /// </summary>
        /// <param name="boundClass">Bound class.</param>
        /// <param name="field">The name of the Field to Bind</param>
        /// <param name="elementName">The Element to Bind to</param>
        /// <param name="property">The Element's Property</param>
        public static void Bind(object boundClass, string field, string elementName, PropertyType property)
        {
            //Get the Element from the current Visual Element Tree
            VisualElement element = root.Q<VisualElement>(elementName);

            //Add the bind to the bind List
            binds.Add(new ElementBind(boundClass, field, element, property));
        }

        /// <summary>
        /// Sets the user interface to the Specified UI Object
        /// </summary>
        /// <param name="ID">Identifier.</param>
        public static void Set(string ID)
        {
            //Clear the current UI
            Clear();

            //Get the HUD Template from Resources
            UITemplate ui = GetUI(ID);
            if (ui != null)
                currentTemplate = ui;
            else
                Debug.LogWarning("Failed to set UI to nonexistent " + ID);

            //Render the UI Documents add to the UI 
            foreach(VisualTreeAsset visualElement in ui.UXMLFiles.Keys)
            {
                //Ge the Visual Element Tree from the current Visual Element Asset
                VisualElement tree = new VisualElement();
                visualElement.CloneTree(tree);

                //Add the Tree to the root with it's Style Sheet
                root.Add(tree);
                root.styleSheets.Add(ui.UXMLFiles[visualElement]);
            }

            //Render the Camera Overlays added to the UI
            foreach (CameraOverlay display in ui.cameraOverlays)
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
        public static UITemplate GetUI(string ID)
        {
            foreach(UITemplate ui in uiTemplates)
            {
                if (ui.ID == ID)
                    return ui;
            }
            return null;
        }

        /// <summary>
        /// Check the Game Object into the UI Module to add it to 
        /// </summary>
        /// <param name="checkedObject">Checked object.</param>
        public static void CheckIn(GameObject checkedObject, GameObject prefab)
        {
            if(currentTemplate != null)
            {
                //Loop through all the Game Object Binds in the Template
                foreach (GameObjectBind bind in currentTemplate.gameObjectBinds)
                {
                    //If the Game Object Bind is the same as the one we just checked in, add the appropriate Binds for the Elements in the UI
                    if (bind.boundObject == prefab)
                    {
                        //Get the Component that the Game Object Bind is for
                        var component = checkedObject.GetComponent(bind.boundBehaviour);
                        //Bind the Element to the Game Object's Field
                        Bind(component, bind.fieldName, bind.element, (PropertyType)bind.property);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Instances of the Objects in the UI Module who's fields are being tracked
        /// </summary>
        class ElementBind
        {
            public object instance;
            public FieldInfo field;
            public VisualElement element;
            public PropertyType property;

            public ElementBind(object instance, string fieldName, VisualElement element, PropertyType property)
            {
                this.instance = instance;
                field = instance.GetType().GetField(fieldName);
                this.property = property;
                this.element = element; 
            }

            public object this[string fieldName]
            {
                get
                {
                    return field.GetValue(instance);
                }
                set
                {
                    field.SetValue(instance, value);
                }
            }
        }

    }
}
