using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;


public class NodePropertyView : Viewbase
{
    public bool showProperties = false;

    public NodePropertyView() : base("Property View") { }

    public override void UpdateView(Rect editorRect, Rect percentRect, Event e, NodeGraph curGraph)
    {
        base.UpdateView(editorRect, percentRect, e, curGraph);

        //Demarcate view
        GUI.Box(viewRect, viewTitle, viewSkin.GetStyle("viewBG"));

        GUILayout.BeginArea(viewRect);
        GUILayout.Space(60);

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);

        if (curGraph == null || !curGraph.showProperties)
        {
            EditorGUILayout.LabelField("NONE");
        }
        else if (curGraph.selectedNode != null)
        {
            curGraph.selectedNode.DrawNodeProperties(viewRect);
        }

        GUILayout.Space(30);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();


        ProcessEvents(e);

        showProperties = false;
        if (e.type == EventType.Layout)
        {
            if (curGraph != null && curGraph.selectedNode != null)
            {
                showProperties = true;
            }
            else
            {
                showProperties = false;
            }
        }
        else
        {
            showProperties = false;
        }
    }

    public override void ProcessEvents(Event e)
    {
        base.ProcessEvents(e);

        if (viewRect.Contains(e.mousePosition))
        {
            //Debug.Log("Inside" + viewTitle);
        }
    }

}
