using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using CairoEngine;

[CustomEditor(typeof(CairoEngine.EventSheet), true)]
public class EventSheetEditor : EditorWindow
{
    class VisualNode
    {
        ElementType type;
        VisualElement element;
    }

    private enum ElementType
    {
        Event,
        Condition,
        Action,
        Variable,
        Comment,
        Function,
        Trigger
    }

    //The Current Event Sheet to modify
    private EventSheet currentTemplate;

    //Visual Tree Elements
    //Base Event Sheet
    [Header(" - Base Event Sheet - ")]
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default(VisualTreeAsset);
    //Event Sheet Elements
    [Header(" - Event Sheet Elements - ")]
    [SerializeField]
    private VisualTreeAsset blockTemplate = default(VisualTreeAsset);
    [SerializeField]
    private VisualTreeAsset commentTemplate = default(VisualTreeAsset);
    [SerializeField]
    private VisualTreeAsset eventTemplate = default(VisualTreeAsset);

    Event e;
    Vector2 mousePosition;

    [MenuItem("Window/UI Toolkit/EventSheetEditor")]
    public static void ShowExample()
    {
        EventSheetEditor wnd = GetWindow<EventSheetEditor>();
        wnd.titleContent = new GUIContent("EventSheetEditor");
    }

    public void CreateGUI()
    {
        EventSheet[] selectedTemplates = Selection.GetFiltered<EventSheet>(SelectionMode.Assets);
        if (selectedTemplates.Length > 0)
        {
            currentTemplate = selectedTemplates[0];
        }

        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }

    void OnGUI()
    {   
        e = Event.current;
        mousePosition = e.mousePosition;

        if (e.button == 1)
        {
            //Context Menu
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Test"), false, () =>
            {
                Debug.Log("Test");
            });
            menu.ShowAsContext();
        }
    }

    /// <summary>
    /// Creates an Element of the given Type
    /// </summary>
    /// <returns>The element.</returns>
    /// <param name="type">Type.</param>
    void CreateElement(ElementType type, VisualElement parent)
    {
        VisualElement createdElement;
        
        //Run Initialization Code for each created Element
        switch (type)
        {
            case ElementType.Event:
                createdElement = eventTemplate.Instantiate();
                createdElement.RegisterCallback<MouseDownEvent, object[]>(ElementClicked, new object[] { type });
                parent.Add(createdElement);
                break;
            case ElementType.Action:
                break;
            case ElementType.Condition:
                break;
            case ElementType.Variable:
                break;
            case ElementType.Comment:
                break;
            case ElementType.Function:
                break;
            case ElementType.Trigger:
                break;
        }
    }

    /// <summary>
    /// Callback for Elements of the Event Sheet being clicked
    /// </summary>
    /// <param name="evt">The current Mouse Down Event.</param>
    /// <param name="parameters">Parameters Passed with the Callback.</param>
    void ElementClicked(MouseDownEvent evt, object[] parameters)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    void Compile()
    {

    }
}
