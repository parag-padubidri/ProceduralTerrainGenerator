using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;

public class MapOutputNode : NodeBase
{
    public NodeInput inputA;
    float[,] noiseMap;
    Texture2D texture;
    public GameObject terrain = null;
    public float heightMultiplier = 40;

    public MapOutputNode()
    {
        inputA = new NodeInput();
    }    

    public override void InitNode()
    {
        base.InitNode();
        nodeType = NodeType.Output;
        nodeRect = new Rect(10f, 10f, 200f, 150f);
    }

    public override void UpdateNode(Event e, Rect viewRect)
    {
        base.UpdateNode(e, viewRect);
    }

#if UNITY_EDITOR
    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        base.UpdateNodeGUI(e, viewRect, viewSkin);

        //Display Map output

        //InputA
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

        //Check if connected
        if (inputA.isOccupied)
        {
            PerlinAdjustNode nodeA = (PerlinAdjustNode)inputA.inputNode;

            if (nodeA != null)
            {
                //Do something
                noiseMap = GenerateNoiseMap(nodeA.mapWidth, nodeA.mapHeight, nodeA.seed, nodeA.noiseScale, nodeA.octaves, nodeA.persistance, nodeA.lacunarity, nodeA.offset);
                                
                texture = DrawNoiseMap(noiseMap);
            }
        }

        //DrawLines
        DrawInputLines();
    }

    public override void DrawNodeProperties(Rect viewRect)
    {
        base.DrawNodeProperties(viewRect);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("NoiseMap");
        EditorGUILayout.LabelField("[MAP TEXTURE]");

       if (texture != null)
        {
            GUI.DrawTexture(new Rect(50f, 100f, 200, 200), texture, ScaleMode.ScaleToFit, true, 0.0F);
        }


        GUI.Label(new Rect(50f, 320f, 200, 20), "Select Plane");
        terrain = (GameObject)EditorGUI.ObjectField(new Rect(50f, 340f, 200, 16), terrain, typeof(GameObject), true);

        GUI.Label(new Rect(50f, 400f, 200, 20), "Height Multiplier");

        heightMultiplier = GUI.HorizontalSlider(new Rect(50f, 420f, 200, 20), heightMultiplier, -100f, 100f);

        GUI.Label(new Rect(50f, 440f, 200, 20), "Value : " +heightMultiplier.ToString());


        if (GUI.Button(new Rect(50f, 360f, 200, 20), "Generate Terrain"))
        {        
            MeshData terrainMesh = MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiplier);
            terrain.GetComponent<MeshFilter>().sharedMesh = terrainMesh.CreateMesh();

            if (terrain.GetComponent<MeshCollider>() != null)
            {
                DestroyImmediate(terrain.GetComponent<MeshCollider>());
            }
            terrain.AddComponent<MeshCollider>();
        }

        if (GUI.changed && terrain != null)
        {
            MeshData terrainMesh = MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiplier);
            terrain.GetComponent<MeshFilter>().sharedMesh = terrainMesh.CreateMesh();

            if (terrain.GetComponent<MeshCollider>() != null)
            {
                DestroyImmediate(terrain.GetComponent<MeshCollider>());
            }
            terrain.AddComponent<MeshCollider>();
        }



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

    //Generate Noise
    float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;


        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

    //Draw NoiseMap Texture
    public Texture2D DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        texture.SetPixels(colourMap);
        texture.Apply();

        GUI.DrawTexture(new Rect(nodeRect.x + 30f, nodeRect.y + 30f, 100, 100), texture, ScaleMode.ScaleToFit, true, 0.0F);

        return texture;
    }
}
