using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(TilemapPositionMapper))]
public class TilemapPositionMapperEditor : Editor
{
    public readonly string NAME_PREFIX = "LN";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TilemapPositionMapper mapper = (TilemapPositionMapper)target;

        if(GUILayout.Button("Refresh Location Nodes"))
        {
            RenameLocationNodes(mapper);
        }
    }

    private void RenameLocationNodes(TilemapPositionMapper mapper)
    {
        Tilemap tilemap = mapper.GetComponent<Tilemap>();

        //Loop through all children of the mapper and
        //rename it based on its position on the tilemap
        for(int i = 0; i < mapper.transform.childCount; i++)
        {
            var child = mapper.transform.GetChild(i);
            //Get the nearest grid position of this node
            var gridPosition = (Vector2Int)tilemap.WorldToCell(child.position);

            //Set name to grid position
            child.name = NAME_PREFIX + gridPosition.ToString();
        }
    }
}
