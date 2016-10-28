using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System;

[Serializable]
public class Viewbase
{
    public string viewTitle;
    public Rect viewRect;

    protected GUISkin viewSkin;
    protected NodeGraph curGraph;

    public Viewbase(string title)
    {
        //Set title & skin
        viewTitle = title;
        GetEditorSkin();
    }

    //Set work & property view to 2/3 & 1/3
    public virtual void UpdateView(Rect editorRect, Rect percentRect, Event e, NodeGraph curGraph)
    {
        if (viewSkin == null)
        {
            GetEditorSkin();
            return;
        }

        //Set the current view graph
        this.curGraph = curGraph;

        //Update viewTitle
        if (curGraph != null)
        {
            viewTitle = curGraph.graphName;
        }
        else
        {
            viewTitle = "No Graph";
        }

        //Update view rectangle
        viewRect = new Rect(editorRect.x * percentRect.x, 
                            editorRect.y * percentRect.y, 
                            editorRect.width * percentRect.width, 
                            editorRect.height * percentRect.height);

       
    }

    public virtual void ProcessEvents(Event e)
    {

    }

    protected void GetEditorSkin()
    {
        viewSkin = (GUISkin)Resources.Load("GUISkins/EditorSkins/NodeEditorSkin");
    }
}
