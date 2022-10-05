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
using NaughtyAttributes;

namespace UDT.UI
{
    /// <summary>
    /// Controls Display of User Interfaces, with many tools for simplifying display of Information. 
    /// </summary>
    public class UIModule : MonoBehaviour
    {
        //public PanelSettings defaultPanelSettings;
        //public VisualTreeAsset defaultUIDocument;
        public GameObject UIContainerPrefab;

        public static UIModule singleton;

        public UIDocument documentComponent;

        /// <summary>
        /// The data to use for the UI that should be Displayed.
        /// </summary>
        private static UITemplate currentTemplate;
        /// <summary>
        /// The HUD infos that have are present in the Game.
        /// </summary>
        private static List<UITemplate> uiTemplates = new List<UITemplate>();

        public static DropdownList<string> elementProperties = null;

        /// <summary>
        /// All the UI Binds in the Module, ordered by the Field Instances grouped with the Element that is to be modified
        /// </summary>
        //private static List<ElementBind> binds = new List<ElementBind>();

        /// <summary>
        /// The current UI root, is recreated when the UI is Reset
        /// </summary>
        public VisualElement root;

        /// <summary>
        /// All the current Overlays. Created when the UI is loaded, and deleted when reset.
        /// </summary>
        private static List<GameObject> overlays = new List<GameObject>();

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            GameObject singletonObject = new GameObject();
            singleton = singletonObject.AddComponent<UIModule>();
            GameObject uiContainer = Instantiate(singleton.UIContainerPrefab);
            singleton.root = uiContainer.GetComponent<UIDocument>().rootVisualElement;
            DontDestroyOnLoad(singletonObject);
            singleton.name = "UI Module";
        }

        public static DropdownList<string> ValidateElementProperties()
        {
            if (elementProperties == null)
            {
                VisualElement element = new VisualElement();

                DropdownList<string> properties = new DropdownList<string>();
                properties.Add("Text", "Text");

                PropertyInfo[] fieldInfos = typeof(IStyle).GetProperties();
                foreach (PropertyInfo field in fieldInfos)
                {
                    if (field.Name.TokenCount('_') == 1)
                        properties.Add(field.Name, field.Name);
                }

                elementProperties = properties;
            }
            return elementProperties;
        }

        /// <summary>
        /// Resets the User Interface. Usually you'll want to do this at the beginning of Each Runtime State, and then Create/Link new UI Values.
        /// </summary>
        public static void Clear()
        {
            //Destroy all the Camera Overlays
            foreach (GameObject overlay in overlays)
            {
                Destroy(overlay);
            }
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
            foreach (VisualTreeAsset visualElement in ui.UXMLFiles.Keys)
            {
                //Ge the Visual Element Tree from the current Visual Element Asset
                VisualElement tree = new VisualElement();
                visualElement.CloneTree(tree);

                //Add the Tree to the root with it's Style Sheet
                //root.Add(tree);
                //root.styleSheets.Add(ui.UXMLFiles[visualElement]);
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
            foreach (UITemplate ui in uiTemplates)
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
            if (currentTemplate != null)
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
                        //Bind(component, bind.fieldName, bind.element, (PropertyType)bind.property);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Renders the given UI Information from the UI Template, and returns an Object Array of all the elements that will have to be Discarded when disabling the UI
        /// </summary>
        /// <returns>The user interface.</returns>
        /// <param name="item">The UI Item to Render</param>
        public static VisualElement[] RenderUI(UIItem item)
        {
            List<VisualElement> elements = new List<VisualElement>();

            if (singleton == null || singleton.root == null)
            {
                return null;
            }

            VisualElement tree = new VisualElement();
            if (item.UIDocument != null)
            {
                //Ge the Visual Element Tree from the current Visual Element Asset
                item.UIDocument.CloneTree(tree);

                //Add the Tree to the root with it's Style Sheet
                singleton.root.Add(tree);
            }

            if (item.styleSheet != null)
                singleton.root.styleSheets.Add(item.styleSheet);

            if (item.UIDocument != null)
                elements.Add(tree);

            return elements.ToArray();
        }

        /// <summary>
        /// Destroy the given UI Items
        /// </summary>
        /// <param name="items">The UI Items to Destroy.</param>
        public static void DestroyUI(List<UIItem> items)
        {
            foreach (UIItem item in items)
            {
                item.Destroy();
            }
        }

        /// <summary>
        /// Instances of the Objects in the UI Module who's fields are being tracked
        /// </summary>
        //class ElementBind
        //{
        //    public object instance;
        //    public FieldInfo field;
        //    public VisualElement element;
        //    public PropertyType property;

        //    public ElementBind(object instance, string fieldName, VisualElement element, PropertyType property)
        //    {
        //        this.instance = instance;
        //        field = instance.GetType().GetField(fieldName);
        //        this.property = property;
        //        this.element = element; 
        //    }

        //    public object this[string fieldName]
        //    {
        //        get
        //        {
        //            return field.GetValue(instance);
        //        }
        //        set
        //        {
        //            field.SetValue(instance, value);
        //        }
        //    }
        //}

    }
}
