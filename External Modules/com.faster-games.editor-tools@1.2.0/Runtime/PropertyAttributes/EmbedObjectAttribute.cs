using UnityEngine;

namespace FasterGames.EditorTools.PropertyAttributes
{
    /// <summary>
    /// Attribute to automatically embed an object in the editor ui.
    /// </summary>
    /// <remarks>
    /// This facilitates nested object editing from one inspector panel
    /// </remarks>
    public class EmbedObjectAttribute : PropertyAttribute
    {
        // TODO(bengreenier): For now, this does not work on MonoBehaviours, only ScriptableObjects
        // this is due to some UI control bug that I can't figure out
    }
}