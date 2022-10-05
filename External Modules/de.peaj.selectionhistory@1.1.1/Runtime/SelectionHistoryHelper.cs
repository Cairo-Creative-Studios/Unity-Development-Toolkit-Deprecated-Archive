using UnityEngine;

namespace Unitility.SelectionHistory
{
    /// <summary>
    /// Helper class for temporarily storing selection history in a GameObject to recover from assebly reload
    /// </summary>
    public class SelectionHistoryHelper : MonoBehaviour
    {
        private const string Name = "[de.peaj.selectionhistory.helper]";

        public static void SaveSelection(HistoryBuffer<SelectionSnapshot> history)
        {
            var instance = GetOrCreate();
            instance.history = history.ToArray();
            instance.current = history.GetCurrentArrayIndex();
        }
        
        public static HistoryBuffer<SelectionSnapshot> LoadSelection()
        {
            var instance = GetOrCreate();
            return HistoryBuffer<SelectionSnapshot>.FromArray(instance.history, instance.current, 50);
        }
        
        public static SelectionHistoryHelper GetOrCreate()
        {
            var go = GameObject.Find(Name)??new GameObject(Name) {hideFlags = HideFlags.HideAndDontSave};
            var helper = go.GetComponent<SelectionHistoryHelper>()??go.AddComponent<SelectionHistoryHelper>();

            return helper;
        }
        
        [SerializeField] private SelectionSnapshot[] history;
        [SerializeField] private int current;
    }
}
