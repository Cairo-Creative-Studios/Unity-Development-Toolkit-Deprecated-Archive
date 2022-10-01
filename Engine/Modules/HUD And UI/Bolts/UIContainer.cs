using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CairoEngine.Scripting;
using CairoEngine.Reflection;

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
        /// Updates the Properties of the Element Binds
        /// </summary>
        void UpdateAssetBinds()
        {
            if (assetVariableBinds.Count != boundElements.Count)
            {
                QueryAssetBinds();
                return;
            }

            foreach(UIBind bind in boundElements)
            {
                switch (bind.property)
                {
                    case UIBind.Property.Text:
                        ((TextElement)bind.element).text = ((AssetString)bind.variable)?.variable?.value;
                        break;
                    case UIBind.Property.Color:
                        ((object)bind.element.style).SetField("backgroundColor", (Color)((object)bind.variable)?.GetField("variable")?.GetField("value"));
                        break;
                    case UIBind.Property.Width:
                        ((object)bind.element.style).SetField("width", (float)((object)bind.variable)?.GetField("variable")?.GetField("value"));
                        break;
                    case UIBind.Property.Height:
                        ((object)bind.element.style).SetField("height", (float)((object)bind.variable)?.GetField("variable")?.GetField("value"));
                        break;
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
        public enum Property
        {
            Text,
            Width,
            Height,
            Color
        }

        public string elementID = "";
        public Property property;
        public AssetVariableBase variable;
        public VisualElement element;
    }
}
