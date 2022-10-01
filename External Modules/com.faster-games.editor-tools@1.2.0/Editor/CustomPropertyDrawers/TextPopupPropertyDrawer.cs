using UnityEditor;
using UnityEngine;

namespace FasterGames.EditorTools.Editor.CustomPropertyDrawers
{
    public abstract class TextPopupPropertyDrawer : PropertyDrawer
    {
        private readonly int _controlIdHash = $"{nameof(TextPopupPropertyDrawer)}".GetHashCode();
        private bool _shouldRefreshNext = false;
        
        protected const int NoItemIndex = -1;
        
        protected abstract string RefreshButtonText { get; }
        protected abstract string[] PopupText { get; }
        protected abstract int CurrentIndex { get; set; }

        protected virtual void OnSelect(SerializedProperty property, string[] text, int oldIndex, int newIndex)
        {
            
        }

        protected virtual void RefreshContent(SerializedProperty property)
        {
            
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (PopupText == null || PopupText.Length == 0 || _shouldRefreshNext)
            {
                RefreshContent(property);
                _shouldRefreshNext = false;
            }


            var popupStyle = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = position.height,
                fixedWidth = 24,
                imagePosition = ImagePosition.ImageOnly,
            };
            
            // By manually creating the control ID, we can keep the ID for the
            // label and button the same. This lets them be selected together
            // with the keyboard in the inspector, much like a normal popup.
            var controlId = GUIUtility.GetControlID(_controlIdHash, FocusType.Keyboard, position);
            
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, controlId, label);

            var dropdownRect = new Rect(position.x - (popupStyle.fixedWidth + popupStyle.margin.right), position.y, popupStyle.fixedWidth + popupStyle.margin.right, position.height);
            var valueRect = new Rect(position.x, position.y, position.width, position.height);
            
            var currentPopupText = PopupText;

            GUIContent buttonText;
            if (CurrentIndex == NoItemIndex || CurrentIndex >= currentPopupText.Length)
            {
                buttonText = new GUIContent();
            }
            else
            {
                buttonText = new GUIContent(currentPopupText[CurrentIndex]);
            }

            var refreshContent = new GUIContent(EditorGUIUtility.IconContent("Refresh"))
            {
                tooltip = RefreshButtonText
            };
            
            if (GUI.Button(dropdownRect, refreshContent, popupStyle))
            {
                _shouldRefreshNext = true;
            }
            
            if (DropdownButton(controlId, valueRect, buttonText))
            {
                SearchablePopup.Show(position, currentPopupText, CurrentIndex, (int index) =>
                {
                    var oldIndex = CurrentIndex;
                    CurrentIndex = index;
                    
                    OnSelect(property, currentPopupText, oldIndex, CurrentIndex);

                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            
            EditorGUI.EndProperty();
        }
        
        /// <summary>
        /// A custom button drawer that allows for a controlID so that we can
        /// sync the button ID and the label ID to allow for keyboard
        /// navigation like the built-in enum drawers.
        /// </summary>
        private static bool DropdownButton(int id, Rect position, GUIContent content)
        {
            Event current = Event.current;
            switch (current.type)
            {
                case EventType.MouseDown:
                    if (position.Contains(current.mousePosition) && current.button == 0)
                    {
                        Event.current.Use();
                        return true;
                    }
                    break;
                case EventType.KeyDown:
                    if (GUIUtility.keyboardControl == id && current.character =='\n')
                    {
                        Event.current.Use();
                        return true;
                    }
                    break;
                case EventType.Repaint:
                    EditorStyles.popup.Draw(position, content, id, false);
                    break;
            }
            return false;
        }
    }
}