using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;

[Serializable]
public class NodeBase : ScriptableObject
{
    public bool isSelected = false;
    public string nodeName;
    public Rect nodeRect;
    public NodeGraph parentGraph;
    public NodeType nodeType;

    protected GUISkin nodeSkin;

    [Serializable]
    public class NodeInput
    {
        public bool isOccupied = false;
        public NodeBase inputNode;
    }

    [Serializable]
    public class NodeOutput
    {
        public bool isOccupied = false;
    }

    public virtual void InitNode()
    {

    }

    public virtual void UpdateNode(Event e, Rect viewRect)
    {
        ProcessEvents(e, viewRect);
    }

    #if UNITY_EDITOR
    public virtual void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        ProcessEvents(e, viewRect);

        if (!isSelected)
        {
            GUI.Box(nodeRect, nodeName, viewSkin.GetStyle("NodeDefault"));
        }
        else
        {
            GUI.Box(nodeRect, nodeName, viewSkin.GetStyle("NodeSelected"));
        }

        EditorUtility.SetDirty(this);
    }

    public virtual void DrawNodeProperties(Rect viewRect)
    {

    }
    #endif

    void ProcessEvents(Event e, Rect viewRect)
    {
        if (isSelected)
        {
            if (viewRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseDrag)
                {
                    nodeRect.x += e.delta.x;
                    nodeRect.y += e.delta.y;

                }
            } 
        }

    }
}
