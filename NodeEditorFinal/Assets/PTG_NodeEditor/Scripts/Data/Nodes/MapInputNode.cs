using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;

[Serializable]
public class MapInputNode : NodeBase
{
    public float nodeValue;
    public NodeOutput output;

    public string mapWidthLabel = "";
    public string mapHeightLabel = "";

    public int mapWidth;
    public int mapHeight;

    public MapInputNode()
    {
        output = new NodeOutput();
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = NodeType.Input;
        nodeRect = new Rect(10f,10f,150f,150f);
    }

    public override void UpdateNode(Event e, Rect viewRect)
    {
        base.UpdateNode(e, viewRect);
    }

#if UNITY_EDITOR
    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        base.UpdateNodeGUI(e, viewRect, viewSkin);

        //Enter map dimensions
        GUI.Label(new Rect(nodeRect.x + 10f, nodeRect.y + 50f, 100, 20), "Map Width");
        GUI.Label(new Rect(nodeRect.x + 10f, nodeRect.y + 70f, 100, 20), "Map Height");

        mapWidthLabel = (GUI.TextField(new Rect(nodeRect.x + nodeRect.width - 50f, nodeRect.y + 50f, 40f, 20f), mapWidthLabel));
        mapHeightLabel = (GUI.TextField(new Rect(nodeRect.x + nodeRect.width - 50f, nodeRect.y + 70f, 40f, 20f), mapHeightLabel));

        if (!string.IsNullOrEmpty(mapWidthLabel))
        {
            int.TryParse(mapWidthLabel.ToString(), out mapWidth);
        }

        if (!string.IsNullOrEmpty(mapHeightLabel))
        {
            int.TryParse(mapHeightLabel.ToString(), out mapHeight);
        }


        if (GUI.Button(new Rect(nodeRect.x + nodeRect.width, nodeRect.y + (nodeRect.height * 0.5f) - 12f, 24f, 24f), "", viewSkin.GetStyle("NodeOutput")))
        {
            if (parentGraph != null)
            {
                parentGraph.wantsConnection = true;
                parentGraph.connectionNode = this;
            }
        }
    }

    public override void DrawNodeProperties(Rect viewRect)
    {
        base.DrawNodeProperties(viewRect);
        //nodeValue = EditorGUILayout.FloatField("Float Value :", nodeValue);

        //Display Node Properties
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Map Width : " + mapWidthLabel);
        EditorGUILayout.Space();        
        EditorGUILayout.LabelField("Map Height : " + mapHeightLabel);
        EditorGUILayout.EndVertical();

    }
#endif

}
