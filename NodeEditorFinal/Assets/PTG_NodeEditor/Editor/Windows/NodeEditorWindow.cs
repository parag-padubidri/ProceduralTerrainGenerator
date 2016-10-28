using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

public class NodeEditorWindow : EditorWindow
{
    public static NodeEditorWindow curWindow;
    public NodePropertyView propertyView;
    public NodeWorkView workView;

    public NodeGraph curGraph = null;

    public float viewPercentage = 0.75f;

    public static void InitEditorWindow()
    {
        curWindow = (NodeEditorWindow)EditorWindow.GetWindow<NodeEditorWindow>();

        //Customize & display Node Editor Window
        Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/PTG_NodeEditor/Resources/Textures/Editor/PTG_icon.png");
        GUIContent titleContent = new GUIContent("Node Editor", icon);
        curWindow.titleContent = titleContent;

        CreateViews();
    }



    void OnEnable()
    {

    }

    void OnDestroy()
    {

    }

    void Update()
    {

    }

    void OnGUI()
    {
        //Check for null views
        if (propertyView == null || workView == null)
        {
            CreateViews();
            return;
        }

        //Capture events
        Event e = Event.current;
        ProcessEvents(e);

        //Update work & property window positions
        workView.UpdateView(position, new Rect(0f, 0f, viewPercentage, 1f), e, curGraph);
        propertyView.UpdateView(new Rect(position.width, position.y, position.width, position.height),
            new Rect(viewPercentage, 0f, 1f - viewPercentage, 1f), e, curGraph);

        Repaint();
    }

    private static void CreateViews()
    {
        if (curWindow != null)
        {
            curWindow.propertyView = new NodePropertyView();
            curWindow.workView = new NodeWorkView();
        }
        else
        {
            curWindow = (NodeEditorWindow)EditorWindow.GetWindow<NodeEditorWindow>();
        }
    }

    //Capture & Process events
    void ProcessEvents(Event e)
    {
        if (e.type == EventType.keyDown && e.keyCode == KeyCode.LeftArrow)
        {
            viewPercentage -= 0.01f;
        }

        if (e.type == EventType.keyDown && e.keyCode == KeyCode.RightArrow)
        {
            viewPercentage += 0.01f;
        }
    }

}
