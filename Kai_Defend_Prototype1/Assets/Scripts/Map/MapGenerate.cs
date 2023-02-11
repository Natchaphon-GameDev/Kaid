using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerate : MonoBehaviour
{
    public GameObject mapArea;

    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    private List<GameObject> mapAreas = new List<GameObject>();
    private List<GameObject> pathAreas = new List<GameObject>();

    private void Start()
    {
        GenerateMap();
    }

    private List<GameObject> getTopEdgeAreas()
    {
        var topEdgeAreas = new List<GameObject>();

        for (var i = mapWidth * (mapHeight - 1); i < mapHeight*mapHeight; i++)
        {
            topEdgeAreas.Add(mapAreas[i]);
        }
        
        return topEdgeAreas;
    }

    private List<GameObject> getBottomEdgeAreas()
    {
        var bottomEdgeAreas = new List<GameObject>();
        
        for (var i = 0; i < mapWidth; i++)
        {
            bottomEdgeAreas.Add(mapAreas[i]);
        }
        return bottomEdgeAreas;
    }

    private void GenerateMap()
    {
        for (var i = 0; i < mapHeight; i++)
        {
            for (var j = 0; j < mapWidth; j++)
            {
                var newArea = Instantiate(mapArea);
                
                mapAreas.Add(newArea);

                newArea.transform.position = new Vector2(j, i);
            }
        }

        var topEdgeAreas = getTopEdgeAreas();
        var bottomEdgeAreas = getBottomEdgeAreas();
    }
}

