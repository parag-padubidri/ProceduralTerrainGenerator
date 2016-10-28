using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NodeWorkView : Viewbase {

    Vector2 mousePos;
    int deleteNodeID = 0;

    public NodeWorkView() : base("Work View") {}

    public override void UpdateView(Rect editorRect, Rect percentRect, Event e, NodeGraph curGraph)
    {
        base.UpdateView(editorRect, percentRect, e, curGraph);

        //Demarcate view
        GUI.Box(viewRect, viewTitle, viewSkin.GetStyle("viewBG"));

        //Draw Grid
        NodeUtils.DrawGrid(viewRect, 60f, 0.15f, Color.white);
        NodeUtils.DrawGrid(viewRect, 20f, 0.05f, Color.white);

        GUILayout.BeginArea(viewRect);
        if (curGraph != null)
        {
            curGraph.UpdateGraphGUI(e, viewRect, viewSkin);
        }

        GUILayout.EndArea();

        ProcessEvents(e);
    }

    public override void ProcessEvents(Event e)
    {
        base.ProcessEvents(e);

        if (viewRect.Contains(e.mousePosition))
        {
            if (e.button == 0)
            {
                if (e.type == EventType.MouseDown)
                {
                    //Debug.Log("Left clicked in " + viewTitle);
                }

                if (e.type == EventType.MouseDrag)
                {
                    //Debug.Log("Mouse dragged in " + viewTitle);
                }

                if (e.type == EventType.MouseUp)
                {
                    //Debug.Log("Mouse up in " + viewTitle);
                }
            }

            if (e.button == 1)
            {
                if (e.type == EventType.MouseDown)
                {
                    mousePos = e.mousePosition;
                    bool overNode = false;
                    if (curGraph != null)
                    {
                        if (curGraph.nodes.Count > 0)
                        {
                            for (int i = 0; i < curGraph.nodes.Count; i++)
                            {
                                if (curGraph.nodes[i].nodeRect.Contains(mousePos))
                                {
                                    deleteNodeID = i;
                                    overNode = true;
                                }
                            }
                        }
                    }

                    if (!overNode)
                    {
                        ProcessContextMenu(e, 0);
                    }
                    else
                    {
                        ProcessContextMenu(e, 1);
                    }

                    
                }
            }
        }
    }

    void ProcessContextMenu(Event e, int contextID)
    {
        GenericMenu menu = new GenericMenu();
        if (contextID == 0)
        {
            menu.AddItem(new GUIContent("Create Graph"), false, ContextCallBack, "0");
            menu.AddItem(new GUIContent("Load Graph"), false, ContextCallBack, "1");

            if (curGraph != null)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Unload Graph"), false, ContextCallBack, "2");

                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Map Input Node"), false, ContextCallBack, "3");
                menu.AddItem(new GUIContent("Perlin Adjust Node"), false, ContextCallBack, "4");
                menu.AddItem(new GUIContent("Map Output Node"), false, ContextCallBack, "5");
            }
        }

        if (contextID == 1)
        {
            if (curGraph != null)
            {                
                menu.AddItem(new GUIContent("Delete Node"), false, ContextCallBack, "6");
            }
        }

        menu.ShowAsContext();
        e.Use();
    }

    void ContextCallBack(object obj)
    {
        switch (obj.ToString())
        {
            case "0":                
                NodePopupWindow.InitNodePopup();
                break;

            case "1":
                NodeUtils.LoadGraph();
                break;

            case "2":
                NodeUtils.UnloadGraph();
                break;

            case "3":
                NodeUtils.CreateNode(curGraph, NodeType.Input, mousePos);
                break;

            case "4":
                NodeUtils.CreateNode(curGraph, NodeType.Adjust, mousePos);
                break;

            case "5":
                NodeUtils.CreateNode(curGraph, NodeType.Output, mousePos);
                break;

            case "6":
                NodeUtils.DeleteNode(deleteNodeID, curGraph);
                break;

            default:
                break;
        }
    }
}
