using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CairoEngine.Reflection;
using NaughtyAttributes;
using System.Reflection;
using CairoEngine.AssetScripting;

namespace CairoEngine.UI
{
    [Serializable]
    public class UIContainer
    {
        /// <summary>
        /// The UXML files to use for rendering the UI.
        /// </summary>
        [Tooltip("The UXML Files to render with the UI")]
        public List<UIItem> UIItems = new List<UIItem>();

        /// <summary>
        /// Gets a List of UI Items that need to be Rendered so the UIModule can render them
        /// </summary>
        /// <returns>The render.</returns>
        public List<UIItem> Render()
        {
            List<UIItem> items = new List<UIItem>();

            foreach(UIItem item in UIItems)
            {
                UIItem returnedItem = item.Render();
                if (returnedItem != null)
                {
                    items.Add(returnedItem);
                }
            }

            return items;
        }
    }

    [Serializable]
    public class UIItem
    {
        [Header(" - Rendering - ")]
        [Tooltip("Whether Rendering of the UI Item is enabled")]
        public bool enabled = true;
        [Tooltip("When set, uses an Asset Bool Variable to determine whether the UI Item should be enabled")]
        public AssetBool enabledAsset;
        [Tooltip("The UI Document to use when Rendering the UI Item")]
        public VisualTreeAsset UIDocument;
        [Tooltip("Style Sheet to add to the UI Document")]
        public StyleSheet styleSheet;
        [Header(" - Binds - ")]
        [Tooltip("Asset Variable Binds for this UI Item")]
        public List<UIBind> assetVariableBinds;
        [Tooltip("Whether the Item has been Rendered or not")]
        [HideInInspector] public bool rendered = false;
        [Tooltip("The Element that was created when this UIItem was Rendered")]
        [HideInInspector] public VisualElement renderedElement;
        [Tooltip("Elements that have been bound")]
        List<UIBind> boundElements = new List<UIBind>();

        /// <summary>
        /// Render the UI Item, returning a Visual Element from the UI Document if the Item is Enabled, and hasn't yet been rendered.
        /// </summary>
        /// <returns>The Cloned Tree of the Visual Element, if it has yet to be Rendered and is Enabled</returns>
        public UIItem Render()
        {
            if (enabledAsset == null)
            {
                if (enabled && !rendered)
                {
                    renderedElement = UIDocument.CloneTree();
                    UIModule.singleton.root.Add(renderedElement);
                    rendered = true;
                    return this;
                }
            }
            else if(enabledAsset.variable.value && !rendered)
            {
                renderedElement = UIDocument.CloneTree();
                UIModule.singleton.root.Add(renderedElement);
                rendered = true;
                return this;
            }
            if (renderedElement != null) 
                UpdateAssetBinds();


            return null;
        }

        void QueryAssetBinds()
        {
            foreach(UIBind bind in assetVariableBinds)
            {
                bind.element = renderedElement.Q(bind.elementID);
                boundElements.Add(bind);
            }
        }

        /// <summary>
        /// Updates the Properties of the Element Binds intelligently, automatically handling adapting Asset Variables to the Properties they've been Bound to.
        /// </summary>
        void UpdateAssetBinds()
        {
            if (assetVariableBinds.Count != boundElements.Count)
            {
                QueryAssetBinds();
                return;
            }

            foreach (UIBind bind in boundElements)
            {
                //If the Property the Asset Variable is Bound to is the Text of the Visual Element, get the Value ToString
                if (bind.property == "Text")
                {
                    ((TextElement)bind.element).text = bind.variable?.GetField("variable")?.GetField("value")?.ToString();
                }
                //Otherwise, determine how to evaluate the Value of the Asset Variable based on it's Type and what Property it's been Bound to
                else
                {
                    switch (bind.variable.GetType().Name)
                    {
                        case "AssetNumber":
                            ((object)bind.element.style).SetProperty("UnityEngine.UIElements.IStyle." + bind.property, new StyleFloat((float)((object)bind.variable)?.GetField("variable")?.GetField("value")));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Destroys the Visual Elements and resets Rendered Status.
        /// </summary>
        public void Destroy()
        {
            if(renderedElement != null)
            {
                renderedElement.RemoveFromHierarchy();
                renderedElement = null;
            }
            rendered = false;
        }
    }

    [Serializable]
    public class UIBind
    {

        public string elementID = "";
        [Dropdown("GetStyleProperties")]
        public string property;
        public AssetVariableBase variable;
        public VisualElement element;

        DropdownList<string> GetStyleProperties()
        {
            VisualElement element = new VisualElement();

            DropdownList<string> properties = new DropdownList<string>();
            properties.Add("Text", "Text");

            PropertyInfo[] fieldInfos = typeof(IStyle).GetProperties();
            foreach (PropertyInfo field in fieldInfos)
            {
                if(field.Name.TokenCount('_') == 1)
                    properties.Add(field.Name, field.Name);
            }

            return properties;
        }
    }
}
