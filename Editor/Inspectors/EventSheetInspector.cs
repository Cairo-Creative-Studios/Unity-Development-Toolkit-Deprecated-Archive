using UnityEngine;
using UDT.Scripting;
using UnityEditor;

[CustomEditor(typeof(EventSheet))]
public class EventSheetInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EventSheet myScript = (EventSheet)target;
        if (GUILayout.Button("Open in Event Sheet Editor"))
        {
            EventSheetEditor window = (EventSheetEditor)EditorWindow.GetWindow(typeof(EventSheetEditor), false, "Event Sheet Editor");
            window.Show();
        }
    }
}
