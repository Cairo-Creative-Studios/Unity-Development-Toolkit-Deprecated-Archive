using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CairoEngine.EventSheet))]
public class EventSheetInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CairoEngine.EventSheet myScript = (CairoEngine.EventSheet)target;
        if (GUILayout.Button("Open in Event Sheet Editor"))
        {
            EventSheetEditor window = (EventSheetEditor)EditorWindow.GetWindow(typeof(EventSheetEditor), false, "Event Sheet Editor");
            window.Show();
        }
    }
}
