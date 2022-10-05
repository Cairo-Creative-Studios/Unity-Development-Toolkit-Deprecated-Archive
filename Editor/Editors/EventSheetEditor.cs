using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UDT.Scripting;

[CustomEditor(typeof(EventSheet), true)]
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
    private VisualTreeAsset contentTemplate = default(VisualTreeAsset);
    [SerializeField]
    private VisualTreeAsset blockTemplate = default(VisualTreeAsset);
    [SerializeField]
    private VisualTreeAsset commentTemplate = default(VisualTreeAsset);
    [SerializeField]
    private VisualTreeAsset eventTemplate = default(VisualTreeAsset);
    [SerializeField]
    private VisualTreeAsset eventContainerTemplate = default(VisualTreeAsset);
    [SerializeField]
    private VisualTreeAsset conditionTemplate = default(VisualTreeAsset);
    [SerializeField]
    private VisualTreeAsset actionTemplate = default(VisualTreeAsset);
    VisualElement root;
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
        root = contentTemplate.Instantiate();
        root = root[0];
        root.RegisterCallback<MouseDownEvent, object[]>(ElementClicked, new object[] { });
        rootVisualElement.Add(root);
        //contentContainer = blockTemplate.Instantiate();
        //root.Add(contentContainer);
    }

    void OnGUI()
    {
        e = Event.current;
        mousePosition = e.mousePosition;

        if (e.button == 1)
        {
            ShowContext(root);
        }
    }

    /// <summary>
    /// Creates an Element of the given Type
    /// </summary>
    /// <returns>The element.</returns>
    /// <param name="type">Type.</param>
    VisualElement CreateElement(ElementType type, VisualElement parent)
    {
        VisualElement createdElement = null;

        //Run Initialization Code for each created Element
        switch (type)
        {
            case ElementType.Event:
                createdElement = eventTemplate.CloneTree();
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
        return createdElement;
    }
    VisualElement CreateElement(VisualTreeAsset template, VisualElement parent)
    {
        VisualElement createdElement;
        createdElement = template.Instantiate();
        createdElement.RegisterCallback<MouseDownEvent, object[]>(ElementClicked, null);
        parent.Add(createdElement);
        return createdElement;
    }

    /// <summary>
    /// Callback for Elements of the Event Sheet being clicked
    /// </summary>
    /// <param name="evt">The current Mouse Down Event.</param>
    /// <param name="parameters">Parameters Passed with the Callback.</param>
    void ElementClicked(MouseDownEvent evt, object[] parameters)
    {
        if (evt.button == 1)
        {
            ShowContext((VisualElement)evt.target);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Compile()
    {

    }

    void ShowContext(VisualElement origin)
    {
        //Context Menu
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("New Event"), false, () =>
        {
            CreateElement(ElementType.Event, FindForTemplate(origin, "EventContainer"));
        });
        menu.ShowAsContext();
    }

    VisualElement FindForTemplate(VisualElement child, string name)
    {
        if (child != root)
        {
            if (child.name == "EventContainer")
                return child;
            else
            {
                VisualElement selectedElement = child;

                while (selectedElement.name != "EventContainer" && selectedElement != root)
                {
                    selectedElement = selectedElement.parent;
                }
                if (selectedElement != root)
                    return selectedElement;
            }
        }

        return root;
    }
}
