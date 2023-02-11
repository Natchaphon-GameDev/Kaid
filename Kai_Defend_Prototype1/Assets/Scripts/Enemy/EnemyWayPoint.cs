using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWayPoint : MonoBehaviour
{
    public Transform[] WayPoints { get; private set; }

    public static EnemyWayPoint Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        WayPoints = new Transform[transform.childCount];

        for (var i = 0; i < WayPoints.Length; i++)
        {
            WayPoints[i] = transform.GetChild(i);
        }
    }
}