using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class NodeMenus
{
    [MenuItem("Procedural Terrain Generator/Launch Node Editor")]
    public static void InitNodeEditor()
    {
        NodeEditorWindow.InitEditorWindow();
    }

}
