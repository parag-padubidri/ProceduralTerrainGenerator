using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;

[Serializable]
public class PerlinAdjustNode : NodeBase
{
    //Map inputs
    public int mapWidth;
    public int mapHeight;

    //Perlin Inputs
    public string noiseScaleLabel = "";
    public float noiseScale;

    public string octavesLabel = "";
    public int octaves;

    public string persistanceLabel = "";
    [Range(0, 1)]
    public float persistance;

    public string lacunarityLabel = "";
    public float lacunarity;

    public string seedLabel = "";
    public int seed;


    public float offsetX, offsetY;
    public string offsetXLabel = "";
    public string offsetYLabel = "";
    public Vector2 offset;

    //Nodes
    public NodeOutput output;
    public NodeInput inputA;

    public PerlinAdjustNode()
    {
        output = new NodeOutput();
        inputA = new NodeInput();
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = NodeType.Adjust;
        nodeRect = new Rect(10f, 10f, 200f, 270f);
    }

    public override void UpdateNode(Event e, Rect viewRect)
    {
        base.UpdateNode(e, viewRect);
    }

#if UNITY_EDITOR
    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        base.UpdateNodeGUI(e, viewRect, viewSkin);        

        //Enter Perlin Adjusters
        GUI.Label(new Rect(nodeRect.x + 10f, nodeRect.y + 50f, 100, 20), "Noise Scale");
        GUI.Label(new Rect(nodeRect.x + 10f, nodeRect.y + 80f, 100, 20), "Octaves");
        GUI.Label(new Rect(nodeRect.x + 10f, nodeRect.y + 110f, 100, 20), "Persistance");
        GUI.Label(new Rect(nodeRect.x + 10f, nodeRect.y + 140f, 100, 20), "Lacunarity");
        GUI.Label(new Rect(nodeRect.x + 10f, nodeRect.y + 170f, 100, 20), "Seed");
        GUI.Label(new Rect(nodeRect.x + 10f, nodeRect.y + 200f, 100, 20), "OffsetX");
        GUI.Label(new Rect(nodeRect.x + 10f, nodeRect.y + 230f, 100, 20), "OffsetY");        

        noiseScaleLabel = (GUI.TextField(new Rect(nodeRect.x + nodeRect.width - 50f, nodeRect.y + 50f, 40f, 20f), noiseScaleLabel));
        octavesLabel = (GUI.TextField(new Rect(nodeRect.x + nodeRect.width - 50f, nodeRect.y + 80f, 40f, 20f), octavesLabel));
        persistance = GUI.HorizontalSlider(new Rect(nodeRect.x + nodeRect.width - 50f, nodeRect.y + 110f, 40f, 20f), persistance, 0f,1f);

        lacunarityLabel = (GUI.TextField(new Rect(nodeRect.x + nodeRect.width - 50f, nodeRect.y + 140f, 40f, 20f), lacunarityLabel));
        seedLabel = (GUI.TextField(new Rect(nodeRect.x + nodeRect.width - 50f, nodeRect.y + 170f, 40f, 20f), seedLabel));
        offsetXLabel = (GUI.TextField(new Rect(nodeRect.x + nodeRect.width - 50f, nodeRect.y + 200f, 40f, 20f), offsetXLabel));
        offsetYLabel = (GUI.TextField(new Rect(nodeRect.x + nodeRect.width - 50f, nodeRect.y + 230f, 40f, 20f), offsetYLabel));

        if (!string.IsNullOrEmpty(noiseScaleLabel))
        {
            float.TryParse(noiseScaleLabel.ToString(), out noiseScale);
        }

        if (!string.IsNullOrEmpty(octavesLabel))
        {
            int.TryParse(octavesLabel.ToString(), out octaves);
        }


        if (!string.IsNullOrEmpty(lacunarityLabel))
        {
            float.TryParse(lacunarityLabel.ToString(), out lacunarity);
        }

        if (!string.IsNullOrEmpty(seedLabel))
        {
            int.TryParse(seedLabel.ToString(), out seed);
        }

        if (!string.IsNullOrEmpty(offsetXLabel) && !string.IsNullOrEmpty(offsetYLabel))
        {
            float.TryParse(offsetXLabel.ToString(), out offsetX);
            float.TryParse(offsetYLabel.ToString(), out offsetY);
            offset = new Vector2(offsetX, offsetY);
        }


        //Output
        if (GUI.Button(new Rect(nodeRect.x + nodeRect.width, nodeRect.y + (nodeRect.height * 0.5f) - 12f, 24f, 24f), "", viewSkin.GetStyle("NodeOutput")))
        {
            if (parentGraph != null)
            {
                parentGraph.wantsConnection = true;
                parentGraph.connectionNode = this;
            }
        }


        //Input A
        if (GUI.Button(new Rect(nodeRect.x - 24f, (nodeRect.y + (nodeRect.height * 0.5f)) - 14f, 24f, 24f), "", viewSkin.GetStyle("NodeInput")))
        {
            if (parentGraph != null)
            {
                inputA.inputNode = parentGraph.connectionNode;
                inputA.isOccupied = inputA.inputNode != null ? true : false;

                parentGraph.wantsConnection = false;
                parentGraph.connectionNode = null;
            }
        }


        if (inputA.isOccupied)
        {
            MapInputNode nodeA = (MapInputNode)inputA.inputNode;

            if (nodeA != null)
            {
                mapWidth = nodeA.mapWidth;
                mapHeight = nodeA.mapHeight;
            }
        }

        //DrawLines
        DrawInputLines();

    }

    public override void DrawNodeProperties(Rect viewRect)
    {
        base.DrawNodeProperties(viewRect);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Noise Scale : " + noiseScale);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Octaves : " + octaves);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Persistance : " + persistance);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Lacunarity : " + lacunarity);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Seed : " + seed);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Offset X : " + offsetX  + " Offset Y : " + offsetY);
        EditorGUILayout.EndVertical();
    }
#endif

    void DrawInputLines()
    {
        if (inputA.isOccupied && inputA.inputNode != null)
        {
            DrawLine(inputA, 1f); 
        }
        else
        {
            inputA.isOccupied = false;
        }
    }

    void DrawLine(NodeInput curInput, float inputID)
    {
        Handles.BeginGUI();
        Handles.color = Color.white;
        Handles.DrawLine(new Vector3(curInput.inputNode.nodeRect.x + curInput.inputNode.nodeRect.width + 24f,
                                     curInput.inputNode.nodeRect.y + (curInput.inputNode.nodeRect.height * 0.5f), 0f),
                         new Vector3(nodeRect.x - 24f, (nodeRect.y + (nodeRect.height * 0.5f) * inputID), 0f));

        Handles.EndGUI();
    }

}
