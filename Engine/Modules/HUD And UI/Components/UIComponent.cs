using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UDT.AssetScripting;

namespace UDT.UI
{
    public class UIComponent : MonoBehaviour
    {
        //The Template to use for Displaying the UI
        public UIContainer UI;
        VisualElement[] elements;

        void Update()
        {
            //Send the List of UI Items that need to be Rendered to the UI Module
            UI.Render();
        }

        void OnDisable()
        {
            UIModule.DestroyUI(UI.UIItems);
        }
    }
}
